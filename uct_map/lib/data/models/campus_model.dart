import '../../domain/entities/campus.dart';
import '../../domain/entities/coordinates.dart';
import 'building_model.dart';

class CampusModel extends Campus {
  const CampusModel({
    required super.id,
    required super.nombre,
    required super.direccion,
    required super.coordenadas,
    required super.edificios,
  });

  factory CampusModel.fromJson(Map<String, dynamic> json) {
    final edificiosJson = json['edificios'] as List<dynamic>? ?? [];

    return CampusModel(
      id: json['id_campus'] as String,
      nombre: json['nombre'] as String,
      direccion: json['direccion'] as String,
      coordenadas: Coordinates(
        latitude: (json['coordenadas']['latitud'] as num).toDouble(),
        longitude: (json['coordenadas']['longitud'] as num).toDouble(),
      ),
      edificios: edificiosJson
          .map(
            (edificio) =>
                BuildingModel.fromJson(edificio as Map<String, dynamic>),
          )
          .toList(),
    );
  }
}
