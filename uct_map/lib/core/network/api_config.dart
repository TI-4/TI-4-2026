import 'api_endpoints.dart';

// URL base del API Gateway y rutas del backend.
class ApiConfig {
  ApiConfig({
    this.baseUrl = ApiConfig.defaultBaseUrl,
    this.timeout = ApiConfig.defaultTimeout,
  });

  static const String defaultBaseUrl = 'http://localhost:5052';

  /// Host del Gateway visto desde un emulador Android.
  static const String androidEmulatorBaseUrl = 'http://10.0.2.2:5052';

  static const String loginPath = ApiEndpoints.identityLogin;

  static const Duration defaultTimeout = Duration(seconds: 15);

  final String baseUrl;
  final Duration timeout;

  /// Retorna un objeto [Uri] completo a partir de una ruta del Gateway y parámetros opcionales.
  Uri uri(String path, [Map<String, dynamic>? queryParameters]) {
    final cleanPath = path.startsWith('/') ? path : '/$path';
    final fullUrl = '$baseUrl$cleanPath';
    final baseUri = Uri.parse(fullUrl);
    if (queryParameters == null || queryParameters.isEmpty) {
      return baseUri;
    }
    final stringParams = queryParameters.map(
      (key, value) => MapEntry(key, value?.toString() ?? ''),
    );
    return baseUri.replace(queryParameters: stringParams);
  }

  Uri get loginUri => uri(loginPath);
}
