import 'dart:async';
import 'dart:convert';
import 'dart:io';

import 'package:http/http.dart' as http;

import '../../core/network/api_config.dart';
import '../../core/network/api_endpoints.dart';
import '../../core/network/api_exception.dart';
import '../../domain/entities/office_hour.dart';
import '../../domain/entities/professor.dart';
import '../../domain/entities/schedule_meeting.dart';
import '../../domain/repositories/schedule_repository.dart';

/// Datasource remoto para consumir /api/schedule a través del API Gateway.
class ScheduleRemoteDataSource implements ScheduleRepository {
  ScheduleRemoteDataSource({http.Client? client, ApiConfig? config})
      : _client = client ?? http.Client(),
        _config = config ?? ApiConfig();

  final http.Client _client;
  final ApiConfig _config;

  @override
  Future<List<Professor>> getProfessors() async {
    final uri = _config.uri(ApiEndpoints.scheduleProfessors);
    try {
      final res = await _client.get(uri).timeout(_config.timeout);
      if (res.statusCode == 200) {
        final decoded = jsonDecode(res.body);
        if (decoded is List) {
          return decoded
              .whereType<Map<String, dynamic>>()
              .map((p) => Professor.fromJson(p))
              .toList();
        }
      }
      if (res.statusCode == 401) {
        throw const UnauthorizedException();
      }
      throw ApiException('Error al obtener profesores (${res.statusCode})', res.statusCode);
    } on SocketException {
      return _fallbackProfessors;
    } on http.ClientException {
      return _fallbackProfessors;
    } on TimeoutException {
      return _fallbackProfessors;
    }
  }

  @override
  Future<List<OfficeHour>> getOfficeHours(String teacherId) async {
    final uri = _config.uri(ApiEndpoints.professorOfficeHours(teacherId));
    try {
      final res = await _client.get(uri).timeout(_config.timeout);
      if (res.statusCode == 200) {
        final decoded = jsonDecode(res.body);
        if (decoded is List) {
          return decoded
              .whereType<Map<String, dynamic>>()
              .map((h) => OfficeHour.fromJson(h))
              .toList();
        }
      }
      if (res.statusCode == 401) {
        throw const UnauthorizedException();
      }
      throw ApiException('Error al obtener horarios (${res.statusCode})', res.statusCode);
    } on SocketException {
      return [];
    } on http.ClientException {
      return [];
    } on TimeoutException {
      return [];
    }
  }

  @override
  Future<List<ScheduleMeeting>> getMyMeetings() async {
    final uri = _config.uri(ApiEndpoints.scheduleMeetings);
    try {
      final res = await _client.get(uri).timeout(_config.timeout);
      if (res.statusCode == 200) {
        final decoded = jsonDecode(res.body);
        if (decoded is List) {
          return decoded
              .whereType<Map<String, dynamic>>()
              .map((m) => ScheduleMeeting.fromJson(m))
              .toList();
        }
      }
      if (res.statusCode == 401) {
        throw const UnauthorizedException();
      }
      throw ApiException('Error al obtener citas (${res.statusCode})', res.statusCode);
    } on SocketException {
      return [];
    } on http.ClientException {
      return [];
    } on TimeoutException {
      return [];
    }
  }

  @override
  Future<ScheduleMeeting> createMeeting({
    required String teacherRefId,
    required String structureRefId,
    required DateTime scheduledAt,
  }) async {
    final uri = _config.uri(ApiEndpoints.scheduleMeetings);
    try {
      final res = await _client
          .post(
            uri,
            headers: const {'Content-Type': 'application/json'},
            body: jsonEncode({
              'teacherRefId': teacherRefId,
              'structureRefId': structureRefId,
              'scheduledAt': scheduledAt.toIso8601String(),
            }),
          )
          .timeout(_config.timeout);

      if (res.statusCode == 200 || res.statusCode == 201) {
        final decoded = jsonDecode(res.body);
        if (decoded is Map<String, dynamic>) {
          return ScheduleMeeting.fromJson(decoded);
        }
      }
      if (res.statusCode == 401) {
        throw const UnauthorizedException();
      }
      throw ApiException('Error al agendar cita (${res.statusCode})', res.statusCode);
    } on SocketException {
      throw const NetworkException();
    } on TimeoutException {
      throw const NetworkException('Tiempo de espera agotado al agendar cita.');
    }
  }

  @override
  Future<bool> updateMeetingStatus(String meetingId, MeetingStatus status) async {
    final uri = _config.uri(ApiEndpoints.meetingStatus(meetingId));
    try {
      final res = await _client
          .patch(
            uri,
            headers: const {'Content-Type': 'application/json'},
            body: jsonEncode({'status': status.name}),
          )
          .timeout(_config.timeout);

      return res.statusCode == 200 || res.statusCode == 204;
    } on Exception {
      return false;
    }
  }

  static const List<Professor> _fallbackProfessors = [
    Professor(
      id: 'prof-1',
      name: 'Dr. Roberto González',
      department: 'Ingeniería de Software',
      office: 'Oficina 304 - Edificio Central',
      email: 'rgonzalez@uct.cl',
      officeHours: [
        OfficeHour(
          id: 'oh-1',
          teacherRefId: 'prof-1',
          dayOfWeek: 2,
          startTime: '10:00',
          endTime: '12:00',
        ),
      ],
    ),
    Professor(
      id: 'prof-2',
      name: 'Dra. Marcela Soto',
      department: 'Ciencias de la Computación',
      office: 'Oficina 210 - Edificio C',
      email: 'msoto@uct.cl',
      officeHours: [
        OfficeHour(
          id: 'oh-2',
          teacherRefId: 'prof-2',
          dayOfWeek: 4,
          startTime: '14:30',
          endTime: '16:30',
        ),
      ],
    ),
    Professor(
      id: 'prof-3',
      name: 'Mg. Carlos Peña',
      department: 'Redes y Telecomunicaciones',
      office: 'Oficina 105 - Laboratorios',
      email: 'cpena@uct.cl',
      officeHours: [
        OfficeHour(
          id: 'oh-3',
          teacherRefId: 'prof-3',
          dayOfWeek: 3,
          startTime: '09:00',
          endTime: '11:00',
        ),
      ],
    ),
  ];
}
