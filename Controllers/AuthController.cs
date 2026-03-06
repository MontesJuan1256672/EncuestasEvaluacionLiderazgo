using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using EncuestasEvaluacionLiderazgo.Services;
using EncuestasEvaluacionLiderazgo.Models;
using EncuestasEvaluacionLiderazgo.Utilities;
using EncuestasEvaluacionLiderazgo.Data;

namespace EncuestasEvaluacionLiderazgo.Controllers
{
    /// <summary>
    /// Controlador responsable de manejar la autenticación de usuarios
    /// </summary>
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// GET: /Auth/Login
        /// Muestra la página de login.
        /// Recibe parámetros encriptados por QueryString: Id (idPersonal), loc (idCentro), mat (noEmp)
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            /*GET muestra el formulario
             * Lee los query strings encriptados, los desencripta, 
             * los guarda en sesión y muestra la vista de login
            */

            // Si el usuario ya está autenticado, redirigir al home
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            //IdPersonalDWH=WeRac2RviU0%3d              // 49395 = cE0rFzF1yTk=
            //IdCentro=JBDCu2wqR68%3d
            //noEmp=RYoAmK/HOdU=                        // 14532 = myN8m0ACXhvMihPlWAkr9w==    

            //?Id=WeRac2RviU0%3d&loc=JBDCu2wqR68%3d&mat=RYoAmK/HOdU=
            // Leer parámetros encriptados del QueryString
            string idPersonal = Request.Query["Id"].ToString();
            string idCentro = Request.Query["loc"].ToString();
            string noEmp = Request.Query["mat"].ToString();

            // Desencriptar parámetros si vienen en el QueryString
            if (!string.IsNullOrEmpty(idPersonal) && !string.IsNullOrEmpty(idCentro) && !string.IsNullOrEmpty(noEmp))
            {
                try
                {
                    // Intentar desencriptar con TelvistaServices (AES)
                    idPersonal = Desencripta.idPersonalDecrypt(idPersonal, "");
                    idCentro = Desencripta.idPersonalDecrypt(idCentro, "");
                    noEmp = Desencripta.idPersonalDecrypt(noEmp, "");
                }
                catch
                {
                    // Si falla, intentar desencriptar con CENTREX (DES)
                    idPersonal = Desencripta.DecryptWitValueCENTREX("Id", idPersonal);
                    idCentro = Desencripta.DecryptWitValueCENTREX("loc", idCentro);
                    noEmp = Desencripta.DecryptWitValueCENTREX("mat", noEmp);
                }

                // Guardar datos desencriptados en Session para uso durante la sesión
                HttpContext.Session.SetString("IdPersonal", idPersonal);
                HttpContext.Session.SetString("IdCentro", idCentro);
                HttpContext.Session.SetString("NoEmp", noEmp);
            }

            // Cargar combos para la vista
            CargarCombosLogin();

            return View();
        }

        /// <summary>
        /// Carga los datos de los combos del formulario de login en ViewBag
        /// </summary>
        private void CargarCombosLogin()
        {
            try
            {
                var tiposEvaluacion = FL.TraeTiposEvaluacion();
                ViewBag.TiposEvaluacion = tiposEvaluacion?.Tables.Count > 0 ? tiposEvaluacion.Tables[0] : null;
            }
            catch
            {
                ViewBag.TiposEvaluacion = null;
            }

            CargarCiudades();
        }

        /// <summary>
        /// Carga la lista de ciudades/centros disponibles
        /// </summary>
        private void CargarCiudades()
        {
            try
            {
                var ciudades = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Seleccione ciudad --" },
                    new SelectListItem { Value = "17", Text = "Tijuana" },
                    new SelectListItem { Value = "18", Text = "Mexicali" },
                    new SelectListItem { Value = "19", Text = "CDMX" }
                };

                ViewBag.Ciudades = ciudades;
            }
            catch (Exception ex)
            {
                ViewBag.Ciudades = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Error al cargar ciudades" }
                };
            }
        }

        /// <summary>
        /// POST: /Auth/Login
        /// Procesa el login del usuario
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                int tipoAcceso = int.TryParse(model.TipoAcceso, out int ta) ? ta : 1;

                // Leer datos desencriptados de sesión (vienen del GET con query strings)
                string idPersonal = HttpContext.Session.GetString("IdPersonal") ?? "";
                string idCentro = HttpContext.Session.GetString("IdCentro") ?? "";
                string noEmp = HttpContext.Session.GetString("NoEmp") ?? "";

                // Guardar tipo de encuesta y centro en sesión
                HttpContext.Session.SetString("IdTipoEnc", model.CmbTipoEnc ?? "");
                HttpContext.Session.SetString("IdCentro", model.CmbCentro ?? "");
                HttpContext.Session.SetString("TipoAcceso", tipoAcceso.ToString());

                if (tipoAcceso == 1)
                {
                    // Acceso Empleado: validar clave de acceso
                    if (string.IsNullOrWhiteSpace(model.TxtClave))
                    {
                        ModelState.AddModelError(string.Empty, "Favor de colocar la clave de acceso");
                        CargarCombosLogin();
                        return View(model);
                    }

                    // Validar clave contra BD: traer todos los tipos y filtrar por ID y clave
                    var tiposEval = FL.TraeTiposEvaluacion();
                    bool claveValida = false;
                    if (tiposEval?.Tables.Count > 0 && tiposEval.Tables[0].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow row in tiposEval.Tables[0].Rows)
                        {
                            if (row["IdTipoEvaluacion"].ToString() == model.CmbTipoEnc
                                && row["cClaveAcceso"].ToString().Trim() == model.TxtClave.Trim())
                            {
                                claveValida = true;
                                break;
                            }
                        }
                    }

                    if (!claveValida)
                    {
                        ModelState.AddModelError(string.Empty, "Favor de colocar una clave de acceso válida");
                        CargarCombosLogin();
                        return View(model);
                    }

                    // Sesión para empleado
                    HttpContext.Session.SetString("Termino", "False");
                    HttpContext.Session.SetInt32("UserId", 1);
                    HttpContext.Session.SetString("UserName", "Empleado");
                    HttpContext.Session.SetInt32("UserType", (int)TipoUsuario.Empleado);

                    // Si viene de servidor externo, guardar IdPersonalDWH; si no, "0"
                    HttpContext.Session.SetString("IdPersonalDWH", string.IsNullOrEmpty(noEmp) ? "0" : noEmp);

                    // Redirigir a la encuesta
                    return RedirectToAction("Details", "Encuesta", new { id = 1, filtroTipo = model.CmbTipoEnc });
                }
                else if (tipoAcceso == 2)
                {
                    // Acceso Administrador: validar que el IdPersonal esté en la lista de autorizados
                    var dsAdmins = FL.TraeUsuariosAdministradores();
                    var administradoresAutorizados = new List<string>();
                    if (dsAdmins?.Tables.Count > 0)
                    {
                        foreach (System.Data.DataRow row in dsAdmins.Tables[0].Rows)
                        {
                            administradoresAutorizados.Add(row["IdPErsonalDWH"].ToString().Trim());
                        }
                    }

                    if (string.IsNullOrEmpty(idPersonal) || !administradoresAutorizados.Contains(idPersonal))
                    {
                        ModelState.AddModelError(string.Empty, "Acceso denegado. No tiene permisos de administrador.");
                        CargarCombosLogin();
                        return View(model);
                    }

                    HttpContext.Session.SetString("IdPersonalDWH", idPersonal);
                    HttpContext.Session.SetInt32("UserId", 1);
                    HttpContext.Session.SetString("UserName", "Administrador");
                    HttpContext.Session.SetInt32("UserType", (int)TipoUsuario.Administrador);

                    return RedirectToAction("Index", "Home");
                }
                else if (tipoAcceso == 3)
                {
                    // Acceso Consulta
                    HttpContext.Session.SetInt32("UserId", 1);
                    HttpContext.Session.SetString("UserName", "Consulta");
                    HttpContext.Session.SetInt32("UserType", (int)TipoUsuario.Consulta);

                    return RedirectToAction("Index", "Reportes");
                }

                ModelState.AddModelError(string.Empty, "Tipo de acceso no válido");
                CargarCombosLogin();
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error en página: " + ex.Message);
                CargarCombosLogin();
                return View(model);
            }
        }

        /// <summary>
        /// GET: /Auth/Logout
        /// Cierra la sesión del usuario
        /// </summary>
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// GET: /Auth/Register
        /// Muestra la página de registro
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// POST: /Auth/Register
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _authService.RegisterAsync(
                model.Nombre,
                model.Email,
                model.Contraseña,
                model.TipoUsuario
            );

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }

            TempData["Success"] = "Registro exitoso. Por favor inicia sesión.";
            return RedirectToAction("Login");
        }
    }

    /// <summary>
    /// ViewModel para el formulario de login
    /// </summary>
    public class LoginViewModel
    {
        public string TipoAcceso { get; set; }
        public string CmbTipoEnc { get; set; }
        public string CmbCentro { get; set; }
        public string TxtClave { get; set; }
    }

    /// <summary>
    /// ViewModel para el formulario de registro
    /// </summary>
    public class RegisterViewModel
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public string ConfirmarContraseña { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
    }
}
