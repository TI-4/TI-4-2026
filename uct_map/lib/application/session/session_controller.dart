import 'package:flutter/foundation.dart';

import '../../core/auth/jwt_claims.dart';
import '../../data/storage/token_storage.dart';
import '../../domain/entities/auth_session.dart';
import '../../domain/entities/user.dart';
import '../../domain/entities/user_role.dart';
import '../../domain/repositories/auth_repository.dart';

// Sesión en memoria con forma del backend y token persistido.
class SessionController extends ChangeNotifier {
  SessionController({TokenStorage? storage})
      : _storage = storage ?? SecureTokenStorage();

  final TokenStorage _storage;
  AuthSession? _session;

  bool get isAuthenticated => _session != null && _session!.isValid;
  User? get currentUser => _session?.user;
  String? get token {
    final session = _session;
    final value = session?.token;
    if (session == null || !session.isValid) return null;
    if (value == null || value.isEmpty) return null;
    return value;
  }

  void signInDemo(String email) {
    _session = AuthSession(
      user: User.fromLogin(userId: 'local', email: email.trim()),
      loginAt: DateTime.now(),
      token: 'demo-local-token',
    );
    notifyListeners();
  }

  Future<void> signInReal(AuthLoginResult result) async {
    final claims = parseJwt(result.token);
    _session = AuthSession(
      user: User.fromLogin(
        userId: result.userId,
        email: result.email,
        role: claims.roles.isNotEmpty
            ? claims.roles.first
            : UserRole.desconocido,
      ),
      loginAt: DateTime.now(),
      token: result.token,
      expiresAt: claims.expiresAt,
    );
    await _storage.save(result.token);
    notifyListeners();
  }

  Future<bool> restore() async {
    final token = await _storage.read();
    if (token == null || token.isEmpty) return false;
    try {
      final claims = parseJwt(token);
      if (DateTime.now().isAfter(claims.expiresAt)) {
        await _storage.clear();
        return false;
      }
      _session = AuthSession(
        user: User.fromLogin(
          userId: claims.subject,
          email: claims.email,
          role: claims.roles.isNotEmpty
              ? claims.roles.first
              : UserRole.desconocido,
        ),
        loginAt: DateTime.now(),
        token: token,
        expiresAt: claims.expiresAt,
      );
      notifyListeners();
      return true;
    } on AuthFailure {
      await _storage.clear();
      return false;
    }
  }

  void signOut() {
    _session = null;
    _storage.clear();
    notifyListeners();
  }
}
