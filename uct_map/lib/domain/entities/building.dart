import 'room.dart';

/// Entidad Edificio dentro de un Campus (ms.svg - Campus Service).
class Building {
  final String id;
  final String campusId;
  final String name;
  final int floorsCount;
  final double latitude;
  final double longitude;
  final List<Room> rooms;

  const Building({
    required this.id,
    required this.campusId,
    required this.name,
    required this.floorsCount,
    required this.latitude,
    required this.longitude,
    this.rooms = const [],
  });

  factory Building.fromJson(Map<String, dynamic> json) {
    final coords = json['coordinates'] ?? json['Coordinates'] ?? {};
    final lat = (coords['latitude'] ?? coords['Latitude'] ?? json['latitude'] ?? 0.0).toDouble();
    final lng = (coords['longitude'] ?? coords['Longitude'] ?? json['longitude'] ?? 0.0).toDouble();

    final rawRooms = json['rooms'] ?? json['Rooms'] as List<dynamic>? ?? [];
    final roomsList = rawRooms
        .whereType<Map<String, dynamic>>()
        .map((r) => Room.fromJson(r))
        .toList();

    return Building(
      id: (json['id'] ?? json['Id'] ?? '').toString(),
      campusId: (json['campusId'] ?? json['CampusId'] ?? '').toString(),
      name: (json['name'] ?? json['Name'] ?? '').toString(),
      floorsCount: (json['floorsCount'] ?? json['FloorsCount'] ?? 1) as int,
      latitude: lat,
      longitude: lng,
      rooms: roomsList,
    );
  }

  Map<String, dynamic> toJson() => {
        'id': id,
        'campusId': campusId,
        'name': name,
        'floorsCount': floorsCount,
        'coordinates': {
          'latitude': latitude,
          'longitude': longitude,
        },
        'rooms': rooms.map((r) => r.toJson()).toList(),
      };
}
