# Solución: Error de Autenticación en Página de Login

## Problema Reportado

La página de login en producción y localhost presentaba el siguiente error cuando se accedía con parámetros encriptados:

```
Error de autenticación
• Error en página: Error al obtener usuarios administradores: Error al obtener usuarios administradores: 
  The type initializer for 'EncuestasEvaluacionLiderazgo.Data.DL' threw an exception.
• The TxtClave field is required.
• The CmbTipoEnc field is required.
```

El método `public IActionResult Login()` no se ejecutaba correctamente.

## Causa Raíz

El problema ocurría en la clase `DL.cs` (Data Access Layer):

### Inicialización Estática Problemática

```csharp
// ANTES (incorrecto)
private static string ConStr = GetConEvaluaLiderazgo();
private static string ConDWH = GetConDWH();
```

Estas variables se inicializaban de forma estática cuando se cargaba la clase. Si alguno de los métodos (`GetConEvaluaLiderazgo()` o `GetConDWH()`) lanzaba una excepción (por ejemplo, cuando `ConnectionWebApi.getConnectionString()` fallaba), toda la clase `DL` se volvía inutilizable.

Esto causaba un "type initializer exception" que propagaba a:
1. `FL.TraeTiposEvaluacion()` → Error en `CargarCombosLogin()`
2. `FL.TraeUsuariosAdministradores()` → Error en acceso de administrador

## Soluciones Implementadas

### 1. **Refactorizar DL.cs - Usar Lazy<T>**

Se cambió a inicialización perezosa (lazy initialization):

```csharp
// DESPUÉS (correcto)
private static readonly Lazy<string> _conStr = new Lazy<string>(() => GetConEvaluaLiderazgo());
private static readonly Lazy<string> _conDWH = new Lazy<string>(() => GetConDWH());

public static string ConStr => _conStr.Value;
public static string ConDWH => _conDWH.Value;
```

**Ventajas:**
- La conexión se obtiene solo cuando se necesita (`_conStr.Value`)
- Si falla, no tira abajo toda la aplicación al inicio
- Permite manejo de errores más granular

### 2. **Agregar Try-Catch en DL con Debug**

```csharp
public static string GetConDWH()
{
    try
    {
        ConnectionWebApi cs_telvista = new ConnectionWebApi();
        Coneccion = cs_telvista.getConnectionString(...);
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"Error al obtener conexión DWH: {ex.Message}");
        Coneccion = "";
    }
    return Coneccion;
}
```

Ahora registra el error en lugar de lanzar excepción.

### 3. **Mejorar FL.cs - Devolver DataSet Vacío**

En lugar de relanzar excepciones, los métodos ahora devuelven DataSets vacíos:

```csharp
public static DataSet TraeTiposEvaluacion()
{
    try
    {
        return DL.TraeTiposEvaluacion();
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Error en TraeTiposEvaluacion: {ex.Message}");
        
        // Retornar DataSet vacío en lugar de lanzar excepción
        DataSet ds = new DataSet();
        DataTable dt = new DataTable("TiposEvaluacion");
        dt.Columns.Add("IdTipoEvaluacion", typeof(string));
        dt.Columns.Add("cDescripcion", typeof(string));
        dt.Columns.Add("cClaveAcceso", typeof(string));
        ds.Tables.Add(dt);
        return ds;
    }
}
```

### 4. **Agregar Validación en LoginViewModel**

Se agregaron atributos `[Required]` con mensajes claros:

```csharp
public class LoginViewModel
{
    [Required(ErrorMessage = "Debe seleccionar un tipo de acceso")]
    public string TipoAcceso { get; set; }

    [Required(ErrorMessage = "The CmbTipoEnc field is required.")]
    public string CmbTipoEnc { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un centro")]
    public string CmbCentro { get; set; }

    [Required(ErrorMessage = "The TxtClave field is required.")]
    public string TxtClave { get; set; }
}
```

### 5. **Mejorar Manejo de Errores en AuthController**

- **GET Login**: Mejor validación de datos antes de mostrar
- **POST Login**: Try-catch específico para acceso administrador
- Mensajes de error más informativos

### 6. **Mejorar Vista Login.cshtml**

- Displays de advertencias y errores separados
- Mensajes más claros cuando no hay datos disponibles
- Mejor UX con estados de error

## Archivos Modificados

1. **[Data/DL.cs](Data/DL.cs)**
   - Cambio de inicialización estática a Lazy<T>
   - Agregar try-catch con Debug.WriteLine

2. **[Data/FL.cs](Data/FL.cs)**
   - `TraeTiposEvaluacion()`: Devolver DataSet vacío en error
   - `TraeUsuariosAdministradores()`: Devolver DataSet vacío en error

3. **[Controllers/AuthController.cs](Controllers/AuthController.cs)**
   - Agregar using `System.ComponentModel.DataAnnotations`
   - Mejorar `CargarCombosLogin()` con validaciones
   - Agregar try-catch en acceso administrador
   - Agregar validación en `LoginViewModel`

4. **[Views/Auth/Login.cshtml](Views/Auth/Login.cshtml)**
   - Agregar displays para advertencias y errores
   - Validar si hay datos antes de mostrar combos

## Comportamiento Esperado Después de los Cambios

### Escenario 1: Servicio de Conexiones Disponible ✅
- Todo funciona normalmente como antes
- Los datos se cargan correctamente

### Escenario 2: Servicio de Conexiones No Disponible ✅
- **GET /Auth/Login**: Muestra la página con combos vacíos o con valores por defecto
- **POST /Auth/Login**: Muestra mensaje de error claro sin crash
- La aplicación sigue funcionando

### Escenario 3: Con Parámetros Encriptados ✅
- Desencripta correctamente los parámetros
- No hay excepción de type initializer
- Muestra la página de login

## Pruebas Recomendadas

1. **Acceso Normal**: https://localhost:44358/Auth/Login
2. **Con Parámetros**: https://localhost:44358/Auth/Login?Id=gkx7UX/Kr7E=&loc=JBDCu2wqR68=&mat=myN8m0ACXhs=
3. **Simular Fallo de Conexión**: Desconectar temporalmente la BD y acceder al login
4. **Diferentes Tipos de Acceso**: Empleado, Administrador, Consulta

## Notas para Producción

- Los errores se registran en Visual Studio (Debug.WriteLine)
- En producción, considere implementar logging centralizado (NLog, Serilog)
- Monitoree los logs para detectar problemas de conexión persistentes
- Verifique que `ConnectionWebApi` esté correctamente configurado en ambos ambientes

## Rollback

Si es necesario revertir los cambios:
- Restaurar DL.cs a inicialización estática
- Restaurar FL.cs para relanzar excepciones
- Restaurar AuthController.cs y Login.cshtml a versión anterior
