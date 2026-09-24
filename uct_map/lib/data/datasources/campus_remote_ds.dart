import 'dart:async';
import 'dart:convert';
import 'dart:io';

import 'package:http/http.dart' as http;

import '../../core/network/api_config.dart';
import '../../core/network/api_endpoints.dart';
import '../../core/network/api_exception.dart';
import '../../domain/entities/building.dart';
import '../../domain/entities/campus.dart';
import '../../domain/entities/room.dart';
import '../../domain/repositories/campus_repository.dart';

/// Datasource remoto para consumir /api/campus a través del API Gateway.
class CampusRemoteDataSource implements CampusRepository {
  CampusRemoteDataSource({http.Client? client, ApiConfig? config})
      : _client = client ?? http.Client(),
        _config = config ?? ApiConfig();

  final http.Client _client;
  final ApiConfig _config;

  @override
  Future<List<Campus>> getCampuses() async {
    final uri = _config.uri(ApiEndpoints.campusList);
    try {
      final res = await _client.get(uri).timeout(_config.timeout);
      if (res.statusCode == 200) {
        final decoded = jsonDecode(res.body);
        if (decoded is List) {
          return decoded
              .whereType<Map<String, dynamic>>()
              .map((c) => Campus.fromJson(c))
              .toList();
        }
      }
      if (res.statusCode == 401) {
        throw const UnauthorizedException();
      }
      throw ApiException('Error al obtener campus (${res.statusCode})', res.statusCode);
    } on SocketException {
      return fallbackCampuses;
    } on http.ClientException {
      return fallbackCampuses;
    } on TimeoutException {
      return fallbackCampuses;
    }
  }

  @override
  Future<List<Building>> getBuildings(String campusId) async {
    final uri = _config.uri(ApiEndpoints.campusBuildings(campusId));
    try {
      final res = await _client.get(uri).timeout(_config.timeout);
      if (res.statusCode == 200) {
        final decoded = jsonDecode(res.body);
        if (decoded is List) {
          return decoded
              .whereType<Map<String, dynamic>>()
              .map((b) => Building.fromJson(b))
              .toList();
        }
      }
      if (res.statusCode == 401) {
        throw const UnauthorizedException();
      }
      throw ApiException('Error al obtener edificios (${res.statusCode})', res.statusCode);
    } on SocketException {
      return [];
    } on http.ClientException {
      return [];
    } on TimeoutException {
      return [];
    }
  }

  @override
  Future<List<Room>> getRooms(String buildingId) async {
    final uri = _config.uri(ApiEndpoints.buildingRooms(buildingId));
    try {
      final res = await _client.get(uri).timeout(_config.timeout);
      if (res.statusCode == 200) {
        final decoded = jsonDecode(res.body);
        if (decoded is List) {
          return decoded
              .whereType<Map<String, dynamic>>()
              .map((r) => Room.fromJson(r))
              .toList();
        }
      }
      if (res.statusCode == 401) {
        throw const UnauthorizedException();
      }
      throw ApiException('Error al obtener salas (${res.statusCode})', res.statusCode);
    } on SocketException {
      return [];
    } on http.ClientException {
      return [];
    } on TimeoutException {
      return [];
    }
  }

  // Campus de ejemplo para ver la funcionalidad sin red o con servicios apagados.
  static const List<Campus> fallbackCampuses = [
    Campus(
      id: 'campus-sjpii',
      name: 'Campus San Juan Pablo II',
      address: 'Rudecindo Ortega 02950, Temuco',
      latitude: -38.7042,
      longitude: -72.5849,
    ),
    Campus(
      id: 'campus-sf',
      name: 'Campus San Francisco',
      address: 'Manuel Montt 056, Temuco',
      latitude: -38.7369,
      longitude: -72.6021,
    ),
    Campus(
      id: 'campus-menchaca',
      name: 'Campus Menchaca Lira',
      address: 'Av. Alemania 0422, Temuco',
      latitude: -38.7361,
      longitude: -72.6080,
    ),
  ];
}
