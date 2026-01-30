# 📊 Sistema de Evaluación de Liderazgo - Documentación

## Descripción General

Sistema MVC escalable para crear y gestionar encuestas de evaluación de liderazgo con arquitectura de tres capas.

## Arquitectura del Proyecto

### 1. **Capa de Presentación (Views)**
- `Views/Auth/` - Vistas de autenticación (Login, Register)
- `Views/Encuesta/` - Vistas para responder encuestas
- `Views/EditaEncuesta/` - Vistas para editar y gestionar encuestas
- `Views/Shared/` - Vistas compartidas y layout

### 2. **Capa de Controladores (Controllers)**
- `AuthController` - Gestiona login y registro
- `EncuestaController` - Maneja las encuestas (crear, ver, responder)
- `EditaEncuestaController` - Edita y administra encuestas
- `HomeController` - Página de inicio

### 3. **Capa de Servicios (Services)**
- `IAuthService / AuthService` - Autenticación y gestión de usuarios
- `IEncuestaService / EncuestaService` - CRUD de encuestas
- `IRespuestaService / RespuestaService` - Gestión de respuestas

### 4. **Capa de Modelos (Models)**
- `Usuario` - Modelo de usuario con tipos (Admin, Evaluador)
- `Encuesta` - Modelo de encuesta
- `Pregunta` - Modelo de pregunta
- `OpcionRespuesta` - Opciones de respuesta
- `Respuesta / RespuestaDetalle` - Respuestas de usuarios

### 5. **Capa de Utilidades (Utilities)**
- `SessionHelper` - Ayudantes para manejo de sesiones
- Validadores y extensiones

## Rutas Principales

| Ruta | Controlador | Método | Descripción |
|------|-------------|--------|-------------|
| `/Auth/Login` | AuthController | GET/POST | Login de usuario |
| `/Auth/Register` | AuthController | GET/POST | Registro de nuevo usuario |
| `/Auth/Logout` | AuthController | GET | Cierre de sesión |
| `/Encuesta` | EncuestaController | GET | Listar encuestas |
| `/Encuesta/Create` | EncuestaController | GET/POST | Crear encuesta |
| `/Encuesta/Details/{id}` | EncuestaController | GET | Ver y responder encuesta |
| `/EditaEncuesta/{id}` | EditaEncuestaController | GET | Editar encuesta |
| `/EditaEncuesta/Update` | EditaEncuestaController | POST | Guardar cambios |
| `/EditaEncuesta/Publish` | EditaEncuestaController | POST | Publicar encuesta |

## Tipos de Usuario

### Administrador (Tipo = 1)
- Crear, editar y eliminar encuestas
- Publicar encuestas
- Ver reportes de respuestas
- Gestionar otros usuarios

### Evaluador (Tipo = 2)
- Ver encuestas publicadas
- Responder encuestas
- Ver historial de respuestas

## Tipos de Preguntas

1. **TextoCorto** - Respuesta de texto corto
2. **TextoLargo** - Párrafo o texto extenso
3. **OpcionUnica** - Una sola opción (radio buttons)
4. **OpcionMultiple** - Múltiples opciones (checkboxes)
5. **Escala** - Puntuación de 1 a 5 estrellas

## Estados de Encuesta

- **Borrador** - En edición, no visible para otros
- **Publicada** - Disponible para responder
- **Cerrada** - No acepta más respuestas
- **Archivada** - Histórica, no visible

## Estilos y Diseño

- **Framework CSS**: Tailwind CSS (vía CDN)
- **Colores principales**:
  - Azul: `#3B82F6` - Acciones principales
  - Verde: `#10B981` - Éxito
  - Rojo: `#EF4444` - Peligro
  - Naranja: `#F97316` - Advertencias

## Variables de Sesión

```csharp
HttpContext.Session.GetInt32("UserId")      // ID del usuario
HttpContext.Session.GetString("UserName")   // Nombre del usuario
HttpContext.Session.GetInt32("UserType")    // Tipo de usuario (1 o 2)
```

## Estructura de Base de Datos (Cuando se implemente)

```sql
-- Usuarios
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY,
    Nombre NVARCHAR(100),
    Email NVARCHAR(100) UNIQUE,
    Contraseña NVARCHAR(255),
    TipoUsuario INT,
    Activo BIT
);

-- Encuestas
CREATE TABLE Encuestas (
    Id INT PRIMARY KEY,
    Titulo NVARCHAR(200),
    Descripcion NVARCHAR(MAX),
    FechaCreacion DATETIME,
    FechaVencimiento DATETIME,
    Estado INT,
    UsuarioCreadorId INT FOREIGN KEY,
    Activa BIT
);

-- Preguntas
CREATE TABLE Preguntas (
    Id INT PRIMARY KEY,
    EncuestaId INT FOREIGN KEY,
    Texto NVARCHAR(MAX),
    Tipo INT,
    Orden INT,
    Requerida BIT
);

-- OpcionesRespuesta
CREATE TABLE OpcionesRespuesta (
    Id INT PRIMARY KEY,
    PreguntaId INT FOREIGN KEY,
    Texto NVARCHAR(200),
    Valor INT,
    Orden INT
);

-- Respuestas
CREATE TABLE Respuestas (
    Id INT PRIMARY KEY,
    EncuestaId INT FOREIGN KEY,
    UsuarioId INT FOREIGN KEY,
    FechaRespuesta DATETIME,
    Completada BIT
);

-- DetallesRespuesta
CREATE TABLE DetallesRespuesta (
    Id INT PRIMARY KEY,
    RespuestaId INT FOREIGN KEY,
    PreguntaId INT FOREIGN KEY,
    Valor NVARCHAR(MAX),
    FechaRespuesta DATETIME
);
```

## Próximos Pasos para Implementación

1. **Integrar Base de Datos**
   - Instalar Entity Framework Core
   - Crear DbContext
   - Migrations

2. **Mejoras de Seguridad**
   - Hashing de contraseñas (bcrypt)
   - Autenticación mediante cookies ASP.NET Core
   - CSRF protection
   - SQL Injection prevention

3. **Funcionalidades Adicionales**
   - Reportes y gráficos
   - Exportar a Excel
   - Plantillas de encuestas
   - Envío de invitaciones por email
   - Análisis de resultados

4. **Testing**
   - Unit Tests para servicios
   - Integration Tests
   - Tests de UI

5. **DevOps**
   - CI/CD pipeline
   - Docker containerization
   - Deployment scripts

## Usuarios de Prueba

| Email | Contraseña | Tipo |
|-------|-----------|------|
| admin@test.com | Admin@123 | Administrador |
| evaluador@test.com | Eval@123 | Evaluador |

## Notas de Desarrollo

- Actualmente usa datos en memoria (List<T>)
- Para producción, reemplazar con Entity Framework Core
- Las contraseñas están en texto plano (aplicar hashing)
- Implementar validaciones más robustas
- Agregar logging
- Implementar manejo de errores global

## Convenciones de Código

- Nombres en PascalCase para clases
- Nombres en camelCase para variables locales
- Interfases con prefijo `I`
- Métodos async con sufijo `Async`
- Comentarios XML para métodos públicos
- Métodos privados con comentarios si es necesario
