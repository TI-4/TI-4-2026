import 'package:flutter/material.dart';

import '../../../data/datasources/campus_remote_ds.dart';
import '../../../domain/entities/building.dart';
import '../../../domain/entities/campus.dart';
import '../../../domain/repositories/campus_repository.dart';

/// Visor interactivo del mapa conectado a Campus Service (ms.svg - /api/campus).
class MapScreen extends StatefulWidget {
  const MapScreen({super.key, this.campusRepository});

  final CampusRepository? campusRepository;

  @override
  State<MapScreen> createState() => _MapScreenState();
}

class _MapScreenState extends State<MapScreen> {
  late final CampusRepository _repository;
  List<Campus> _campuses = [];
  Campus? _selectedCampus;
  List<Building> _buildings = [];
  bool _loading = true;

  @override
  void initState() {
    super.initState();
    _repository = widget.campusRepository ?? CampusRemoteDataSource();
    _loadCampuses();
  }

  Future<void> _loadCampuses() async {
    setState(() => _loading = true);
    try {
      final list = await _repository.getCampuses();
      if (mounted) {
        setState(() {
          _campuses = list;
          if (list.isNotEmpty) {
            _selectedCampus = list.first;
          }
          _loading = false;
        });
        if (_selectedCampus != null) {
          _loadBuildings(_selectedCampus!.id);
        }
      }
    } catch (_) {
      if (mounted) setState(() => _loading = false);
    }
  }

  Future<void> _loadBuildings(String campusId) async {
    try {
      final bList = await _repository.getBuildings(campusId);
      if (mounted) {
        setState(() => _buildings = bList);
      }
    } catch (_) {}
  }

  @override
  Widget build(BuildContext context) {
    return Stack(
      children: [
        // Visor interactivo del mapa (Canvas / MapLibre / OpenStreetMap)
        Container(
          color: Colors.blueGrey.shade50,
          child: Center(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(
                  Icons.map,
                  size: 80,
                  color: Colors.blueGrey.shade300,
                ),
                const SizedBox(height: 16),
                const Text(
                  'Mapa Interactivo - Campus UCT',
                  style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                    color: Colors.blueGrey,
                  ),
                ),
                if (_selectedCampus != null) ...[
                  const SizedBox(height: 4),
                  Text(
                    _selectedCampus!.name,
                    style: const TextStyle(
                      fontSize: 14,
                      fontWeight: FontWeight.w600,
                      color: Color(0xFF003865),
                    ),
                  ),
                ],
                const SizedBox(height: 8),
                Padding(
                  padding: const EdgeInsets.symmetric(horizontal: 32.0),
                  child: Text(
                    _selectedCampus != null
                        ? '${_selectedCampus!.address}\n(${_buildings.length} edificios registrados)'
                        : 'Visualización de edificios, salas, pisos y rutas peatonales.',
                    textAlign: TextAlign.center,
                    style: const TextStyle(color: Colors.grey, fontSize: 13),
                  ),
                ),
              ],
            ),
          ),
        ),

        // Barra flotante superior: Selector de Campus
        Positioned(
          top: 12,
          left: 16,
          right: 16,
          child: Card(
            elevation: 4,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(30)),
            child: Padding(
              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
              child: Row(
                children: [
                  const Icon(Icons.location_city, color: Color(0xFF003865)),
                  const SizedBox(width: 10),
                  Expanded(
                    child: Text(
                      _selectedCampus?.name ?? 'Cargando campus...',
                      style: const TextStyle(fontWeight: FontWeight.w600),
                    ),
                  ),
                  IconButton(
                    icon: const Icon(Icons.keyboard_arrow_down),
                    tooltip: 'Cambiar Campus',
                    onPressed: _campuses.isEmpty ? null : () => _showCampusSelector(context),
                  ),
                ],
              ),
            ),
          ),
        ),

        // Botones flotantes de acción en el mapa (GPS, Capas)
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
                    const SnackBar(content: Text('Centrando en tu ubicación actual...')),
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
      builder: (modalCtx) {
        return Padding(
          padding: const EdgeInsets.all(16.0),
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
                    isSelected ? Icons.location_on : Icons.location_on_outlined,
                    color: isSelected ? const Color(0xFF003865) : null,
                  ),
                  title: Text(
                    campus.name,
                    style: TextStyle(
                      fontWeight: isSelected ? FontWeight.bold : FontWeight.normal,
                    ),
                  ),
                  subtitle: Text(campus.address),
                  trailing: isSelected ? const Icon(Icons.check, color: Colors.green) : null,
                  onTap: () {
                    Navigator.pop(modalCtx);
                    setState(() => _selectedCampus = campus);
                    _loadBuildings(campus.id);
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
