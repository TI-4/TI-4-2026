import 'coordinates.dart';

class Estructura {
  final String id;
  final String edificioId;
  final int piso;
  final String tipo;
  final String numero;
  final Coordinates coordenadas;

  const Estructura({
    required this.id,
    required this.edificioId,
    required this.piso,
    required this.tipo,
    required this.numero,
    required this.coordenadas,
  });
}
