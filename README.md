# Guía de Uso del Repositorio y Desarrollo

Este documento establece las directrices de trabajo, el flujo de control de versiones con Git, el estándar de arquitectura de software y el stack tecnológico utilizado en el proyecto.

---

## 1. Flujo de Trabajo con Git (Git Workflow)

Para mantener un historial ordenado, evitar conflictos y asegurar la calidad del código, se establece el siguiente flujo de trabajo:

### 1.1 Ramas de Integración
* **`ti2` / `taller-2`**: Rama base para todo el desarrollo correspondiente a Integra 2.
* **`ti4` / `taller-4`**: Rama base para todo el desarrollo correspondiente a Integra 4.
* **`main`**: Rama principal de producción / entregas finales estables.

> **Regla:** Está prohibido hacer commits directos a las ramas de integración (`ti2`, `ti4`) o a `main`.

### 1.2 Creación de Ramas por Feature
Cada nueva funcionalidad, módulo o corrección debe desarrollarse en su propia rama independiente:

1. **Actualizar la rama base correspondiente:**
   ```bash
   git checkout ti2   # o git checkout ti4 según corresponda
   git pull origin ti2
   ```

2. **Crear una nueva rama con prefijo `feat/`:**
   ```bash
   git checkout -b feat/nombre-de-la-funcionalidad
   ```
   *Ejemplos:* `feat/login-auth`, `feat/map-view`, `feat/crud-edificios`

### 1.3 Pull Requests (PR) e Integración
1. Al finalizar la funcionalidad en la rama `feat/nombre-de-la-funcionalidad`, subir los cambios al repositorio remoto:
   ```bash
   git push origin feat/nombre-de-la-funcionalidad
   ```
2. Crear un **Pull Request (PR)** en la plataforma (GitHub / GitLab) apuntando **exclusivamente a la rama de integración correspondiente (`ti2` o `ti4`)**.
3. El PR debe detallar los cambios realizados.
4. Todo PR debe ser revisado y aprobado por al menos un compañero de equipo antes de ser integrado (merge).

---

## 2. Arquitectura de Software: Arquitectura Limpia (Clean Architecture)

El desarrollo del proyecto se estructura bajo los principios de **Arquitectura Limpia (Clean Architecture)** con el objetivo de lograr desacoplamiento, mantenibilidad, testabilidad e independencia de frameworks y bases de datos.

### 2.1 Principios Clave
* **Regla de Dependencia:** Las dependencias del código solo pueden apuntar hacia adentro. Las capas internas no deben conocer nada de las capas externas.
* **Independencia de Frameworks:** La lógica del negocio no depende de la existencia de librerías o frameworks específicos.
* **Independencia de la UI y Base de Datos:** La interfaz de usuario y la persistencia de datos pueden ser modificadas sin alterar las reglas del negocio.

### 2.2 Capas de la Arquitectura
* **Capa de Dominio (Domain):** Contiene las entidades, modelos centrales y las interfaces/contratos del negocio. Es el núcleo puro del sistema y no tiene dependencias externas.
* **Capa de Aplicación / Casos de Uso (Application / Use Cases):** Define la lógica y flujos de negocio específicos de la aplicación. Orquesta el flujo de datos hacia y desde las entidades.
* **Capa de Infraestructura (Infrastructure):** Implementa el acceso a datos, servicios externos, repositorios y configuraciones de bases de datos.
* **Capa de Presentación / UI (Presentation):** Maneja la interacción con el usuario, controladores, componentes visuales, pantallas y gestión del estado de la interfaz.

---

## 3. Tecnologías del Proyecto

De acuerdo con la división de trabajo y roles definidos:

### 3.1 Integra 2 (Web & Backend)
* **Backend:** C# (.NET)
* **Frontend Web:** Diseño e implementación de Interfaz Web.
* **Mapeo Frontend:** Tecnologías de Frontend para visualización e interacción con mapas.
* **Persistencia:** Base de Datos (tecnología a definir).

### 3.2 Integra 4 (Móvil)
* **Desarrollo Móvil:** Flutter (Dart)
* **Frontend Móvil:** Diseño e implementación de Interfaz Móvil.
* **Mapeo Frontend:** Tecnologías de Frontend para visualización e interacción con mapas en plataforma móvil.

