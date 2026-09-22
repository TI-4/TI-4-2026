import 'dart:convert';
import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:http/testing.dart';
import 'package:uct_map/core/network/api_config.dart';
import 'package:uct_map/data/datasources/schedule_remote_ds.dart';
import 'package:uct_map/domain/entities/office_hour.dart';
import 'package:uct_map/domain/entities/professor.dart';
import 'package:uct_map/domain/entities/schedule_meeting.dart';

void main() {
  group('Schedule Service Domain & Datasource Tests', () {
    test('Professor y OfficeHour parsean correctamente', () {
      final profJson = {
        'id': 'prof-99',
        'name': 'Dr. Alan Turing',
        'department': 'Ciencias de la Computación',
        'office': 'Oficina 404',
        'email': 'aturing@uct.cl',
        'officeHours': [
          {
            'id': 'oh-1',
            'teacherRefId': 'prof-99',
            'dayOfWeek': 3,
            'startTime': '11:00',
            'endTime': '13:00',
          }
        ],
      };

      final prof = Professor.fromJson(profJson);
      expect(prof.id, 'prof-99');
      expect(prof.name, 'Dr. Alan Turing');
      expect(prof.officeHours.length, 1);
      expect(prof.officeHours.first.dayName, 'Miércoles');
      expect(prof.officeHours.first.startTime, '11:00');
    });

    test('ScheduleRemoteDataSource agenda una cita exitosamente con POST /api/schedule/meetings', () async {
      final mockMeetingJson = {
        'id': 'meeting-123',
        'teacherRefId': 'prof-1',
        'studentRefId': 'student-1',
        'structureRefId': 'room-101',
        'scheduledAt': '2026-09-25T10:00:00.000Z',
        'status': 'Pending',
      };

      final mockClient = MockClient((request) async {
        expect(request.method, 'POST');
        expect(request.url.path, '/api/schedule/meetings');
        return http.Response(jsonEncode(mockMeetingJson), 201);
      });

      final ds = ScheduleRemoteDataSource(
        client: mockClient,
        config: ApiConfig(baseUrl: 'http://localhost:5052'),
      );

      final meeting = await ds.createMeeting(
        teacherRefId: 'prof-1',
        structureRefId: 'room-101',
        scheduledAt: DateTime.parse('2026-09-25T10:00:00.000Z'),
      );

      expect(meeting.id, 'meeting-123');
      expect(meeting.status, MeetingStatus.pending);
    });

    test('ScheduleRemoteDataSource activa fallback de profesores ante error de red', () async {
      final mockClient = MockClient((request) async {
        throw http.ClientException('Socket error');
      });

      final ds = ScheduleRemoteDataSource(client: mockClient);
      final list = await ds.getProfessors();

      expect(list.isNotEmpty, isTrue);
      expect(list.any((p) => p.name.contains('Roberto González')), isTrue);
    });
  });
}
