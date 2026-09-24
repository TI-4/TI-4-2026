import '../../domain/entities/coordinates.dart';
import '../../domain/entities/estructura.dart';

class StructureModel extends Estructura {
  const StructureModel({
    required super.id,
    required super.edificioId,
    required super.piso,
    required super.tipo,
    required super.numero,
    required super.coordenadas,
  });

  factory StructureModel.fromJson(Map<String, dynamic> json) {
    return StructureModel(
      id: json['id_estructura'] as String,
      edificioId: json['id_edificio'] as String,
      piso: json['piso'] as int,
      tipo: json['tipo'] as String,
      numero: json['numero'] as String,
      coordenadas: Coordinates(
        latitude: (json['coordenadas']['latitud'] as num).toDouble(),
        longitude: (json['coordenadas']['longitud'] as num).toDouble(),
      ),
    );
  }
}
