import 'dart:async';
import 'dart:convert';
import 'dart:io';

import 'package:http/http.dart' as http;

import '../../core/network/api_config.dart';
import '../../domain/repositories/auth_repository.dart';

/// Consume el endpoint de autenticación del backend vía API Gateway.
///
/// Envía exactamente `{Email, Password}` y acepta exactamente
/// `{UserId, Email, Token}`. Cualquier otra forma es error, no se adivina.
/// La contraseña viaja solo en el body del POST y nunca se guarda ni se
/// registra en logs.
class AuthRemoteDataSource implements AuthRepository {
  AuthRemoteDataSource({http.Client? client, ApiConfig? config})
      : _client = client ?? http.Client(),
        _config = config ?? ApiConfig();

  final http.Client _client;
  final ApiConfig _config;

  @override
  Future<AuthLoginResult> login({
    required String email,
    required String password,
  }) async {
    final http.Response res;
    try {
      res = await _client
          .post(
            _config.loginUri,
            headers: const {'Content-Type': 'application/json'},
            body: jsonEncode(
                {'Email': email.trim(), 'Password': password}),
          )
          .timeout(_config.timeout);
    } on TimeoutException {
      throw const AuthFailure(
          'El servidor no responde. Revisa tu conexión e intenta de nuevo.');
    } on SocketException {
      throw const AuthFailure(
          'Sin conexión con el servidor. Revisa tu conexión e intenta de nuevo.');
    } on http.ClientException {
      throw const AuthFailure(
          'Sin conexión con el servidor. Revisa tu conexión e intenta de nuevo.');
    } on HttpException {
      throw const AuthFailure(
          'Sin conexión con el servidor. Revisa tu conexión e intenta de nuevo.');
    } on FormatException {
      throw const AuthFailure(
          'Respuesta inesperada del servidor. Intenta de nuevo.');
    }
    return _parse(res.statusCode, res.body);
  }

  AuthLoginResult _parse(int status, String body) {
    if (status == 200) {
      try {
        final decoded = jsonDecode(body);
        if (decoded is Map<String, dynamic>) {
          final userId = decoded['UserId'];
          final mail = decoded['Email'];
          final token = decoded['Token'];
          if (userId is String &&
              userId.isNotEmpty &&
              mail is String &&
              mail.isNotEmpty &&
              token is String &&
              token.isNotEmpty) {
            return AuthLoginResult(
                userId: userId, email: mail, token: token);
          }
        }
      } on FormatException {
        // Cae al failure de respuesta inesperada.
      }
      throw const AuthFailure(
          'Respuesta inesperada del servidor. Intenta de nuevo.');
    }
    if (status == 400) {
      throw const AuthFailure('Faltan el correo o la contraseña.');
    }
    if (status == 401) {
      throw const AuthFailure('Correo o contraseña inválidos.');
    }
    throw AuthFailure('Error del servidor ($status). Intenta de nuevo.');
  }
}
