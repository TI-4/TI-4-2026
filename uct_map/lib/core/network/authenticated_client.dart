import 'dart:async';
import 'package:http/http.dart' as http;

typedef TokenProvider = String? Function();

/// Cliente HTTP que inyecta automáticamente el token JWT Bearer
/// en las cabeceras de cada petición dirigida al API Gateway
/// y detecta respuestas 401 para gestionar la expiración de sesión.
class AuthenticatedClient extends http.BaseClient {
  AuthenticatedClient({
    http.Client? innerClient,
    this.tokenProvider,
    this.onUnauthorized,
  }) : _inner = innerClient ?? http.Client();

  final http.Client _inner;
  final TokenProvider? tokenProvider;
  final void Function()? onUnauthorized;

  @override
  Future<http.StreamedResponse> send(http.BaseRequest request) async {
    // Inyectar Token Bearer si está disponible y no se configuró manualmente
    final token = tokenProvider?.call();
    if (token != null && token.isNotEmpty && !request.headers.containsKey('Authorization')) {
      request.headers['Authorization'] = 'Bearer $token';
    }

    // Asegurar encabezados JSON por defecto
    if (!request.headers.containsKey('Accept')) {
      request.headers['Accept'] = 'application/json';
    }

    final response = await _inner.send(request);

    // Si el Gateway retorna 401 Unauthorized, notificar para revocar sesión
    if (response.statusCode == 401) {
      onUnauthorized?.call();
    }

    return response;
  }

  @override
  void close() {
    _inner.close();
    super.close();
  }
}
