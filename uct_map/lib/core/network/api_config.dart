// Punto único de salida del móvil hacia el backend.
//
// Todo el tráfico pasa por el API Gateway (Móvil → Gateway → Identity).
// El puerto directo de identity (:5001) queda solo para debug del backend.
// En emulador Android el loopback del host es 10.0.2.2: usar
// [androidEmulatorBaseUrl] en ese caso.
class ApiConfig {
  ApiConfig({
    this.baseUrl = ApiConfig.defaultBaseUrl,
    this.timeout = ApiConfig.defaultTimeout,
  });

  static const String defaultBaseUrl = 'http://localhost:5052';

  /// Loopback del host visto desde un emulador Android.
  static const String androidEmulatorBaseUrl = 'http://10.0.2.2:5052';

  static const String loginPath = '/api/identity/login';

  static const Duration defaultTimeout = Duration(seconds: 15);

  final String baseUrl;
  final Duration timeout;

  Uri get loginUri => Uri.parse('$baseUrl$loginPath');
}
