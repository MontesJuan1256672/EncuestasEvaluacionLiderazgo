# 🚀 GUÍA RÁPIDA DE EJECUCIÓN

## Requisitos
- .NET 6.0 o superior
- Visual Studio Code o Visual Studio
- Navegador moderno

## Pasos para Ejecutar

### 1. Abrir Terminal
```powershell
# Navegar a la carpeta del proyecto
cd c:\DesarrolloAdministrativo\AplicacionesWeb\EncuestasEvaluacionLiderazgo
```

### 2. Restaurar Dependencias
```powershell
dotnet restore
```

### 3. Ejecutar la Aplicación
```powershell
dotnet run
```

La aplicación se iniciará en:
- **HTTPS**: https://localhost:5001
- **HTTP**: http://localhost:5000

## 🔐 Credenciales de Prueba

### Cuenta Administrador
```
Email: admin@test.com
Contraseña: Admin@123
Permisos: Crear, editar, publicar encuestas
```

### Cuenta Evaluador
```
Email: evaluador@test.com
Contraseña: Eval@123
Permisos: Ver y responder encuestas publicadas
```

## 📋 Pruebas Rápidas

### 1. Prueba de Login (Admin)
1. Ir a https://localhost:5001/auth/login
2. Ingresar: admin@test.com / Admin@123
3. Debería redirigir a Home con sesión activa
4. En el header debe aparecer "admin"

### 2. Prueba de Crear Encuesta
1. Estando como Admin, ir a /encuesta/index
2. Click en "Nueva Encuesta"
3. Llenar formulario:
   - Título: "Evaluación Prueba"
   - Descripción: "Esta es una encuesta de prueba"
   - Fecha: Seleccionar fecha futura
4. Click "Crear Encuesta"
5. Debería redirigir a EditaEncuesta

### 3. Prueba de Publicar Encuesta
1. En EditaEncuesta, click en "Publicar Encuesta"
2. Debería aparecer error (sin preguntas)
3. Estado cambiaría a "Publicada" si tuviera preguntas

### 4. Prueba de Logout
1. Click en botón "Cerrar Sesión"
2. Debería redirigir a login
3. Intentar acceder a /encuesta/index redirige a login

### 5. Prueba de Registro
1. Ir a /auth/login
2. Click en "Regístrate aquí"
3. Llenar formulario con nuevos datos
4. Click "Crear Cuenta"
5. Debería mostrar éxito y redirigir a login
6. Intentar login con nuevas credenciales

## 🔧 Solución de Problemas

### Error: "Port 5001 already in use"
```powershell
# Usar puerto diferente
dotnet run --urls "https://localhost:5002"
```

### Error: "No se encuentran los archivos estáticos"
```powershell
# Verificar que exista la carpeta wwwroot
# Si no existe, crearla:
mkdir wwwroot
```

### Aplicación no carga el CSS de Tailwind
✅ Está usando CDN, debería funcionar directamente
- Si no funciona, verificar conexión a internet
- Tailwind CSS se carga desde cdn.tailwindcss.com

### Error de Sesión
- Limpiar cookies del navegador
- Usar navegación en incógnito
- Reiniciar la aplicación

## 📊 Estructura de Prueba Completa

```
ESCENARIO: Usuario prueba la aplicación completa

1. LOGIN
   └─ admin@test.com / Admin@123 ✓

2. VER ENCUESTAS
   └─ /encuesta/index ✓

3. CREAR ENCUESTA
   ├─ Llenar formulario ✓
   └─ Guardar ✓

4. EDITAR ENCUESTA
   ├─ Ver detalles ✓
   ├─ Ver preguntas (vacío) ✓
   └─ Ver respuestas (vacío) ✓

5. INTENTAR PUBLICAR
   └─ Error: Sin preguntas ✓

6. LOGOUT
   └─ /auth/logout ✓

7. LOGIN COMO EVALUADOR
   └─ evaluador@test.com / Eval@123 ✓

8. VER ENCUESTAS
   └─ Listar todas (no solo propias) ✓

9. RESPONDER ENCUESTA
   └─ Si alguna está publicada ✓

10. LOGOUT
    └─ Fin de prueba ✓
```

## 🧪 Checklist de Funcionalidad

- [ ] Login funciona para admin
- [ ] Login funciona para evaluador
- [ ] Logout funciona
- [ ] Crear encuesta funciona
- [ ] Listar encuestas funciona
- [ ] Página responsiva en mobile
- [ ] Página responsiva en desktop
- [ ] Botones y links funcionan
- [ ] Validaciones muestran errores
- [ ] Mensajes de éxito aparecen
- [ ] Sesión se mantiene entre páginas
- [ ] Acceso restringido sin autenticación

## 📱 Prueba de Responsividad

### Desktop (1920x1080)
Abrir DevTools (F12) → Desktop
- Layout se ve correcto ✓
- Todos los elementos visibles ✓

### Tablet (768x1024)
Abrir DevTools → iPad
- Layout se ajusta ✓
- Menú funciona ✓
- Cards se reorganizan ✓

### Mobile (375x667)
Abrir DevTools → iPhone
- Layout se ajusta ✓
- Texto es legible ✓
- Botones son clickeables ✓

## 🎯 Verificación Visual

### Colores
- [ ] Botones azules claros (#3B82F6)
- [ ] Éxitos en verde (#10B981)
- [ ] Peligros en rojo (#EF4444)
- [ ] Alertas en naranja (#F97316)

### Fuentes
- [ ] Títulos grandes y claros
- [ ] Texto base legible
- [ ] Contraste adecuado

### Espaciado
- [ ] Padding/margin consistente
- [ ] Elementos no se solapan
- [ ] Alineación es uniforme

## 📝 Notas Importantes

1. **Datos en Memoria**
   - Los datos se pierden al reiniciar
   - Para persistencia, implementar BD

2. **Seguridad**
   - Contraseñas en texto plano (solo demo)
   - Aplicar bcrypt en producción

3. **Rendimiento**
   - Óptimo para hasta 100 usuarios
   - Optimizar con BD para más

4. **Navegadores Soportados**
   - Chrome 90+
   - Firefox 88+
   - Edge 90+
   - Safari 14+

## 🎓 Próximos Pasos Después de Ejecutar

1. Leer [README.md](README.md)
2. Revisar [DOCUMENTACION.md](DOCUMENTACION.md)
3. Estudiar [BUENAS_PRACTICAS.md](BUENAS_PRACTICAS.md)
4. Revisar [EJEMPLOS_SERVICIOS.cs](EJEMPLOS_SERVICIOS.cs)
5. Ejecutar [PRUEBAS.md](PRUEBAS.md) checklist completo

## 🆘 Ayuda

Si algo no funciona:

1. Verificar que .NET está instalado: `dotnet --version`
2. Verificar que el puerto 5001 está libre
3. Limpiar y restaurar: `dotnet clean && dotnet restore`
4. Revisar errores en la consola
5. Consultar documentación en los archivos .md

---

**¡La aplicación está lista para usar!** 🎉
