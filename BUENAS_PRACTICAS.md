// Este archivo muestra las buenas prácticas implementadas en el proyecto

/*
 * ARQUITECTURA DE 3 CAPAS IMPLEMENTADA
 * ====================================
 * 
 * 1. CAPA DE PRESENTACIÓN (Views + Controllers)
 *    - Views/Auth/Login.cshtml
 *    - Views/Encuesta/Index.cshtml
 *    - Views/EditaEncuesta/Index.cshtml
 *    - Controllers/AuthController.cs
 *    - Controllers/EncuestaController.cs
 *    - Controllers/EditaEncuestaController.cs
 * 
 * 2. CAPA DE LÓGICA DE NEGOCIOS (Services)
 *    - Services/AuthService.cs (autenticación)
 *    - Services/EncuestaService.cs (gestión encuestas)
 *    - Services/RespuestaService.cs (gestión respuestas)
 * 
 * 3. CAPA DE DATOS (Models)
 *    - Models/Usuario.cs
 *    - Models/Encuesta.cs
 *    - Models/Pregunta.cs
 *    - Models/Respuesta.cs
 * 
 * FLUJO DE UNA SOLICITUD:
 * ======================
 * 
 * Usuario solicita página
 *        ↓
 * Controller recibe solicitud
 *        ↓
 * Controller llama al Service
 *        ↓
 * Service procesa lógica de negocio
 *        ↓
 * Service retorna datos
 *        ↓
 * Controller pasa datos a View
 *        ↓
 * View renderiza respuesta HTML
 */

// BUENAS PRÁCTICAS IMPLEMENTADAS:

// 1. INYECCIÓN DE DEPENDENCIAS
//    En Program.cs:
//    builder.Services.AddScoped<IAuthService, AuthService>();
//    
//    En Controller:
//    public AuthController(IAuthService authService)
//    {
//        _authService = authService;
//    }

// 2. INTERFACES PARA ABSTRACCIÓN
//    - IAuthService
//    - IEncuestaService
//    - IRespuestaService

// 3. MÉTODOS ASYNC/AWAIT
//    public async Task<IActionResult> Login(LoginViewModel model)
//    {
//        var (success, message, usuario) = await _authService.LoginAsync(...);
//    }

// 4. VALIDACIÓN DE ENTRADA
//    if (!ViewData.ModelState.IsValid)
//    {
//        return View(model);
//    }

// 5. MANEJO DE ERRORES CON TUPLAS
//    var (success, message, usuarioId) = await _authService.LoginAsync(email, pwd);
//    if (!success)
//    {
//        ModelState.AddModelError(string.Empty, message);
//    }

// 6. SESIONES PARA AUTENTICACIÓN
//    HttpContext.Session.SetInt32("UserId", usuario.Id);
//    HttpContext.Session.SetString("UserName", usuario.Nombre);

// 7. HELPERS REUTILIZABLES
//    SessionHelper.IsAuthenticated(HttpContext.Session)
//    SessionHelper.GetUserId(HttpContext.Session)

// 8. COMENTARIOS XML PARA DOCUMENTACIÓN
//    /// <summary>
//    /// Autentica un usuario con email y contraseña
//    /// </summary>

// 9. NAMING CONVENTIONS
//    - Clases: PascalCase (Usuario, EncuestaService)
//    - Variables: camelCase (usuario, encuestaService)
//    - Constantes: UPPER_CASE
//    - Métodos privados: Prefijo underscore (_usuarios)
//    - Métodos async: Sufijo Async (LoginAsync)

// 10. ENUMS PARA VALORES FIJOS
//     public enum TipoUsuario { Administrador = 1, Evaluador = 2 }
//     public enum EstadoEncuesta { Borrador = 1, Publicada = 2, ... }

// 11. VALIDACIÓN CON TUPLE RETURNS
//     public Task<(bool Success, string Message)> DeleteEncuestaAsync(int id)
//     {
//         // ...
//         return Task.FromResult<(bool, string)>(
//             (false, "No se pueden eliminar encuestas con respuestas")
//         );
//     }

// 12. PROTECCIÓN DE DATOS SENSIBLES
//     if (encuesta.UsuarioCreadorId != GetCurrentUserId())
//         return Forbid();

// 13. SEPARACIÓN DE RESPONSABILIDADES
//     - AuthController: Solo autenticación
//     - EncuestaController: Solo operaciones de lectura de encuestas
//     - EditaEncuestaController: Solo operaciones de edición

// 14. DISEÑO RESPONSIVE CON TAILWIND CSS
//     <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
//     <input class="w-full px-4 py-2 border rounded-lg focus:ring-2">

// PRÓXIMAS MEJORAS RECOMENDADAS:
// ==============================

// 1. IMPLEMENTAR ENTITY FRAMEWORK CORE
//    public class AppDbContext : DbContext
//    {
//        public DbSet<Usuario> Usuarios { get; set; }
//        public DbSet<Encuesta> Encuestas { get; set; }
//        // ...
//    }

// 2. HASHING DE CONTRASEÑAS
//    using Microsoft.AspNetCore.Identity;
//    var hashedPassword = new PasswordHasher<Usuario>().HashPassword(usuario, contraseña);

// 3. AUTENTICACIÓN CON COOKIES
//    [Authorize]
//    public class EncuestaController : Controller { }

// 4. VALIDATION ATTRIBUTES
//    public class Usuario
//    {
//        [Required(ErrorMessage = "El nombre es requerido")]
//        [StringLength(100, MinimumLength = 3)]
//        public string Nombre { get; set; }
//    }

// 5. MAPEO AUTOMÁTICO CON AUTOMAPPER
//    IMapper mapper.Map<EncuestaDto>(encuesta);

// 6. LOGGING
//    private readonly ILogger<AuthService> _logger;
//    _logger.LogError("Error en login: {Email}", email);

// 7. REPOSITORIO PATTERN
//    public interface IRepository<T>
//    {
//        Task<T> GetByIdAsync(int id);
//        Task<IEnumerable<T>> GetAllAsync();
//        // ...
//    }

// 8. UNIT OF WORK PATTERN
//    public interface IUnitOfWork
//    {
//        IUsuarioRepository Usuarios { get; }
//        IEncuestaRepository Encuestas { get; }
//        Task SaveAsync();
//    }

// 9. VALIDACIÓN CON FLUENT VALIDATION
//    RuleFor(u => u.Email).EmailAddress().NotEmpty();

// 10. CACHÉ DISTRIBUIDO
//     IDistributedCache _cache;
//     await _cache.SetStringAsync("encuesta_1", jsonEncuesta);

/*
 * ESTRUCTURA DE CARPETAS FINAL RECOMENDADA:
 * 
 * EncuestasEvaluacionLiderazgo/
 * ├── Controllers/
 * ├── Models/
 * │   ├── Entities/
 * │   ├── ViewModels/
 * │   └── DTOs/
 * ├── Services/
 * │   ├── Interfaces/
 * │   └── Implementations/
 * ├── Data/
 * │   ├── Context/
 * │   └── Repositories/
 * ├── Utilities/
 * │   ├── Helpers/
 * │   └── Extensions/
 * ├── Middleware/
 * ├── Views/
 * ├── wwwroot/
 * ├── Tests/
 * │   ├── UnitTests/
 * │   └── IntegrationTests/
 * ├── Program.cs
 * ├── appsettings.json
 * ├── README.md
 * └── DOCUMENTACION.md
 */
