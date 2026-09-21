import 'coordinates.dart';
import 'edificio.dart';

class Campus {
  final String id;
  final String nombre;
  final String direccion;
  final Coordinates coordenadas;
  final List<Edificio> edificios;

  const Campus({
    required this.id,
    required this.nombre,
    required this.direccion,
    required this.coordenadas,
    required this.edificios,
  });
}
