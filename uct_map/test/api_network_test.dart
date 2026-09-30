import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:http/testing.dart';
import 'package:uct_map/core/network/api_config.dart';
import 'package:uct_map/core/network/api_endpoints.dart';
import 'package:uct_map/core/network/api_exception.dart';
import 'package:uct_map/core/network/authenticated_client.dart';

void main() {
  group('ApiConfig & ApiEndpoints Tests', () {
    test('Resuelve URIs correctas para los 4 microservicios', () {
      final config = ApiConfig(baseUrl: 'http://localhost:5052');

      expect(
        config.uri(ApiEndpoints.identityLogin).toString(),
        'http://localhost:5052/api/identity/login',
      );
      expect(
        config.uri(ApiEndpoints.campusList).toString(),
        'http://localhost:5052/api/campus',
      );
      expect(
        config.uri(ApiEndpoints.campusBuildings('c-1')).toString(),
        'http://localhost:5052/api/campus/c-1/buildings',
      );
      expect(
        config.uri(ApiEndpoints.scheduleProfessors).toString(),
        'http://localhost:5052/api/schedule/professors',
      );
      expect(
        config.uri(ApiEndpoints.incidentLostItems).toString(),
        'http://localhost:5052/api/incident/lost-items',
      );
      expect(
        config.uri(ApiEndpoints.incidentReports, {'campus': 'San Francisco'}).toString(),
        'http://localhost:5052/api/incident/reports?campus=San+Francisco',
      );
    });
  });

  group('AuthenticatedClient Tests', () {
    test('Inyecta header Authorization: Bearer cuando el token existe', () async {
      String? capturedAuthHeader;

      final mockInner = MockClient((request) async {
        capturedAuthHeader = request.headers['Authorization'];
        return http.Response('{"ok": true}', 200);
      });

      final client = AuthenticatedClient(
        innerClient: mockInner,
        tokenProvider: () => 'jwt-test-token-123',
      );

      final response = await client.get(Uri.parse('http://localhost:5052/api/campus'));

      expect(response.statusCode, 200);
      expect(capturedAuthHeader, 'Bearer jwt-test-token-123');
    });

    test('Llama a onUnauthorized cuando el Gateway devuelve 401', () async {
      bool unauthorizedCalled = false;

      final mockInner = MockClient((request) async {
        return http.Response('{"error": "Unauthorized"}', 401);
      });

      final client = AuthenticatedClient(
        innerClient: mockInner,
        tokenProvider: () => 'expired-token',
        onUnauthorized: () {
          unauthorizedCalled = true;
        },
      );

      final response = await client.get(Uri.parse('http://localhost:5052/api/schedule/meetings'));

      expect(response.statusCode, 401);
      expect(unauthorizedCalled, isTrue);
    });
  });

  group('ApiException Hierarchy Tests', () {
    test('Verifica propiedades de excepciones tipadas', () {
      const unauth = UnauthorizedException();
      expect(unauth.statusCode, 401);

      const forbidden = ForbiddenException();
      expect(forbidden.statusCode, 403);

      const notFound = NotFoundException();
      expect(notFound.statusCode, 404);

      const netErr = NetworkException('Sin conexión');
      expect(netErr.message, 'Sin conexión');
    });
  });
}
