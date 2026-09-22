import 'building.dart';

/// Entidad Campus universitario (ms.svg - Campus Service / PostgreSQL).
class Campus {
  final String id;
  final String name;
  final String address;
  final double latitude;
  final double longitude;
  final List<Building> buildings;

  const Campus({
    required this.id,
    required this.name,
    required this.address,
    required this.latitude,
    required this.longitude,
    this.buildings = const [],
  });

  factory Campus.fromJson(Map<String, dynamic> json) {
    final coords = json['coordinates'] ?? json['Coordinates'] ?? {};
    final lat = (coords['latitude'] ?? coords['Latitude'] ?? json['latitude'] ?? 0.0).toDouble();
    final lng = (coords['longitude'] ?? coords['Longitude'] ?? json['longitude'] ?? 0.0).toDouble();

    final rawBuildings = json['buildings'] ?? json['Buildings'] as List<dynamic>? ?? [];
    final buildingsList = rawBuildings
        .whereType<Map<String, dynamic>>()
        .map((b) => Building.fromJson(b))
        .toList();

    return Campus(
      id: (json['id'] ?? json['Id'] ?? '').toString(),
      name: (json['name'] ?? json['Name'] ?? '').toString(),
      address: (json['address'] ?? json['Address'] ?? '').toString(),
      latitude: lat,
      longitude: lng,
      buildings: buildingsList,
    );
  }

  Map<String, dynamic> toJson() => {
        'id': id,
        'name': name,
        'address': address,
        'coordinates': {
          'latitude': latitude,
          'longitude': longitude,
        },
        'buildings': buildings.map((b) => b.toJson()).toList(),
      };
}
