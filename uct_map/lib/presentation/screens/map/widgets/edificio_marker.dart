import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong2/latlong.dart';

import '../../../../domain/entities/edificio.dart';

class EdificioMarker extends Marker {
  EdificioMarker({required Edificio edificio, VoidCallback? onTap})
    : super(
        point: LatLng(
          edificio.coordenadas.latitude,
          edificio.coordenadas.longitude,
        ),
        width: 50,
        height: 50,
        child: GestureDetector(
          onTap: onTap,
          child: const Icon(Icons.location_on, size: 40, color: Colors.red),
        ),
      );
}
