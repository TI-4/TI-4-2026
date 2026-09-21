import 'package:flutter/foundation.dart';

import '../../domain/entities/auth_session.dart';
import '../../domain/entities/user.dart';
import '../../domain/entities/user_role.dart';

// Sesión en memoria. Storage seguro + JWT llegan en tarea 4.
class SessionController extends ChangeNotifier {
  AuthSession? _session;

  bool get isAuthenticated => _session != null && _session!.isValid;
  User? get currentUser => _session?.user;

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
      loginAt: DateTime.now(),
    );
    notifyListeners();
  }

  void signOut() {
    _session = null;
    notifyListeners();
  }
}
