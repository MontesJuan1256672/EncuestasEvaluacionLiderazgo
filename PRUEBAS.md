# 🧪 CHECKLIST DE PRUEBAS - EVALUACIÓN DE LIDERAZGO

## 🚀 Requisitos Previos
- [ ] .NET 6.0 o superior instalado
- [ ] Visual Studio Code o Visual Studio
- [ ] La aplicación ejecutándose (`dotnet run`)
- [ ] Acceso a https://localhost:5001

---

## 1️⃣ PRUEBAS DE AUTENTICACIÓN

### Login
- [ ] Acceder a https://localhost:5001/auth/login
- [ ] Página carga correctamente con formulario
- [ ] Campo email es de tipo email
- [ ] Campo contraseña es de tipo password
- [ ] Validación: Email vacío muestra error
- [ ] Validación: Contraseña vacía muestra error
- [ ] Credenciales incorrectas muestran error
- [ ] Login correcto (admin@test.com / Admin@123) redirige a Home
- [ ] Login correcto (evaluador@test.com / Eval@123) redirige a Home
- [ ] Usuario autenticado ver nombre en header
- [ ] Botón Cerrar Sesión aparece en header

### Registro
- [ ] Acceder a https://localhost:5001/auth/register
- [ ] Página carga correctamente
- [ ] Link "Regístrate aquí" en login funciona
- [ ] Campo nombre es requerido
- [ ] Campo email es de tipo email
- [ ] Campo contraseña es requerido
- [ ] Confirmar contraseña es requerido
- [ ] Passwords no coinciden muestra error
- [ ] Email vacío muestra error
- [ ] Registro exitoso muestra mensaje de éxito
- [ ] Redirige a login después de registro
- [ ] Nuevo usuario puede iniciar sesión

### Logout
- [ ] Usuario autenticado puede hacer logout
- [ ] Logout redirige a login
- [ ] Sesión se limpia (no acceso a áreas protegidas)
- [ ] Intentar acceder a /encuesta/index redirige a login

---

## 2️⃣ PRUEBAS DE ENCUESTAS (LISTADO)

### Página Index
- [ ] Acceder a /encuesta sin autenticarse redirige a login
- [ ] Usuario autenticado puede ver /encuesta/index
- [ ] Título "Mis Encuestas" aparece
- [ ] Botón "Nueva Encuesta" está visible
- [ ] Si hay encuestas, se muestran en cards
- [ ] Cada card muestra: Título, Descripción, Fechas, Estado
- [ ] Card muestra número de preguntas
- [ ] Card muestra número de respuestas
- [ ] Botones de acción (Ver, Editar, Eliminar) aparecen
- [ ] Filtros por estado funcionan (si están implementados)
- [ ] Si no hay encuestas, muestra mensaje vacío
- [ ] Mensaje vacío tiene botón para crear encuesta

---

## 3️⃣ PRUEBAS DE CREACIÓN DE ENCUESTA

### Formulario Create
- [ ] Navegar a /encuesta/create
- [ ] Página carga correctamente
- [ ] Campo Título es requerido
- [ ] Campo Descripción es requerido
- [ ] Campo Fecha Vencimiento es requerido
- [ ] No permite vencimiento en el pasado
- [ ] Validación: Título muy corto muestra error
- [ ] Validación: Descripción muy corta muestra error
- [ ] Botón "Crear Encuesta" funciona
- [ ] Encuesta se crea en estado Borrador
- [ ] Redirige a EditaEncuesta después de crear
- [ ] Botón Cancelar funciona correctamente

### Estados y Datos
- [ ] Encuesta nueva aparece en listado
- [ ] Estado es "Borrador"
- [ ] Creador es el usuario autenticado
- [ ] Fechas se guardan correctamente

---

## 4️⃣ PRUEBAS DE EDICIÓN DE ENCUESTA

### Página EditaEncuesta
- [ ] Click en botón "Editar" abre página de edición
- [ ] URL es /editaencuesta/{id}
- [ ] Encuesta sin permiso redirige a acceso denegado
- [ ] Campos están pre-cargados con datos
- [ ] Título en borrador es editable
- [ ] Descripción en borrador es editable
- [ ] Fecha en borrador es editable

### Tabs
- [ ] Tab "Detalles" activo por defecto
- [ ] Tab "Preguntas" muestra contador
- [ ] Tab "Respuestas" muestra contador
- [ ] Click en tabs cambia contenido
- [ ] Estilos de tabs cambian al seleccionar

### Tab Detalles
- [ ] Puede editar encuesta en Borrador
- [ ] Botón "Guardar Cambios" funciona
- [ ] En estado Publicada campos están deshabilitados
- [ ] Mensaje indicativo del estado aparece

### Tab Preguntas
- [ ] Si hay preguntas, se listan
- [ ] Muestra tipo de pregunta
- [ ] Muestra si es requerida
- [ ] Botón "Agregar Pregunta" está disponible
- [ ] Si no hay preguntas, mensaje vacío
- [ ] Si hay preguntas, botón de agregar también aparece

### Tab Respuestas
- [ ] Si hay respuestas, se muestran en tabla
- [ ] Tabla tiene: Participante, Fecha, Estado, Acciones
- [ ] Respuesta completada muestra "Completada"
- [ ] Respuesta incompleta muestra "En progreso"
- [ ] Si no hay respuestas, mensaje vacío

### Acciones
- [ ] Botón "Publicar Encuesta" aparece en Borrador
- [ ] Botón "Publicar" solo aparece en Borrador
- [ ] Publicar cambia estado a Publicada
- [ ] Botón "Eliminar" aparece si no hay respuestas
- [ ] Eliminar funciona y redirige a listado

---

## 5️⃣ PRUEBAS DE RESPONDER ENCUESTA

### Ver Encuesta
- [ ] Click en botón "Ver" abre página Details
- [ ] URL es /encuesta/details/{id}
- [ ] Título de encuesta se muestra
- [ ] Descripción se muestra
- [ ] Estado se muestra con icono
- [ ] Meta información: fechas, preguntas, respuestas
- [ ] Si estado es Publicada, formulario aparece
- [ ] Si estado no es Publicada, mensaje de no disponible

### Responder Encuesta (si está publicada)
- [ ] Preguntas aparecen numeradas
- [ ] Campo requerido marca en rojo (*)
- [ ] Tipo de pregunta es correcto

**Texto Corto:**
- [ ] Input text aparece
- [ ] Acepta entrada de texto

**Texto Largo:**
- [ ] Textarea aparece
- [ ] Acepta múltiples líneas

**Opción Única:**
- [ ] Radio buttons aparecen
- [ ] Solo se puede seleccionar uno
- [ ] Hover es interactivo

**Opción Múltiple:**
- [ ] Checkboxes aparecen
- [ ] Se pueden seleccionar varios
- [ ] Hover es interactivo

**Escala:**
- [ ] Estrellas aparecen (1-5)
- [ ] Click selecciona puntuación
- [ ] Hover muestra retroalimentación

### Envío
- [ ] Botón "Enviar Encuesta" está disponible
- [ ] Click envía respuestas
- [ ] Mensaje de éxito aparece
- [ ] Redirige a listado de encuestas

---

## 6️⃣ PRUEBAS DE INTERFAZ Y DISEÑO

### Layout
- [ ] Header se muestra para usuarios autenticados
- [ ] Logo y título aparecen
- [ ] Navegación tiene links
- [ ] Nombre de usuario aparece
- [ ] Botón Cerrar Sesión aparece
- [ ] Footer se muestra
- [ ] Footer es responsivo

### Colores y Estilos
- [ ] Botones primarios son azules
- [ ] Botones de éxito son verdes
- [ ] Botones de peligro son rojos
- [ ] Alertas tienen colores apropiados
- [ ] Cards tienen sombra y hover effect
- [ ] Inputs tienen focus state

### Responsividad
- [ ] Página se ve bien en desktop (1920px)
- [ ] Página se ve bien en tablet (768px)
- [ ] Página se ve bien en mobile (375px)
- [ ] Menú es responsivo
- [ ] Cards se adaptan a pantalla
- [ ] Tablas se adaptan (scroll en mobile)

---

## 7️⃣ PRUEBAS DE VALIDACIÓN

### Validaciones en Cliente
- [ ] Campos requeridos no permiten envío vacío
- [ ] Email valida formato
- [ ] Contraseña valida longitud mínima
- [ ] Confirmación de contraseña valida coincidencia
- [ ] Fecha no permite pasado

### Mensajes de Error
- [ ] Errores aparecen en rojo
- [ ] Errores son claros y específicos
- [ ] Errores desaparecen al corregir
- [ ] Validación general muestra lista de errores

---

## 8️⃣ PRUEBAS DE SEGURIDAD

### Autenticación
- [ ] Usuario no autenticado no accede a /encuesta
- [ ] Usuario no autenticado no accede a /editaencuesta
- [ ] URL /encuesta redirige a login sin auth
- [ ] Sesión expira correctamente

### Autorización
- [ ] Usuario A no puede editar encuesta de Usuario B
- [ ] Usuario A no puede eliminar encuesta de Usuario B
- [ ] Acceso denegado muestra página apropiada
- [ ] Usuario Evaluador no puede crear encuestas (verificar lógica)

### Datos
- [ ] Datos no se muestran sin autorización
- [ ] Respuestas están asociadas al usuario correcto
- [ ] No se pueden manipular IDs en URL para acceso no autorizado

---

## 9️⃣ PRUEBAS DE CASOS ESPECIALES

### Encuesta Completa
- [ ] Crear encuesta en Borrador
- [ ] Editar encuesta
- [ ] Publicar encuesta
- [ ] Responder encuesta como evaluador
- [ ] Ver respuestas como administrador
- [ ] Cerrar encuesta
- [ ] Encuesta cerrada no acepta más respuestas

### Múltiples Usuarios
- [ ] Admin crea encuesta
- [ ] Evaluador 1 responde
- [ ] Evaluador 2 responde
- [ ] Admin ve ambas respuestas
- [ ] Evaluadores solo ven sus respuestas

### Edge Cases
- [ ] Encuesta sin descripción mínima muestra error
- [ ] Título muy largo se maneja correctamente
- [ ] Email con caracteres especiales válidos
- [ ] Contraseña con caracteres especiales válida
- [ ] Descripción con saltos de línea se mantiene

---

## 🔟 PRUEBAS DE PERFORMANCE

- [ ] Página carga en menos de 2 segundos
- [ ] Listado de encuestas carga rápido
- [ ] Forma de respuesta es fluida
- [ ] Navegación es responsiva
- [ ] No hay retrasos en validación

---

## 1️⃣1️⃣ PRUEBAS DE COMPATIBILIDAD

### Navegadores
- [ ] Chrome (última versión)
- [ ] Firefox (última versión)
- [ ] Edge (última versión)
- [ ] Safari (si disponible)

### Dispositivos
- [ ] Desktop (1920x1080)
- [ ] Tablet (iPad, 768x1024)
- [ ] Mobile (iPhone, 375x667)

---

## 📊 MATRIZ DE PRUEBAS ADICIONALES

| Función | Usuario | Resultado Esperado | ✅/❌ |
|---------|---------|-------------------|------|
| Login | Admin | Acceso concedido | |
| Login | Evaluador | Acceso concedido | |
| Login | Inválido | Acceso denegado | |
| Crear | Admin | Encuesta creada | |
| Crear | Evaluador | Acceso denegado | |
| Editar | Propietario | Encuesta editada | |
| Editar | No propietario | Acceso denegado | |
| Responder | Publicada | Respuesta guardada | |
| Responder | Borrador | No disponible | |
| Ver | Propietario | Todo visible | |
| Ver | Evaluador | Solo publicadas | |

---

## 📝 NOTAS DE PRUEBAS

```
Fecha de prueba: ________________
Tester: ________________________
Navegador: ______________________
SO: ______________________________
Versión .NET: ____________________
Problemas encontrados: ___________
_________________________________
_________________________________
```

---

## ✅ CHECKLIST FINAL

- [ ] Todas las pruebas de autenticación pasaron
- [ ] Todas las pruebas de encuestas pasaron
- [ ] Todas las pruebas de edición pasaron
- [ ] Todas las pruebas de respuesta pasaron
- [ ] Diseño es responsivo en todos los dispositivos
- [ ] Validaciones funcionan correctamente
- [ ] Seguridad está implementada
- [ ] Performance es aceptable
- [ ] Sin errores en consola
- [ ] Sin warnings en aplicación
- [ ] Documentación está completa
- [ ] Código está bien estructurado
- [ ] Listo para siguiente fase

---

**🎉 Si todas las pruebas pasaron, la aplicación está lista!**
