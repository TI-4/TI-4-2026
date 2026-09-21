import '../../domain/entities/coordinates.dart';
import '../../domain/entities/edificio.dart';

class BuildingModel extends Edificio {
  const BuildingModel({
    required super.id,
    required super.campusId,
    required super.nombre,
    required super.cantidadPisos,
    required super.coordenadas,
  });

  factory BuildingModel.fromJson(Map<String, dynamic> json) {
    return BuildingModel(
      id: json['id_edificio'] as String,
      campusId: json['id_campus'] as String,
      nombre: json['nombre'] as String,
      cantidadPisos: json['cant_pisos'] as int,
      coordenadas: Coordinates(
        latitude: (json['coordenadas']['latitud'] as num).toDouble(),
        longitude: (json['coordenadas']['longitud'] as num).toDouble(),
      ),
    );
  }
}
