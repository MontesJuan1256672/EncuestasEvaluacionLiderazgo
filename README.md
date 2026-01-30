# 📊 Evaluación de Liderazgo - Sistema MVC

Sistema web escalable para crear y gestionar encuestas de evaluación de competencias de liderazgo, desarrollado con **ASP.NET Core MVC** y **Tailwind CSS**.

## 🚀 Características

✅ **Arquitectura de 3 capas** - Modelos, Servicios, Controladores y Vistas  
✅ **Autenticación de usuarios** - Login/Register con diferentes tipos de usuario  
✅ **Gestión de encuestas** - Crear, editar, publicar y responder encuestas  
✅ **Múltiples tipos de preguntas** - Texto, opción única, múltiple, escala  
✅ **Diseño responsivo** - Tailwind CSS para interfaz moderna y limpia  
✅ **Código bien estructurado** - Interfaces, servicios, utilidades  
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
- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`

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
│   └── HomeController.cs           # Página de inicio
│
├── Models/
│   ├── Usuario.cs                 # Usuario del sistema
│   ├── Encuesta.cs                # Encuesta
│   ├── Pregunta.cs                # Pregunta de encuesta
│   ├── OpcionRespuesta.cs         # Opción de respuesta
│   └── Respuesta.cs               # Respuesta de usuario
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
│   └── SessionHelper.cs            # Utilidades de sesión
│
├── Views/
│   ├── Auth/
│   │   ├── Login.cshtml           # Página de login
│   │   └── Register.cshtml        # Página de registro
│   ├── Encuesta/
│   │   ├── Index.cshtml           # Listado de encuestas
│   │   ├── Details.cshtml         # Responder encuesta
│   │   └── Create.cshtml          # Crear encuesta
│   ├── EditaEncuesta/
│   │   └── Index.cshtml           # Editar encuesta
│   └── Shared/
│       ├── _Layout.cshtml         # Layout principal
│       └── _Layout.cshtml.css     # Estilos CSS
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
│
├── Program.cs                      # Configuración de la app
├── appsettings.json               # Configuración
└── README.md                       # Este archivo
```

## 🎯 Flujo de Uso

### Para Administradores:
1. Login con credenciales admin
2. Crear nueva encuesta
3. Agregar preguntas y opciones
4. Publicar encuesta
5. Ver respuestas y reportes

### Para Evaluadores:
1. Login con credenciales evaluador
2. Ver encuestas disponibles
3. Responder encuesta
4. Enviar respuestas

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

### Fase 1: Base de Datos
- [ ] Integrar SQL Server o PostgreSQL
- [ ] Implementar Entity Framework Core
- [ ] Crear migrations
- [ ] Seed de datos iniciales

### Fase 2: Funcionalidades
- [ ] Reportes y gráficos
- [ ] Exportar a Excel/PDF
- [ ] Envío de invitaciones por email
- [ ] Análisis avanzado de resultados
- [ ] Plantillas de encuestas

### Fase 3: Testing
- [ ] Unit tests
- [ ] Integration tests
- [ ] Tests E2E

### Fase 4: DevOps
- [ ] CI/CD pipeline
- [ ] Docker
- [ ] Publicación en producción

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

## 🐛 Solución de Problemas

### Error: "No se encuentra SqlServer..."
→ Datos en memoria están habilitados. No requiere BD.

### Error: "Puerto 5001 ya en uso"
```bash
# Usar otro puerto
dotnet run --urls "https://localhost:5002"
```

### Error: "Archivos estáticos no se cargan"
→ Verificar que wwwroot exista y los archivos estén presentes.

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
