/// Horario de atención de un docente (ms.svg - Schedule Service).
class OfficeHour {
  final String id;
  final String teacherRefId;
  final int dayOfWeek; // 1 = Lunes ... 7 = Domingo
  final String startTime; // ej. "14:30"
  final String endTime;   // ej. "16:00"

  const OfficeHour({
    required this.id,
    required this.teacherRefId,
    required this.dayOfWeek,
    required this.startTime,
    required this.endTime,
  });

  String get dayName {
    switch (dayOfWeek) {
      case 1:
        return 'Lunes';
      case 2:
        return 'Martes';
      case 3:
        return 'Miércoles';
      case 4:
        return 'Jueves';
      case 5:
        return 'Viernes';
      case 6:
        return 'Sábado';
      case 7:
        return 'Domingo';
      default:
        return 'Día $dayOfWeek';
    }
  }

  factory OfficeHour.fromJson(Map<String, dynamic> json) {
    return OfficeHour(
      id: (json['id'] ?? json['Id'] ?? '').toString(),
      teacherRefId: (json['teacherRefId'] ?? json['TeacherRefId'] ?? '').toString(),
      dayOfWeek: (json['dayOfWeek'] ?? json['DayOfWeek'] ?? 1) as int,
      startTime: (json['startTime'] ?? json['StartTime'] ?? '').toString(),
      endTime: (json['endTime'] ?? json['EndTime'] ?? '').toString(),
    );
  }

  Map<String, dynamic> toJson() => {
        'id': id,
        'teacherRefId': teacherRefId,
        'dayOfWeek': dayOfWeek,
        'startTime': startTime,
        'endTime': endTime,
      };
}
