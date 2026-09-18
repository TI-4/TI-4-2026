// Contrato de autenticación. Refleja al backend (IdentityService) al pie de
// la letra, sin tolerancias: lo que el backend no entrega no se inventa.
//
// POST /api/identity/login {Email, Password}
//   200 -> {UserId, Email, Token}
//   400 -> faltan datos | 401 -> credenciales inválidas
class AuthLoginResult {
  const AuthLoginResult({
    required this.userId,
    required this.email,
    required this.token,
  });

  final String userId;
  final String email;
  final String token;
}

/// Error de login con mensaje ya en español para mostrar en UI.
/// No expone cuerpos crudos del servidor.
class AuthFailure implements Exception {
  const AuthFailure(this.message);

  final String message;

  @override
  String toString() => 'AuthFailure: $message';
}

abstract class AuthRepository {
  Future<AuthLoginResult> login({
    required String email,
    required String password,
  });
}
