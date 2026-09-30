import '../../application/session/session_controller.dart';
import 'authenticated_client.dart';

/// Proveedor para inicializar el cliente HTTP autenticado conectado
/// al controlador de sesión y al ciclo de vida del token JWT.
class ApiClientProvider {
  const ApiClientProvider._();

  /// Crea un [AuthenticatedClient] configurado para inyectar automáticamente
  /// el token actual de [session] y revocar la sesión ante respuestas 401.
  static AuthenticatedClient create(SessionController session) {
    return AuthenticatedClient(
      tokenProvider: () => session.token,
      onUnauthorized: () => session.signOut(),
    );
  }
}
