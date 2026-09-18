// URL base del API Gateway y rutas del backend.
class ApiConfig {
  ApiConfig({
    this.baseUrl = ApiConfig.defaultBaseUrl,
    this.timeout = ApiConfig.defaultTimeout,
  });

  static const String defaultBaseUrl = 'http://localhost:5052';

  /// Host del Gateway visto desde un emulador Android.
  static const String androidEmulatorBaseUrl = 'http://10.0.2.2:5052';

  static const String loginPath = '/api/identity/login';

  static const Duration defaultTimeout = Duration(seconds: 15);

  final String baseUrl;
  final Duration timeout;

  Uri get loginUri => Uri.parse('$baseUrl$loginPath');
}
