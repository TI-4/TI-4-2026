import 'user.dart';

// Sesión básica en memoria. El token JWT y la expiración real llegan en tarea 4.
class AuthSession {
  final User user;
  final DateTime loginAt;

  const AuthSession({required this.user, required this.loginAt});

  bool get isValid => true;
}
