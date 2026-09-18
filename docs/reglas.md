# Reglas de trabajo — UCT Map móvil (TI-4)

Contexto para la IA y el equipo. Si una regla choca con otra, manda la más
nueva. Este archivo sí se versiona; el resto de `docs/` es local.

## Código y backend
1. El backend manda: donde difiera de lo nuestro, se sigue al backend al pie
   de la letra. Nada de tolerancias ni fallbacks que inventen datos.
2. Lo nuestro se conserva solo si mejora la seguridad (validación local,
   timeouts, no loguear secretos, mensajes sin detalles internos).
3. Sin endpoint `register`: la U es dueña de las cuentas (SSO futuro). Solo
   login contra Identity.
4. Comentarios mínimos y explicativos en cada archivo. Nada de referencias
   a tareas ni historial en los comentarios.

## Red y auth
5. Todo el tráfico móvil va al API Gateway (`http://localhost:5052` en dev,
   `http://10.0.2.2:5052` en emulador Android). El puerto directo de los
   servicios es solo para debug del backend.
6. Login real en standby hasta que backend confirme tabla `Users` (migración)
   y usuario semilla `@uct.cl`. Mientras tanto: contrato + fake.
7. Cierre de sesión local (borra token). Sin revoke en backend por ahora.
8. Roles pendientes de seed en backend; el JWT los trae (vacíos por ahora).

## Git y ramas
9. Una rama por feat desde `taller-4`: `UCT-<n>/ti4-<descripcion-corta>`.
10. No pushear a `taller-2` ni `taller-4`; solo con permiso y si es
    ultra necesario.
11. Nunca crear la PR sin el visto bueno explícito.

## Tests y docs locales
12. Los tests reales los hace el equipo T2 (backend). Los míos son solo
    locales para verificar y jamás se commitean (vía `.git/info/exclude`).
13. Los docs de contexto por feat son locales y no se suben, salvo este
    archivo. Al terminar cada tarea: qué se hizo, por qué, deuda y mejoras
    futuras.
