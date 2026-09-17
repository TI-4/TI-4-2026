# UCT Map Frontend

## UI Components

| Componente | Descripción | Parámetros principales (Props) | Hooks utilizados |
|---|---|---|---|
| `AppLayout` | Layout principal persistente (barra lateral y área de contenido). | N/A | `useState`, `useLocation` |
| `Avatar` | Muestra una imagen de perfil circular. | `src`, `alt`, `className` | `useState` |
| `Button` | Botón interactivo principal. | `variant` (solid/outline/ghost), `color`, `size`, `width` | - |
| `CheckboxItem` | Elemento de selección (checkbox) con título y descripción. | `label`, `desc`, `icon` | - |
| `ExitButton` | Botón estándar de cierre (X). | `variant` (solid/ghost), `size` | - |
| `FileUpload` | Área interactiva para arrastrar y soltar archivos. | `label`, `maxFiles`, `onFilesChange` | `useFileUpload`, `useImageControls`, `useEffect` |
| `IconText` | Bloque pequeño de texto acompañado de un ícono. | `icon`, `text`, `className` | - |
| `ImageControls` | Botones de navegación (anterior/siguiente) para galería. | `onPrev`, `onNext` | - |
| `ImageGallery` | Carrusel automático de imágenes. | `images`, `autoPlayInterval`, `width`, `height` | `useImageControls` |
| `ImagePagination` | Indicadores de posición (puntos) para la galería. | `total`, `currentIndex`, `onSelect` | - |
| `Input` | Campo estándar para ingreso de texto con etiqueta opcional superior. | `label`, `labelColor` (opcional, blanco por defecto), `error`, `multiline`, `icon`, *Props HTML* | - |
| `LoadingSpinner` | Componente visual animado para indicar que un proceso o dato se está cargando. Puede usarse como placeholder. | `size` (sm/md/lg/xl), `color`, `text`, `className` | - |
| `MapMarker` | Botón interactivo usado como pin en el mapa. | `icon`, `color` | - |
| `NavButton` | Enlace de navegación para rutas del React Router. | `to`, `icon`, `label`, `size`, `width` | - |
| `Panel` | Contenedor reutilizable con estilos estandarizados de tarjeta. | `children`, `color`, `withUctBorder`, `onClose` | - |
| `PhotoFrame` | Contenedor con borde para mostrar imágenes o miniaturas. | `src`, `alt`, `className` | `useState` |
| `RoomInfoCard` | Tarjeta/Banner informativo de sala con título, tipo de sala y capacidad aproximada de estudiantes. | `title`, `type`, `capacity`, `icon`, `className` | - |
| `ScheduleCard` | Banner/Tarjeta informativa de horario de atención con icono, título e intervalo de horas. | `title`, `schedule`, `icon`, `color`, `className` | - |
| `SectionButton` | Botón especializado para el menú de navegación lateral. | `to`, `icon`, `label`, `isExpanded`, `expandedWidth` | - |
| `Select` | Menú desplegable de selección de opciones. | `label`, `labelColor` (opcional, blanco por defecto), `options` (value, label), `icon`, `error` | `useState`, `useRef`, `useEffect` |
| `SquareButton` | Botón interactivo cuadrado. | `children` | - |
| `Tag` | Etiqueta pequeña (pill) para categorías o estados. | `label`, `icon`, `color` | - |
| `UserActionInfo` | Muestra detalles rápidos de un usuario o acción. | `title`, `subtitle`, `avatarSrc` | - |

## Módulos

Para mantener la arquitectura escalable, las piezas de interfaz más complejas (que agrupan múltiples componentes genéricos) se organizan en **módulos** dentro de la carpeta `src/modules`.

| Módulo | Componente | Descripción del componente | Parámetros |
|---|---|---|---|
| `map` | `MapFiltersPanel` | Panel interactivo para filtrar categorías y ubicaciones dentro del mapa. | `title`, `sections`, `onToggleItem`, `className` |
| `contacts` | `ContactProfilePanel`, `ContactCard` | Perfiles de profesores, personal e información de contacto. | `title`, `contact`, `onClose`, `className` |
| `map` | `BuildingDetailCard` | Tarjeta/Modal detallado de edificio con carrusel de imágenes, horario de atención, cantidad de pisos, servicios, listado de salas (`RoomInfoCard`), reporte de problemas y botones de llegada y vista 360°. | `title`, `subtitle`, `images`, `schedule`, `floors`, `services`, `rooms`, `onClose`, `onNavigate`, `onView360`, `onReportProblem` |
| `objects` | `PublishReportCard`, `ObjectReportCard` | Card/Modal interactivo para publicar nuevos reportes de objetos perdidos con título, descripción, subida de imágenes, selección de ubicación y botones de acción. Reportes existentes usan `ObjectReportCard`. | `title`, `status`, `photoUrl`, `building`, `code`, `actions` |
| `incidents` | `IncidentCard` | Tarjeta para visualizar un reporte de incidente con ubicación, estado, fotografía, título, autor del reporte y descripción detallada. | `location`, `status`, `photoUrl`, `title`, `reporterName`, `reportTime`, `reporterAvatar`, `description` |

## Vistas (Views)

Las vistas principales de la aplicación corresponden a las páginas renderizadas por cada ruta (`src/views`).

| Vista | Ruta | Descripción |
|---|---|---|
| `MapView` | `/map` | Vista interactiva del mapa del campus y filtros. |
| `IncidentsView` | `/reports` | Vista de reportes de incidentes con filtro por antigüedad, campus y creación de reportes. |
| `ObjectsView` | `/objects` | Vista de búsqueda y gestión de objetos perdidos con filtros por campus, antigüedad y categorías de objetos. |
| `ContactsView` | `/contacts` | Vista de búsqueda y lista de contactos institucionales filtrables por rol. |
| `ShowcaseView` | `/showcase` | Galería interactiva para pruebas y prototipado de la biblioteca de componentes. |

## Interfaces Globales

Las interfaces y tipos compartidos en el proyecto están ubicados de manera centralizada en la carpeta `src/interfaces` para evitar redundancias y facilitar la organización del código.

| Archivo | Contenido / Uso |
|---|---|
| `FilterItem.ts` | Define la estructura individual de cada opción de filtrado (id, label, icon, checked). |
| `FilterSection.ts` | Define la estructura de cada agrupación de filtros (id, title, items). |

