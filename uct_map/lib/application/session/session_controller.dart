import 'package:flutter/foundation.dart';

import '../../domain/entities/auth_session.dart';
import '../../domain/entities/user.dart';

// Sesión en memoria con forma del backend.
class SessionController extends ChangeNotifier {
  AuthSession? _session;

  bool get isAuthenticated => _session != null && _session!.isValid;
  User? get currentUser => _session?.user;
  String? get token => _session?.token;

  void signIn({
    required String userId,
    required String email,
    required String token,
  }) {
    _session = AuthSession(
      user: User.fromLogin(userId: userId, email: email.trim()),
      loginAt: DateTime.now(),
      token: token,
    );
    notifyListeners();
  }

  void signInDemo(String email) {
    _session = AuthSession(
      user: User.fromLogin(userId: 'local', email: email.trim()),
      loginAt: DateTime.now(),
      token: 'demo-local-token',
    );
    notifyListeners();
  }

  void signOut() {
    _session = null;
    notifyListeners();
  }
}
