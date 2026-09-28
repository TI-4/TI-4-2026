import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong2/latlong.dart';

import '../../../data/datasources/campus_remote_ds.dart';
import '../../../data/repositories/campus_repository_impl.dart';
import '../../../domain/entities/building.dart';
import '../../../domain/entities/campus.dart';
import '../../../domain/repositories/campus_repository.dart';

class MapScreen extends StatefulWidget {
  const MapScreen({super.key, this.campusRepository});

  final CampusRepository? campusRepository;

  @override
  State<MapScreen> createState() => _MapScreenState();
}

class _MapScreenState extends State<MapScreen> {
  late final CampusRepository _repository;
  final MapController _mapController = MapController();

  List<Campus> _campuses = [];
  Campus? _selectedCampus;
  List<Building> _buildings = [];

  bool _loading = true;

  @override
  void initState() {
    super.initState();

    _repository =
        widget.campusRepository ??
        CampusRepositoryImpl(remoteDataSource: CampusRemoteDataSource());

    _loadCampuses();
  }

  Future<void> _loadCampuses() async {
    setState(() => _loading = true);

    try {
      final campuses = await _repository.getCampuses();

      if (!mounted) return;

      setState(() {
        _campuses = campuses;

        if (campuses.isNotEmpty) {
          _selectedCampus = campuses.first;
        }

        _loading = false;
      });

      if (_selectedCampus != null) {
        await _loadBuildings(_selectedCampus!.id);
      }
    } catch (_) {
      if (mounted) {
        setState(() => _loading = false);
      }
    }
  }

  Future<void> _loadBuildings(String campusId) async {
    try {
      final buildings = await _repository.getBuildings(campusId);

      if (!mounted) return;

      setState(() {
        _buildings = buildings;
      });

      if (buildings.isNotEmpty) {
        _mapController.move(
          LatLng(buildings.first.latitude, buildings.first.longitude),
          17,
        );
      }
    } catch (_) {
      if (mounted) {
        setState(() {
          _buildings = [];
        });
      }
    }
  }

  void _selectCampus(Campus campus) {
    Navigator.pop(context);

    setState(() {
      _selectedCampus = campus;
      _buildings = [];
    });

    _loadBuildings(campus.id);

    _mapController.move(LatLng(campus.latitude, campus.longitude), 16);
  }

  // Campus visibles: los del backend o ejemplo si viene vacío o falla.
  List<Campus> get _visibleCampuses => _campuses.isNotEmpty
      ? _campuses
      : CampusRemoteDataSource.fallbackCampuses;

  @override
  Widget build(BuildContext context) {
    final campus = _selectedCampus;

    final fallbackCenter = LatLng(
      campus?.latitude ?? -38.7359,
      campus?.longitude ?? -72.5904,
    );

    return Stack(
      children: [
        FlutterMap(
          mapController: _mapController,
          options: MapOptions(initialCenter: fallbackCenter, initialZoom: 16),
          children: [
            TileLayer(
              urlTemplate: 'https://tile.openstreetmap.org/{z}/{x}/{y}.png',
              userAgentPackageName: 'cl.cl.uct.uct_map',
            ),

            MarkerLayer(markers: _buildings.map(_buildingMarker).toList()),
          ],
        ),

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
                      campus?.name ?? 'Cargando campus...',
                      style: const TextStyle(fontWeight: FontWeight.w600),
                    ),
                  ),
                  IconButton(
                    icon: const Icon(Icons.keyboard_arrow_down),
                    tooltip: 'Cambiar Campus',
                    onPressed: _campuses.isEmpty
                        ? null
                        : () => _showCampusSelector(context),
                  ),
                ],
              ),
            ),
          ),
        ),

        if (_loading)
          const Positioned(
            top: 80,
            left: 0,
            right: 0,
            child: Center(
              child: Card(
                child: Padding(
                  padding: EdgeInsets.symmetric(horizontal: 16, vertical: 10),
                  child: Text('Cargando campus...'),
                ),
              ),
            ),
          ),

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
                  if (campus != null) {
                    _mapController.move(
                      LatLng(campus.latitude, campus.longitude),
                      16,
                    );
                  }
                },
                tooltip: 'Centrar campus',
                child: const Icon(Icons.my_location),
              ),
            ],
          ),
        ),
      ],
    );
  }

  Marker _buildingMarker(Building building) {
    return Marker(
      point: LatLng(building.latitude, building.longitude),
      width: 50,
      height: 60,
      child: GestureDetector(
        onTap: () => _showBuildingInfo(building),
        child: const Icon(
          Icons.location_on,
          size: 45,
          color: Color(0xFF003865),
        ),
      ),
    );
  }

  void _showBuildingInfo(Building building) {
    showModalBottomSheet(
      context: context,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      builder: (context) {
        return Padding(
          padding: const EdgeInsets.all(20),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                building.name,
                style: const TextStyle(
                  fontSize: 20,
                  fontWeight: FontWeight.bold,
                ),
              ),
              const SizedBox(height: 12),
              Text('${building.floorsCount} pisos'),
              const SizedBox(height: 4),
              Text('${building.rooms.length} salas'),
              const SizedBox(height: 16),
            ],
          ),
        );
      },
    );
  }

  void _showCampusSelector(BuildContext context) {
    showModalBottomSheet(
      context: context,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      builder: (modalContext) {
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
              ..._visibleCampuses.map((campus) {
                final isSelected = campus.id == _selectedCampus?.id;

                return ListTile(
                  leading: Icon(
                    isSelected ? Icons.location_on : Icons.location_on_outlined,
                    color: isSelected ? const Color(0xFF003865) : null,
                  ),
                  title: Text(
                    campus.name,
                    style: TextStyle(
                      fontWeight: isSelected
                          ? FontWeight.bold
                          : FontWeight.normal,
                    ),
                  ),
                  subtitle: Text(campus.address),
                  trailing: isSelected
                      ? const Icon(Icons.check, color: Colors.green)
                      : null,
                  onTap: () => _selectCampus(campus),
                );
              }),
            ],
          ),
        );
      },
    );
  }
}
