import 'package:flutter/material.dart';
import '../../../core/theme/app_colors.dart';

class SavedPlaceItem {
  final String id;
  final String title;
  final String campus;
  final String building;
  final String category;
  final String schedule;
  final IconData icon;
  final Color color;

  const SavedPlaceItem({
    required this.id,
    required this.title,
    required this.campus,
    required this.building,
    required this.category,
    required this.schedule,
    required this.icon,
    required this.color,
  });
}

class SavedPlacesScreen extends StatefulWidget {
  final Function(int)? onNavigateToTab;

  const SavedPlacesScreen({super.key, this.onNavigateToTab});

  @override
  State<SavedPlacesScreen> createState() => _SavedPlacesScreenState();
}

class _SavedPlacesScreenState extends State<SavedPlacesScreen> {
  final List<SavedPlaceItem> _places = [
    const SavedPlaceItem(
      id: '1',
      title: 'Biblioteca Central San Francisco',
      campus: 'Campus San Francisco',
      building: 'Edificio B, Piso 2',
      category: 'Estudio y Libros',
      schedule: '08:30 - 20:00 hrs',
      icon: Icons.local_library_rounded,
      color: AppColors.uctBlue,
    ),
    const SavedPlaceItem(
      id: '2',
      title: 'Laboratorio de Computación L-204',
      campus: 'Campus San Juan Pablo II',
      building: 'Edificio C, 2do Piso',
      category: 'Laboratorio TI',
      schedule: '08:00 - 18:30 hrs',
      icon: Icons.computer_rounded,
      color: Color(0xFF0284C7),
    ),
    const SavedPlaceItem(
      id: '3',
      title: 'Casino y Comedor Universitario',
      campus: 'Campus San Juan Pablo II',
      building: 'Edificio Central',
      category: 'Alimentación',
      schedule: '11:30 - 16:00 hrs',
      icon: Icons.restaurant_rounded,
      color: Color(0xFFEA580C),
    ),
    const SavedPlaceItem(
      id: '4',
      title: 'Gimnasio y Complejo Deportivo UCT',
      campus: 'Campus San Francisco',
      building: 'Sector Deportivo',
      category: 'Deportes',
      schedule: '09:00 - 21:00 hrs',
      icon: Icons.fitness_center_rounded,
      color: Color(0xFF16A34A),
    ),
  ];

  String _filterCampus = 'Todos';

  List<SavedPlaceItem> get _filteredPlaces {
    if (_filterCampus == 'Todos') return _places;
    return _places.where((p) => p.campus == _filterCampus).toList();
  }

  void _removePlace(SavedPlaceItem place) {
    final index = _places.indexOf(place);
    setState(() {
      _places.removeAt(index);
    });

    ScaffoldMessenger.of(context).clearSnackBars();
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Eliminado de favoritos: ${place.title}'),
        action: SnackBarAction(
          label: 'Deshacer',
          textColor: AppColors.uctYellow,
          onPressed: () {
            setState(() {
              _places.insert(index, place);
            });
          },
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final places = _filteredPlaces;

    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Mis Favoritos y Guardados',
          style: TextStyle(fontWeight: FontWeight.bold, fontSize: 18),
        ),
        backgroundColor: AppColors.uctBlue,
        foregroundColor: Colors.white,
      ),
      body: Column(
        children: [
          // Banner de contexto
          Container(
            width: double.infinity,
            padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 14),
            decoration: BoxDecoration(
              color: AppColors.uctBlue.withValues(alpha: 0.05),
              border: const Border(
                bottom: BorderSide(color: AppColors.fieldBorder),
              ),
            ),
            child: Row(
              children: [
                Container(
                  padding: const EdgeInsets.all(8),
                  decoration: BoxDecoration(
                    color: AppColors.uctGold.withValues(alpha: 0.2),
                    shape: BoxShape.circle,
                  ),
                  child: const Icon(
                    Icons.bookmark_added_rounded,
                    color: AppColors.uctBlue,
                    size: 20,
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        '${_places.length} lugares guardados',
                        style: const TextStyle(
                          fontSize: 15,
                          fontWeight: FontWeight.bold,
                          color: AppColors.ink,
                        ),
                      ),
                      const SizedBox(height: 2),
                      const Text(
                        'Accede rápidamente a tus salas y espacios de estudio preferidos.',
                        style: TextStyle(
                          fontSize: 12,
                          color: AppColors.subtitle,
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),

          // Filtro por campus
          SingleChildScrollView(
            scrollDirection: Axis.horizontal,
            padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
            child: Row(
              children: [
                _filterChip('Todos'),
                const SizedBox(width: 8),
                _filterChip('Campus San Francisco'),
                const SizedBox(width: 8),
                _filterChip('Campus San Juan Pablo II'),
              ],
            ),
          ),

          // Lista de lugares guardados
          Expanded(
            child: places.isEmpty
                ? Center(
                    child: Padding(
                      padding: const EdgeInsets.all(32.0),
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(
                            Icons.bookmark_border_rounded,
                            size: 64,
                            color: Colors.grey.shade400,
                          ),
                          const SizedBox(height: 16),
                          const Text(
                            'No tienes lugares guardados aquí',
                            style: TextStyle(
                              fontSize: 16,
                              fontWeight: FontWeight.w600,
                              color: AppColors.subtitle,
                            ),
                          ),
                          const SizedBox(height: 8),
                          const Text(
                            'Puedes marcar lugares como favoritos explorando el mapa o usando el buscador.',
                            textAlign: TextAlign.center,
                            style: TextStyle(
                              fontSize: 13,
                              color: AppColors.hint,
                            ),
                          ),
                        ],
                      ),
                    ),
                  )
                : ListView.separated(
                    padding: const EdgeInsets.fromLTRB(16, 8, 16, 24),
                    itemCount: places.length,
                    separatorBuilder: (context, index) => const SizedBox(height: 12),
                    itemBuilder: (context, index) {
                      final item = places[index];
                      return _buildPlaceCard(item);
                    },
                  ),
          ),
        ],
      ),
    );
  }

  Widget _filterChip(String label) {
    final selected = _filterCampus == label;
    return FilterChip(
      label: Text(label),
      selected: selected,
      labelStyle: TextStyle(
        fontSize: 12,
        fontWeight: selected ? FontWeight.bold : FontWeight.normal,
        color: selected ? Colors.white : AppColors.ink,
      ),
      backgroundColor: Colors.grey.shade100,
      selectedColor: AppColors.uctBlue,
      showCheckmark: false,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(20),
        side: BorderSide(
          color: selected ? AppColors.uctBlue : AppColors.fieldBorder,
        ),
      ),
      onSelected: (_) {
        setState(() {
          _filterCampus = label;
        });
      },
    );
  }

  Widget _buildPlaceCard(SavedPlaceItem item) {
    return Card(
      elevation: 0,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(16),
        side: const BorderSide(color: AppColors.fieldBorder),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Container(
                  width: 44,
                  height: 44,
                  decoration: BoxDecoration(
                    color: item.color.withValues(alpha: 0.12),
                    borderRadius: BorderRadius.circular(12),
                  ),
                  child: Icon(item.icon, color: item.color, size: 24),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        item.title,
                        style: const TextStyle(
                          fontSize: 15,
                          fontWeight: FontWeight.bold,
                          color: AppColors.ink,
                        ),
                      ),
                      const SizedBox(height: 4),
                      Row(
                        children: [
                          const Icon(
                            Icons.location_on_outlined,
                            size: 14,
                            color: AppColors.subtitle,
                          ),
                          const SizedBox(width: 4),
                          Expanded(
                            child: Text(
                              '${item.campus} • ${item.building}',
                              style: const TextStyle(
                                fontSize: 12,
                                color: AppColors.subtitle,
                              ),
                              overflow: TextOverflow.ellipsis,
                            ),
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
                IconButton(
                  icon: const Icon(Icons.bookmark, color: AppColors.uctGold),
                  tooltip: 'Quitar de guardados',
                  onPressed: () => _removePlace(item),
                ),
              ],
            ),
            const SizedBox(height: 12),
            const Divider(height: 1, color: AppColors.fieldBorder),
            const SizedBox(height: 10),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Container(
                  padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                  decoration: BoxDecoration(
                    color: Colors.grey.shade100,
                    borderRadius: BorderRadius.circular(8),
                  ),
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      const Icon(Icons.schedule, size: 13, color: AppColors.subtitle),
                      const SizedBox(width: 5),
                      Text(
                        item.schedule,
                        style: const TextStyle(
                          fontSize: 11,
                          color: AppColors.subtitle,
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                    ],
                  ),
                ),
                OutlinedButton.icon(
                  onPressed: () {
                    Navigator.pop(context);
                    if (widget.onNavigateToTab != null) {
                      widget.onNavigateToTab!(0); // Navega al mapa
                    }
                    ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(
                        content: Text('Ubicando "${item.title}" en el mapa...'),
                        duration: const Duration(seconds: 2),
                      ),
                    );
                  },
                  icon: const Icon(Icons.map_outlined, size: 16),
                  label: const Text('Ver en Mapa', style: TextStyle(fontSize: 12)),
                  style: OutlinedButton.styleFrom(
                    foregroundColor: AppColors.uctBlue,
                    side: const BorderSide(color: AppColors.uctBlue),
                    padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
                    visualDensity: VisualDensity.compact,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(10),
                    ),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
