/// Jerarquía de excepciones de red y API para la aplicación UCT Map.
class ApiException implements Exception {
  final String message;
  final int? statusCode;

  const ApiException(this.message, [this.statusCode]);

  @override
  String toString() =>
      statusCode != null ? 'ApiException($statusCode): $message' : 'ApiException: $message';
}

/// Error 401 Unauthorized: token expirado o credenciales inválidas.
class UnauthorizedException extends ApiException {
  const UnauthorizedException([String message = 'Sesión expirada o no autorizada.'])
      : super(message, 401);
}

/// Error 403 Forbidden: permisos insuficientes para el recurso.
class ForbiddenException extends ApiException {
  const ForbiddenException([String message = 'Acceso no permitido para este rol.'])
      : super(message, 403);
}

/// Error 404 Not Found: recurso no encontrado.
class NotFoundException extends ApiException {
  const NotFoundException([String message = 'Recurso no encontrado.'])
      : super(message, 404);
}

/// Error de conectividad de red o timeout.
class NetworkException extends ApiException {
  const NetworkException([String message = 'Sin conexión con el servidor. Revisa tu red.'])
      : super(message);
}

/// Error 500+ Internal Server Error.
class ServerException extends ApiException {
  const ServerException([String message = 'Error interno del servidor.'])
      : super(message, 500);
}
