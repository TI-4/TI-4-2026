# Microservicio Campus

## Historial de Cambios (Changelog)

### UCT-99: Población de Datos Reales (Seeders)
Se implementó un Seeder utilizando Entity Framework Core para poblar automáticamente la base de datos de Campus con datos reales de la Universidad Católica de Temuco.

- **Componentes modificados:**
  - `CampusDbContextSeed.cs`: Se ajustó esta clase para insertar la estructura real de la universidad en la base de datos.
  - `Program.cs`: Se integró la llamada a `CampusDbContextSeed.SeedAsync()` durante el arranque de la API (posterior a las migraciones).
- **Datos iniciales incluidos:**
  - **Campus**: San Francisco y San Juan Pablo II con sus coordenadas aproximadas.
  - **Edificios**: Edificios 16 y 12 en Campus San Francisco; Edificios 8 y 7 en Campus San Juan Pablo II (todos de 3 pisos).
  - **Categorías**: Sala de Clases, Laboratorio, Sala de Estudio.
  - **Salas**: 2 salas por piso (del 1 al 3) y 1 laboratorio en el primer piso, aplicados a cada edificio.
  - **Estructuras**: Se añadieron "Punto de Estudio San Francisco" y "Centro de Estudio Juan Pablo II" para validar el uso de la entidad `Structure`.

*Nota: Con esto, al desplegar la API y aplicar migraciones, la base de datos de PostgreSQL estará lista para poder probar la visualización de datos en el mapa.*
