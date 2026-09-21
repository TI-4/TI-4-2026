import 'coordinates.dart';

class Edificio {
  final String id;
  final String campusId;
  final String nombre;
  final int cantidadPisos;
  final Coordinates coordenadas;

  const Edificio({
    required this.id,
    required this.campusId,
    required this.nombre,
    required this.cantidadPisos,
    required this.coordenadas,
  });
}
