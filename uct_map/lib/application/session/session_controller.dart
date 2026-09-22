import 'package:flutter/foundation.dart';

import '../../domain/entities/auth_session.dart';
import '../../domain/entities/user.dart';
import '../../domain/entities/user_role.dart';

// Controlador reactivo de sesión de usuario y token JWT.
class SessionController extends ChangeNotifier {
  AuthSession? _session;

  bool get isAuthenticated => _session != null && _session!.isValid;
  User? get currentUser => _session?.user;
  String? get token => _session?.token;

  /// Establece una sesión autenticada con credenciales y token emitido por Identity Service.
  void signIn({
    required String userId,
    required String email,
    required String token,
    String? name,
    UserRole? role,
    DateTime? expiresAt,
  }) {
    final cleanEmail = email.trim();
    _session = AuthSession(
      user: User(
        id: userId,
        nombre: name != null && name.isNotEmpty ? name : cleanEmail.split('@').first,
        correo: cleanEmail,
        idRol: '',
        rol: role ?? UserRole.desconocido,
      ),
      token: token,
      expiresAt: expiresAt,
      loginAt: DateTime.now(),
    );
    notifyListeners();
  }

  /// Sesión rápida para desarrollo o pruebas locales sin token del Gateway.
  void signInDemo(String email) {
    final clean = email.trim();
    _session = AuthSession(
      user: User(
        id: 'local',
        nombre: clean.split('@').first,
        correo: clean,
        idRol: '',
        rol: UserRole.desconocido,
      ),
      token: 'demo-local-token',
      loginAt: DateTime.now(),
    );
    notifyListeners();
  }

  /// Cierra la sesión activa y notifica a los listeners.
  void signOut() {
    _session = null;
    notifyListeners();
  }
}
