# 📊 Evaluación de Liderazgo - Sistema MVC

Sistema web escalable para crear y gestionar encuestas de evaluación de competencias de liderazgo, desarrollado con **ASP.NET Core MVC** y **Tailwind CSS**.

## 🚀 Características

✅ **Arquitectura de 3 capas** - Modelos, Servicios, Controladores y Vistas  
✅ **Autenticación de usuarios** - Login/Register con diferentes tipos de usuario  
✅ **Gestión de encuestas** - Crear, editar, publicar y responder encuestas  
✅ **Múltiples tipos de preguntas** - Texto, opción única, múltiple, escala  
✅ **Soporte bilingüe** - Encuestas y preguntas en Español e Inglés con switch dinámico  
✅ **Gestión de personas a evaluar** - CRUD completo con búsqueda por empleado  
✅ **Cargas dinámicas AJAX** - Comboboxes que se actualizan sin recargar la página  
✅ **Diseño responsivo** - Tailwind CSS para interfaz moderna y limpia  
✅ **Código bien estructurado** - Interfaces, servicios, capas de datos  
✅ **Fácil de escalar** - Preparado para integración con BD real  

## 📋 Requisitos

- **.NET 6.0** o superior
- **Visual Studio** o **Visual Studio Code**
- **Navegador moderno** (Chrome, Firefox, Edge, Safari)

## ⚡ Inicio Rápido

### 1. Clonar o descargar el proyecto

```bash
cd c:\DesarrolloAdministrativo\AplicacionesWeb\EncuestasEvaluacionLiderazgo
```

### 2. Restaurar paquetes NuGet

```bash
dotnet restore
```

### 3. Ejecutar la aplicación

```bash
dotnet run
```

La aplicación estará disponible en:
- **HTTPS**: `https://localhost:5032`
- **HTTP**: `http://localhost:5032`

## 🔐 Credenciales de Prueba

### Administrador
- **Email**: `admin@test.com`
- **Contraseña**: `Admin@123`

### Evaluador
- **Email**: `evaluador@test.com`
- **Contraseña**: `Eval@123`

## 📁 Estructura del Proyecto

```
EncuestasEvaluacionLiderazgo/
├── Controllers/
│   ├── AuthController.cs          # Autenticación
│   ├── EncuestaController.cs      # Gestión de encuestas
│   ├── EditaEncuestaController.cs # Edición de encuestas
│   ├── PersonasEvaluarController.cs # Gestión de personas a evaluar
│   ├── ReportesController.cs      # Reportes de evaluación
│   └── HomeController.cs          # Página de inicio
│
├── Models/
│   ├── Usuario.cs                 # Usuario del sistema
│   ├── Encuesta.cs                # Encuesta
│   ├── Pregunta.cs                # Pregunta de encuesta
│   ├── OpcionRespuesta.cs         # Opción de respuesta
│   ├── Respuesta.cs               # Respuesta de usuario
│   ├── PersonaEvaluar.cs          # Persona a ser evaluada
│   ├── EncuestaIndexViewModel.cs  # ViewModel para listado de encuestas
│   ├── EncuestaDetailsViewModel.cs# ViewModel para detalles de encuesta
│   ├── PreguntaViewModel.cs       # ViewModel para preguntas (bilingüe)
│   └── ReportesIndexViewModel.cs  # ViewModel para reportes
│
├── Data/
│   ├── FL.cs                      # Capa de Lógica de Negocio
│   └── DL.cs                      # Capa de Datos (SQL Server)
│
├── Services/
│   ├── IAuthService.cs            # Interfaz de autenticación
│   ├── AuthService.cs             # Implementación de autenticación
│   ├── IEncuestaService.cs        # Interfaz de encuestas
│   ├── EncuestaService.cs         # Implementación de encuestas
│   ├── IRespuestaService.cs       # Interfaz de respuestas
│   └── RespuestaService.cs        # Implementación de respuestas
│
├── Utilities/
│   └── SessionHelper.cs           # Utilidades de sesión
│
├── Views/
│   ├── Auth/
│   │   ├── Login.cshtml           # Página de login
│   │   └── Register.cshtml        # Página de registro
│   ├── Encuesta/
│   │   ├── Index.cshtml           # Listado de encuestas
│   │   ├── Index.cshtml.cs        # Code-behind para Index
│   │   ├── Details.cshtml         # Responder encuesta (con switch idioma)
│   │   ├── Details.cshtml.cs      # Code-behind con métodos helper
│   │   └── Create.cshtml          # Crear encuesta
│   ├── EditaEncuesta/
│   │   ├── Index.cshtml           # Editar encuesta
│   │   └── Index.cshtml.cs        # Code-behind para Index
│   ├── PersonasEvaluar/
│   │   ├── Index.cshtml           # Listado de personas
│   │   └── Create.cshtml          # Agregar persona a evaluar
│   ├── Home/
│   │   ├── Index.cshtml           # Página de inicio
│   │   └── Index.cshtml.cs        # Code-behind para Index
│   ├── Reportes/
│   │   └── Index.cshtml           # Panel de reportes
│   └── Shared/
│       ├── _Layout.cshtml         # Layout principal
│       ├── _Layout.cshtml.css     # Estilos CSS
│       ├── _ValidationScriptsPartial.cshtml # Scripts de validación
│       └── Error.cshtml           # Página de error
│
├── wwwroot/
│   ├── css/
│   │   └── site.css               # Estilos personalizados
│   ├── js/
│   │   └── site.js                # Scripts personalizados
│   └── lib/
│       ├── bootstrap/             # Bootstrap UI components
│       ├── jquery/                # jQuery library
│       └── jquery-validation/     # jQuery validation
│
├── Program.cs                     # Configuración de la app
├── appsettings.json              # Configuración
├── appsettings.Development.json  # Configuración de desarrollo
├── EncuestasEvaluacionLiderazgo.csproj # Archivo de proyecto
└── README.md                      # Este archivo
```

## 🎯 Flujo de Uso

### Para Administradores:
1. Login con credenciales admin
2. **Gestionar Personas a Evaluar:**
   - Ir a "Personas a Evaluar"
   - Seleccionar tipo de evaluación
   - Buscar empleado por número (AJAX)
   - Agregar persona a la lista
   - Eliminar personas (soft-delete)
3. **Gestionar Encuestas:**
   - Crear nueva encuesta
   - Agregar preguntas en español (con opción de inglés)
   - Publicar encuesta
4. **Ver Reportes:**
   - Filtrar por tipo de evaluación
   - Filtrar por ciudad
   - Seleccionar persona a evaluar
   - Analizar respuestas

### Para Evaluadores:
1. Login con credenciales evaluador
2. Ver encuestas disponibles
3. Seleccionar encuesta para responder
4. **Responder Encuesta:**
   - Switch de idioma (Español/English)
   - Leer instrucciones en idioma seleccionado
   - Seleccionar persona a evaluar
   - Calificar cada pregunta (escala 1-5)
   - Agregar comentarios opcionales
   - Enviar respuestas

## 🛠️ Componentes Principales

### AuthController
Maneja la autenticación de usuarios:
- `Login()` - Validación de credenciales
- `Register()` - Crear nuevo usuario
- `Logout()` - Cerrar sesión

### EncuestaController
Gestiona encuestas:
- `Index()` - Listar encuestas
- `Create()` - Crear nueva encuesta
- `Details()` - Ver y responder encuesta
- `Submit()` - Guardar respuestas

### EditaEncuestaController
Edición y administración:
- `Index()` - Editar encuesta
- `Update()` - Guardar cambios
- `Publish()` - Publicar encuesta
- `Delete()` - Eliminar encuesta

### PersonasEvaluarController
Gestión de personas a evaluar:
- `Index(string idTipoEvaluacion)` - Listar personas filtradas por tipo de evaluación
- `Create()` - Mostrar formulario para agregar persona
- `GetEmpleado(string numeroEmpleado, string ciudad)` - AJAX para búsqueda de empleados desde DWH
- `GuardarPersona(int idTipoEncuesta, int idPersonal)` - AJAX para guardar persona a evaluar
- `Delete(int id)` - Soft-delete (marca como inactiva)
- Métodos helper: `CargarCiudades()` - Retorna ciudades disponibles (17/18/19), `ObtenerTiposEvaluacion()` - Retorna tipos de evaluación

### ReportesController
Análisis y reportes:
- `Index()` - Panel principal de reportes
- `GetPersonasEvaluar()` - API para cargar personas dinámicamente
- `GetCompetenciasPorTipo()` - API para cargar competencias por tipo

## 🎨 Diseño y Estilos

- **Framework CSS**: Tailwind CSS (CDN)
- **Paleta de colores**:
  - Azul: Acciones principales
  - Verde: Éxito
  - Rojo: Peligro
  - Naranja: Advertencias
- **Componentes**: Cards, Forms, Buttons, Alerts
- **Responsive**: Mobile, tablet, desktop

## 🔄 Flujo de Sesión

```
Login exitoso
    ↓
Session["UserId"] = usuario.Id
Session["UserName"] = usuario.Nombre
Session["UserType"] = usuario.TipoUsuario
    ↓
Acceso a areas protegidas
    ↓
Logout
    ↓
Session.Clear()
```

## 📊 Tipos de Preguntas Soportados

| Tipo | Descripción | Entrada |
|------|-------------|---------|
| TextoCorto | Respuesta breve | Input text |
| TextoLargo | Párrafo o texto extendido | Textarea |
| OpcionUnica | Una sola respuesta | Radio buttons |
| OpcionMultiple | Múltiples respuestas | Checkboxes |
| Escala | Puntuación 1-5 | Estrellas |

## 🔒 Seguridad

Consideraciones actuales y mejoras recomendadas:

### ✅ Implementado:
- Validación de sesión
- Anti-CSRF tokens
- Restricción de acceso a recursos

### 🔐 Recomendado para Producción:
- Hashing de contraseñas (bcrypt, PBKDF2)
- HTTPS obligatorio
- Autenticación basada en cookies ASP.NET Core
- Rate limiting
- Logging de accesos
- Validación de entrada más robusta

## 🚀 Próximos Pasos

### Fase 1: Encuestas Completas ✅ (En Progreso)
- [x] Crear modelo PersonaEvaluar
- [x] Implementar CRUD de personas a evaluar
- [x] Integración con DWH para búsqueda de empleados
- [x] Crear EncuestaDetailsViewModel con soporte bilingüe
- [x] Implementar switch de idioma en vista Details
- [x] Crear combobox dinámico para seleccionar persona a evaluar
- [ ] **Implementar Submit de respuestas** - Recopilar y guardar respuestas del formulario
- [ ] Crear tabla Respuestas en BD
- [ ] Persistencia de datos de respuestas
- [ ] Validación de respuestas requeridas

### Fase 2: Reportes y Análisis
- [ ] Dashboard de resultados
- [ ] Gráficos de respuestas por pregunta
- [ ] Análisis por persona evaluada
- [ ] Análisis por tipo de evaluación
- [ ] Exportar reportes a Excel/PDF
- [ ] Análisis comparativo entre periodos

### Fase 3: Mejoras de UX
- [ ] Notificaciones por email de invitación
- [ ] Recordatorios de encuestas pendientes
- [ ] Progreso visual de finalización
- [ ] Validación mejorada del lado del cliente
- [ ] Temas de color personalizables

### Fase 4: Administración
- [ ] Plantillas de encuestas reutilizables
- [ ] Gestión de periodos de evaluación
- [ ] Auditoría y logging de cambios
- [ ] Copiar encuestas existentes
- [ ] Versionado de encuestas

### Fase 5: Testing y DevOps
- [ ] Unit tests para controladores
- [ ] Tests de integración
- [ ] Tests E2E
- [ ] CI/CD pipeline
- [ ] Docker containerization
- [ ] Documentación API (Swagger)

### Fase 6: Escalabilidad
- [ ] Migración completa a Entity Framework Core
- [ ] Caché distribuida (Redis)
- [ ] Optimización de queries
- [ ] Índices en base de datos
- [ ] Load balancing

## 📝 Configuración (appsettings.json)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*",
  "SessionTimeout": 30
}
```

## � Características Implementadas

### Autenticación y Sesiones
- ✅ Login y Registro de usuarios
- ✅ Validación de credenciales
- ✅ Gestión de sesiones
- ✅ Cierre de sesión
- ✅ Diferentes tipos de usuario (Admin, Evaluador)

### Gestión de Encuestas
- ✅ Crear encuestas
- ✅ Publicar encuestas
- ✅ Responder encuestas con interfaz amigable
- ✅ Soporte para múltiples tipos de preguntas
- ✅ Escala de calificación 1-5

### Personas a Evaluar
- ✅ CRUD completo
- ✅ Búsqueda de empleados desde DWH (Tr3ss.Personal)
- ✅ Asignación a tipos de evaluación
- ✅ Soft-delete (marcar como inactivas)
- ✅ Filtrado por ciudad y tipo de evaluación

### Soporte Multiidioma
- ✅ Switch dinámico Español/Inglés en la vista
- ✅ Textos de indicaciones en ambos idiomas
- ✅ Preguntas en español e inglés
- ✅ Escala de calificaciones bilingüe
- ✅ Labels y placeholders que cambian con el idioma

### Cargas Dinámicas AJAX
- ✅ Búsqueda de empleados en tiempo real
- ✅ Carga de personas a evaluar según tipo
- ✅ Comboboxes que se rellenan automáticamente
- ✅ Peticiones GET seguras sin recargar página

### Diseño y UX
- ✅ Interfaz moderna con Tailwind CSS
- ✅ Responsive design (mobile, tablet, desktop)
- ✅ Cards con información estructurada
- ✅ Botones con estados visuales
- ✅ Validación visual de formularios

## 🔧 Base de Datos

### Stored Procedures Utilizados

**sp_traePersonasEvaluar** (SQL Server)
```sql
EXECUTE sp_traePersonasEvaluar @IdTipoEncuesta = 1
```
Retorna: `IdPersonal, NoEmp, Ciudad, Nombre, cDescripcion`

**sp_traePreguntasII** (SQL Server)
```sql
EXECUTE sp_traePreguntasII @IdTipoEvaluacion = '1'
```
Retorna: `IdPregunta, nOrden, cPregunta, cPregunta_Ingles, ...`

**sp_Inserta_PorIdPersonal** (SQL Server)
```sql
EXECUTE sp_Inserta_PorIdPersonal @IdTipoEncuesta = 1, @IdPersonal = 100
```

**sp_Elimina_PorIdPersonal** (SQL Server)
```sql
EXECUTE sp_Elimina_PorIdPersonal @idPersonal = 100
```
Realiza soft-delete (Activo = 0)

### Tablas de Consulta

- **Tr3ss.Personal** - DWH con datos maestros de empleados
  - Columnas: `IdPersonal, PersonalId, Nombre, Ciudad, Puesto, EmpleadoActivo`
  - Usada para búsqueda de empleados a evaluar

## �🐛 Solución de Problemas

### Error: "Submitted form is not multipart/form-data"
→ Verificar que el formulario use `enctype="multipart/form-data"` si sube archivos.

### Error: "Puerto 5001 ya en uso" / "Puerto 5032 ya en uso"
```bash
# Usar otro puerto
dotnet run --urls "https://localhost:5002"
```

### Error: "Archivos estáticos no se cargan"
→ Verificar que wwwroot exista y los archivos estén presentes.

### Error: "AJAX devuelve 404 en GetPersonasEvaluar"
→ Verificar que el endpoint sea `/Reportes/GetPersonasEvaluar` (no `/Encuesta/...`).

### Error: "Las personas no cargan en el combobox"
→ Abre la consola del navegador (F12) y verifica:
  - La solicitud AJAX se envía correctamente
  - El servidor retorna JSON válido
  - El idTipoEvaluacion no está vacío

### Error: "Placeholder no aparece en textarea"
→ Verificar que no haya espacios en blanco dentro del elemento `<textarea></textarea>`.

## 📞 Soporte y Contacto

Para preguntas o reportar problemas:
- Email: `desarrollo@evaluacion.com`
- Documentación: Ver [DOCUMENTACION.md](DOCUMENTACION.md)

## 📄 Licencia

Este proyecto está disponible bajo la licencia MIT.

## 👥 Autores

- Equipo de Desarrollo Administrativo
- Año: 2026

---

**¡Gracias por usar el Sistema de Evaluación de Liderazgo!** 🎉