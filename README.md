<<<<<<< HEAD
# Tecnologías — Módulo Mapa y Campus

## 1. Objetivo

Definir las tecnologías que utilizará el módulo **Mapa y Campus** de la aplicación móvil UCT Map, considerando la visualización del campus, edificios, salas, ubicaciones y marcadores, además de su futura integración con el backend.

---

## 2. Tecnologías principales

### Flutter

Framework utilizado para el desarrollo de la aplicación móvil de UCT Map.

Se utilizará para construir la interfaz, componentes y lógica del módulo de Mapa y Campus.

### Dart

Lenguaje de programación utilizado por Flutter.

Será utilizado para implementar los modelos de datos, componentes, servicios y lógica relacionada con el mapa.

---

## 3. Tecnología de mapas

### flutter_map

Librería de Flutter utilizada para implementar el mapa interactivo.

Permitirá:

* Visualizar el mapa.
* Realizar zoom y desplazamiento.
* Mostrar marcadores.
* Representar ubicaciones del campus.
* Personalizar elementos visuales del mapa.
* Integrar posteriormente información proveniente del Campus Service.

La utilización de `flutter_map` permite mantener la implementación dentro del ecosistema Flutter y facilita su integración con la aplicación móvil.

### OpenStreetMap

Fuente de datos cartográficos utilizada para representar el mapa base.

Se utilizará como base para la visualización geográfica del campus, sobre la cual se agregarán los edificios, ubicaciones y marcadores propios de UCT Map.

---

## 4. Datos del Campus

Los datos relacionados con el campus serán obtenidos posteriormente desde el **Campus Service** mediante el API Gateway definido por el proyecto.

Inicialmente, durante el desarrollo del módulo, se utilizarán **datos de prueba (mock)** para no depender de la disponibilidad del backend.

Los datos podrán incluir:

* Edificios.
* Salas.
* Ubicaciones.
* Coordenadas.
* Información descriptiva.
* Marcadores asociados a lugares específicos.

---

## 5. Comunicación con el Backend

La aplicación móvil no se comunicará directamente con los microservicios.

El flujo esperado será:

**Flutter → API Gateway → Campus Service → Base de datos**

De esta manera, la aplicación consumirá los endpoints expuestos por el Gateway y recibirá la información necesaria para representar el campus.

---

## 6. Modelos de datos

Se crearán modelos en Dart para representar la información recibida desde el Campus Service.

Ejemplo conceptual:

```text
Campus
 ├── Building
 │    ├── id
 │    ├── name
 │    ├── latitude
 │    ├── longitude
 │    └── description
 │
 └── Location
      ├── id
      ├── name
      ├── type
      ├── latitude
      └── longitude
```

La estructura definitiva dependerá del contrato de API definido por TI2.

---

## 7. Vista 360°

La visualización de fotografías 360° se considera una funcionalidad independiente del mapa principal.

La tecnología **Photo Sphere Viewer**, propuesta para el frontend web de TI2, está orientada a React/JavaScript, por lo que no se utilizará directamente en Flutter.

Para la aplicación móvil se evaluará posteriormente una alternativa compatible con Flutter o una integración mediante WebView, dependiendo de los requerimientos finales.

---

## 8. Tecnologías descartadas para el módulo móvil

Las siguientes tecnologías propuestas para el frontend web de TI2 no serán utilizadas directamente en Flutter:

* React + Vite
* TypeScript
* Tailwind CSS
* React Router DOM
* Zustand
* TanStack Query
* React Hook Form
* Zod
* Photo Sphere Viewer

Esto no significa que exista una incompatibilidad entre los proyectos. **TI2 y TI4 utilizarán diferentes tecnologías de frontend, pero compartirán el backend y las APIs del proyecto.**

---

## 9. Estado inicial del setup

La tarea `map-setup` debe finalizar con:

* [ ] Librería `flutter_map` configurada.
* [ ] Mapa base funcionando.
* [ ] OpenStreetMap configurado.
* [ ] Aplicación ejecutándose correctamente en el entorno objetivo.
* [ ] Prueba de zoom y desplazamiento.
* [ ] Prueba de un marcador.
* [ ] Estructura inicial preparada para incorporar edificios y ubicaciones.
* [ ] Documentación de las tecnologías utilizadas.

---

## 10. Arquitectura inicial

```text
                    UCT Map
                       │
                    Flutter
                       │
                  flutter_map
                       │
                OpenStreetMap
                       │
             ┌─────────┴─────────┐
             │                   │
         Edificios           Marcadores
             │                   │
          Salas             Ubicaciones
             │
             └───────┬───────────┘
                     │
                Campus Service
                     │
                API Gateway
                     │
              Backend / TI2
```

### Nota

La implementación inicial utilizará datos mock para permitir el desarrollo independiente del módulo móvil. La integración con el Campus Service se realizará una vez que el endpoint correspondiente esté disponible y su contrato haya sido definido.
=======
# TI-4-2026 poto
>>>>>>> 91d8ab4588d646cc9f7e6b738ec58ea49b33f1bd
