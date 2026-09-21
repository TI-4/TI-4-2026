import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong2/latlong.dart';

import '../../../data/datasources/campus_local_datasource.dart';
import '../../../data/repositories/campus_repository_impl.dart';
import '../../../domain/entities/campus.dart';
import 'widgets/edificio_marker.dart';

class MapScreen extends StatefulWidget {
  const MapScreen({super.key});

  @override
  State<MapScreen> createState() => _MapScreenState();
}

class _MapScreenState extends State<MapScreen> {
  late final CampusRepositoryImpl _campusRepository;
  final MapController _mapController = MapController();

  List<Campus> _campuses = [];
  Campus? _selectedCampus;

  @override
  void initState() {
    super.initState();

    _campusRepository = CampusRepositoryImpl(
      localDataSource: CampusLocalDataSource(),
    );

    _loadCampuses();
  }

  Future<void> _loadCampuses() async {
    final campuses = await _campusRepository.getCampuses();

    if (!mounted) return;

    setState(() {
      _campuses = campuses;

      if (campuses.isNotEmpty) {
        _selectedCampus = campuses.first;
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    final campus = _selectedCampus;

    if (campus == null) {
      return const Center(child: CircularProgressIndicator());
    }

    return Stack(
      children: [
        FlutterMap(
          mapController: _mapController,
          options: MapOptions(
            initialCenter: LatLng(
              campus.coordenadas.latitude,
              campus.coordenadas.longitude,
            ),
            initialZoom: 16,
          ),
          children: [
            TileLayer(
              urlTemplate: 'https://tile.openstreetmap.org/{z}/{x}/{y}.png',
              userAgentPackageName: 'cl.cl.uct.uct_map',
            ),

            MarkerLayer(
              markers: campus.edificios.map((edificio) {
                return EdificioMarker(
                  edificio: edificio,
                  onTap: () {
                    ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(
                        content: Text(
                          '${edificio.nombre} - ${edificio.cantidadPisos} pisos',
                        ),
                      ),
                    );
                  },
                );
              }).toList(),
            ),
          ],
        ),

        // Selector de Campus
        Positioned(
          top: 12,
          left: 16,
          right: 16,
          child: Card(
            elevation: 4,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(30),
            ),
            child: Padding(
              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
              child: Row(
                children: [
                  const Icon(Icons.location_city, color: Color(0xFF003865)),
                  const SizedBox(width: 10),

                  Expanded(
                    child: Text(
                      campus.nombre,
                      style: const TextStyle(fontWeight: FontWeight.w600),
                    ),
                  ),

                  IconButton(
                    icon: const Icon(Icons.keyboard_arrow_down),
                    tooltip: 'Cambiar Campus',
                    onPressed: () {
                      _showCampusSelector(context);
                    },
                  ),
                ],
              ),
            ),
          ),
        ),

        // Botones flotantes
        Positioned(
          right: 16,
          bottom: 24,
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              FloatingActionButton.small(
                heroTag: 'map_layers',
                backgroundColor: Colors.white,
                foregroundColor: const Color(0xFF003865),
                onPressed: () {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text('Selección de capas de mapa')),
                  );
                },
                tooltip: 'Capas',
                child: const Icon(Icons.layers_outlined),
              ),
              const SizedBox(height: 10),
              FloatingActionButton(
                heroTag: 'map_gps',
                backgroundColor: const Color(0xFF003865),
                foregroundColor: Colors.white,
                onPressed: () {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Centrando en tu ubicación actual...'),
                    ),
                  );
                },
                tooltip: 'Mi ubicación',
                child: const Icon(Icons.my_location),
              ),
            ],
          ),
        ),
      ],
    );
  }

  void _showCampusSelector(BuildContext context) {
    showModalBottomSheet(
      context: context,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      builder: (context) {
        return Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              const Text(
                'Selecciona un Campus',
                style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
              ),
              const SizedBox(height: 12),

              ..._campuses.map((campus) {
                final isSelected = campus.id == _selectedCampus?.id;

                return ListTile(
                  leading: Icon(
                    Icons.location_on_outlined,
                    color: isSelected ? const Color(0xFF003865) : Colors.grey,
                  ),
                  title: Text(campus.nombre),
                  subtitle: Text(campus.direccion),
                  trailing: isSelected
                      ? const Icon(Icons.check, color: Colors.green)
                      : null,
                  onTap: () {
                    setState(() {
                      _selectedCampus = campus;
                    });
                    _mapController.move(
                      LatLng(
                        campus.coordenadas.latitude,
                        campus.coordenadas.longitude,
                      ),
                      16,
                    );

                    Navigator.pop(context);
                  },
                );
              }),
            ],
          ),
        );
      },
    );
  }
}
