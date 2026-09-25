# UCT Map Frontend

## Componentes

| Componente | Descripción | Parámetros principales (Props) |
|---|---|---|
| `AppLayout` | Layout principal persistente (barra lateral y área de contenido). | N/A |
| `Avatar` | Muestra una imagen de perfil circular. | `src`, `alt`, `className` |
| `Button` | Botón interactivo principal (Soporta estado `disabled`). | `variant` (solid/outline/ghost), `color`, `size`, `width`, `disabled` |
| `CheckboxItem` | Elemento de selección (checkbox) con título y descripción. | `label`, `desc`, `icon` |
| `ExitButton` | Botón estándar de cierre (X). | `variant` (solid/ghost), `size` |
| `FileUpload` | Área interactiva para arrastrar y soltar archivos. | `label`, `maxFiles`, `onFilesChange` |
| `GlobalModals` | Contenedor maestro de modales flotantes globales gestionado por Zustand. | N/A |
| `IconText` | Bloque pequeño de texto acompañado de un ícono. | `icon`, `text`, `className` |
| `ImageControls` | Botones de navegación (anterior/siguiente) para galería. | `onPrev`, `onNext` |
| `ImageGallery` | Carrusel automático de imágenes. | `images`, `autoPlayInterval`, `width`, `height` |
| `ImagePagination` | Indicadores de posición (puntos) para la galería. | `total`, `currentIndex`, `onSelect` |
| `Input` | Campo estándar para ingreso de texto con etiqueta opcional superior. | `label`, `labelColor` (opcional), `error`, `multiline`, `icon`, *Props HTML* |
| `LoadingSpinner` | Componente visual animado para indicar carga. | `size` (sm/md/lg/xl), `color`, `text`, `className` |
| `ModalOverlay` | Envoltorio con animaciones y fondo oscuro para pop-ups. | `isOpen`, `onClose`, `children` |
| `NavButton` | Enlace de navegación para rutas del React Router. | `to`, `icon`, `label`, `size`, `width` |
| `NumberSelect` | Selector numérico con botones de incremento/decremento. | `initialValue`, `multiplier`, `isFloat`, `min`, `max`, `variant` |
| `Panel` | Contenedor reutilizable con estilos estandarizados de tarjeta. | `children`, `color`, `withUctBorder`, `onClose` |
| `PhotoFrame` | Contenedor con borde para mostrar imágenes o miniaturas. | `src`, `alt`, `className` |
| `SearchInput` | Campo de texto avanzado con menú desplegable de sugerencias. | `label`, `placeholder`, `value`, `onChange`, `icon`, `options` |
| `SectionButton` | Botón especializado para el menú de navegación lateral. | `to`, `icon`, `label`, `isExpanded`, `expandedWidth` |
| `Select` | Menú desplegable de selección de opciones con bordes UCT. | `label`, `labelColor`, `options`, `icon`, `error` |
| `SquareButton` | Botón interactivo cuadrado. | `children` |
| `Tag` | Etiqueta pequeña (pill) para categorías o estados. | `label`, `icon`, `color` |
| `TimedActionCard` | Tarjeta para acciones críticas con barra de progreso regresiva y bordes activos UCT. | `title`, `description`, `initialDurationMs`, `options` |
| `UserActionInfo` | Muestra detalles rápidos de un usuario o acción. | `title`, `subtitle`, `avatarSrc` |

## Módulos

Para mantener la arquitectura escalable, las piezas de interfaz más complejas (que agrupan múltiples componentes genéricos) se organizan en **módulos** dentro de la carpeta `src/modules`.

| Módulo | Componente | Descripción del componente | Parámetros |
|---|---|---|---|
| `admin` | `UserManagementCard` | Tarjeta para la gestión administrativa de usuarios, roles y baneos. | `user`, `roles`, `onRoleChange`, `onToggleBan` |
| `admin` | `MapEditorPanel` | Panel animado con herramientas para la edición interactiva del mapa. | N/A |
| `global` | `UserProfilePanel` | Panel lateral con información del usuario logueado y reportes activos. | `name`, `admissionYear`, `career`, `email`, `photoUrl`, `objectReports`, `incidentReports`, `onClose` |
| `map` | `BuildingDetailCard` | Tarjeta/Modal detallado de edificio con carrusel, horarios y salas. | `title`, `subtitle`, `images`, `schedule`, `floors`, `services`, `rooms`, `onClose`, `onNavigate`, `onView360`, `onReportProblem` |
| `map` | `HeatmapSpot` | Elemento visual superpuesto en el mapa que representa zonas térmicas. | `intensity` (low/medium/high), `size`, `className` |
| `map` | `MapFiltersPanel` | Panel interactivo para filtrar categorías y ubicaciones dentro del mapa. | `title`, `sections`, `onToggleItem`, `className` |
| `map` | `MapMarker` | Botón interactivo usado como pin en el mapa. | `icon`, `color` |
| `map` | `RoomInfoCard` | Tarjeta/Banner informativo de sala con título, tipo y capacidad. | `title`, `type`, `capacity`, `icon`, `className` |
| `map` | `ScheduleCard` | Banner/Tarjeta informativa de horario de atención. | `title`, `schedule`, `icon`, `color`, `className` |
| `contacts` | `ContactProfilePanel`, `ContactCard` | Perfiles de profesores, personal e información de contacto. | `title`, `contact`, `onClose`, `className` |
| `objects` | `PublishReportCard`, `ObjectReportCard`, `ReportLostObjectCard` | Modales/Cards para la gestión de objetos perdidos y encontrados. | Múltítulos props según tarjeta. |
| `incidents` | `IncidentCard` | Tarjeta para visualizar un reporte de incidente. | `location`, `status`, `photoUrl`, `title`, `reporterName`, `reportTime`, `reporterAvatar`, `description` |

## Vistas

Las vistas principales de la aplicación corresponden a las páginas renderizadas por cada ruta (`src/views`).

| Vista | Ruta | Descripción |
|---|---|---|
| `MapView` | `/map` | Vista interactiva del mapa del campus y filtros. |
| `IncidentsView` | `/reports` | Vista de reportes de incidentes con filtro por antigüedad y creación. |
| `ObjectsView` | `/objects` | Vista de gestión de objetos perdidos con filtros. |
| `ContactsView` | `/contacts` | Vista de contactos institucionales filtrables por rol. |
| `AdministrationView` | `/administration` | Panel protegido de administración general del sistema (solo Admin/Teacher). |
| `LoginView` | `/login` | Inicio de sesión |
| `ShowcaseView` | `/showcase` | Galería interactiva para pruebas y prototipado de componentes. |

## Interfaz Role-Based

Con la gestión de interfaces basadas en roles se permite añadir nuevas funcionalidades específicas de manera limpia:

1. **Para componentes de UI:**
   Se utiliza el componente `<RoleGuard>`. Si se desea que solo ciertos roles vean una funcionalidad, simplemente se envuelve el código:
   ```tsx
   <RoleGuard allowedRoles={['INCIDENTS_OFFICER', 'ADMIN']}>
     <IncidentManagerPanel />
   </RoleGuard>
   ```
   Si el usuario no tiene los permisos, React ignora el componente por completo.

2. **Para Vistas y Páginas Completas:**
   La navegación y enrutamiento se protegen con `<ProtectedRoute>` en el `AppRouter`:
   ```tsx
   <Route path="settings" element={
     <ProtectedRoute allowedRoles={['ADMIN', 'TEACHER']}>
       <SettingsView />
     </ProtectedRoute>
   } />
   ```
Si se desea llevar al usuario a una ruta específica si no cuenta con autorización, se puede modificar con `unauthorizedTo` (por default, lleva a `/map`):

```tsx
<Route element="{<ProtectedRoute" unauthorizedTo="/login"/>}>
  <Route element="{<ContactsView" path="/contacts"/>} />
</Route>
```
   El enrutador maneja la seguridad automáticamente e intercepta a los usuarios no autorizados.

3. **Para Lógica Interna:**
   Cualquier parte del código puede acceder instantáneamente a la sesión mediante el store global Zustand (`useAuthState`):
   ```tsx
   const user = useAuthState(state => state.user);
   if (user?.role === 'MEMBER') { /* lógica específica */ }
   ```

## Gestión de Modales Globales

Gracias a la arquitectura basada en estado global (Zustand) y React Portals integrados, crear y lanzar ventanas flotantes (Modales) que se sobreponen a toda la aplicación es sumamente sencillo y no requiere "ensuciar" el estado local de tus vistas.

Sigue estos 3 pasos para crear un panel flotante nuevo:

1. **Registra el nombre del Panel:**
   Añade un identificador único en `src/states/uiState.ts`:
   ```tsx
   type Panel = 'none' | 'saveMapChanges' | 'discardMapChanges' | 'miNuevoPanel';
   ```

2. **Dibuja el Modal:**
   Agrega tu diseño envuelto en `<ModalOverlay>` dentro del componente central `src/components/layout/GlobalModals.tsx`. Esto garantiza que la ventana tenga el fondo oscuro, las animaciones de entrada/salida y que flote por encima de la barra de navegación:
   ```tsx
   <ModalOverlay 
     isOpen={activePanel === 'miNuevoPanel'} 
     onClose={closePanel}
   >
     <div className="bg-white p-8 rounded-xl shadow-lg">
       <h2 className="text-2xl font-bold">¡Hola!</h2>
       <p>Soy un panel flotante.</p>
     </div>
   </ModalOverlay>
   ```

3. **Ejecuta (Trigger):**
   Desde cualquier botón o componente de la aplicación, sin importar cuán profundo esté, invoca el estado global para abrirlo:
   ```tsx
   import { useUiState } from '../states/uiState';

   <button onClick={() => useUiState.getState().openPanel('miNuevoPanel')}>
     Abrir Panel
   </button>
   ```
