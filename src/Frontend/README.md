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
| `Input` | Campo estándar para ingreso de texto. | *Props estándar de HTMLInputElement* | - |
| `MapMarker` | Botón interactivo usado como pin en el mapa. | `icon`, `color` | - |
| `NavButton` | Enlace de navegación para rutas del React Router. | `to`, `icon`, `label`, `size`, `width` | - |
| `Panel` | Contenedor reutilizable con estilos estandarizados de tarjeta. | `children`, `color`, `withUctBorder`, `onClose` | - |
| `PhotoFrame` | Contenedor con borde para mostrar imágenes o miniaturas. | `src`, `alt`, `className` | `useState` |
| `SectionButton` | Botón especializado para el menú de navegación lateral. | `to`, `icon`, `label`, `isExpanded`, `expandedWidth` | - |
| `Select` | Menú desplegable de selección de opciones. | `label`, `options` (value, label) | `useState`, `useRef`, `useEffect` |
| `SquareButton` | Botón interactivo cuadrado. | `children` | - |
| `Tag` | Etiqueta pequeña (pill) para categorías o estados. | `label`, `icon`, `color` | - |
| `UserActionInfo` | Muestra detalles rápidos de un usuario o acción. | `title`, `subtitle`, `avatarSrc` | - |
