import 'package:flutter/material.dart';
import '../../../core/navigation/app_routes.dart';

class SearchScreen extends StatefulWidget {
  final VoidCallback onExploreMap;

  const SearchScreen({
    super.key,
    required this.onExploreMap,
  });

  @override
  State<SearchScreen> createState() => _SearchScreenState();
}

class _SearchScreenState extends State<SearchScreen> {
  final TextEditingController _searchController = TextEditingController();

  // Lista de profesores destacados de ejemplo
  final List<Map<String, String>> _featuredProfessors = [
    {
      'name': 'Dr. Juan Pérez',
      'faculty': 'Facultad de Ingeniería',
      'classTime': '10:00 - 11:30',
      'room': 'Sala C-302',
    },
    {
      'name': 'Dra. María González',
      'faculty': 'Facultad de Ciencias Jurídicas',
      'classTime': '11:45 - 13:15',
      'room': 'Sala A-105',
    },
    {
      'name': 'Mg. Carlos Soto',
      'faculty': 'Facultad de Educación',
      'classTime': '14:30 - 16:00',
      'room': 'Laboratorio C-101',
    },
  ];

  @override
  void dispose() {
    _searchController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final query = _searchController.text.trim().toLowerCase();

    // Filtramos profesores por la búsqueda si escribe algo
    final filteredProfessors = _featuredProfessors.where((prof) {
      return prof['name']!.toLowerCase().contains(query) ||
          prof['faculty']!.toLowerCase().contains(query) ||
          prof['room']!.toLowerCase().contains(query);
    }).toList();

    // Las 4 categorías principales
    final categories = [
      {'name': 'Edificios', 'icon': Icons.apartment_rounded, 'color': Colors.indigo},
      {'name': 'Salas', 'icon': Icons.meeting_room_rounded, 'color': Colors.teal},
      {'name': 'Profesores', 'icon': Icons.school_rounded, 'color': Colors.blue},
      {'name': 'Servicios', 'icon': Icons.miscellaneous_services_rounded, 'color': Colors.amber.shade800},
    ];

    // Si el usuario escribe algo y no coincide ni con las categorías ni con profesores
    final matchesCategory = categories.any((cat) => (cat['name'] as String).toLowerCase().contains(query));
    final isNotFound = query.isNotEmpty && filteredProfessors.isEmpty && !matchesCategory;

    return SingleChildScrollView(
      padding: const EdgeInsets.symmetric(horizontal: 16.0, vertical: 12.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // 1. Buscador arriba de la pantalla
          Card(
            elevation: 2,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
            child: Padding(
              padding: const EdgeInsets.symmetric(horizontal: 6.0, vertical: 2.0),
              child: Row(
                children: [
                  IconButton(
                    icon: const Icon(Icons.menu, color: Color(0xFF003865)),
                    tooltip: 'Categorías',
                    onPressed: () {
                      Scaffold.of(context).openDrawer();
                    },
                  ),
                  Expanded(
                    child: TextField(
                      controller: _searchController,
                      decoration: const InputDecoration(
                        hintText: 'Buscar edificio, sala, profesor...',
                        border: InputBorder.none,
                        hintStyle: TextStyle(color: Colors.grey, fontSize: 14),
                      ),
                      onChanged: (val) {
                        setState(() {});
                      },
                    ),
                  ),
                  if (_searchController.text.isNotEmpty)
                    IconButton(
                      icon: const Icon(Icons.clear, size: 20, color: Colors.grey),
                      onPressed: () {
                        setState(() {
                          _searchController.clear();
                        });
                      },
                    )
                  else
                    const Padding(
                      padding: EdgeInsets.only(right: 12.0),
                      child: Icon(Icons.search, color: Colors.grey),
                    ),
                ],
              ),
            ),
          ),
          const SizedBox(height: 18),

          // 2. Si no se encuentra lo buscado, mostramos la pantalla de "No encontramos lo que buscas"
          if (isNotFound)
            _buildNotFoundView()
          else ...[
            // 3. Cuadrícula de 4 Categorías en 2x2
            const Text(
              'Categorías',
              style: TextStyle(
                fontSize: 18,
                fontWeight: FontWeight.bold,
                color: Color(0xFF1E293B),
              ),
            ),
            const SizedBox(height: 12),
            GridView.builder(
              shrinkWrap: true,
              physics: const NeverScrollableScrollPhysics(),
              itemCount: categories.length,
              gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                crossAxisCount: 2,
                crossAxisSpacing: 12,
                mainAxisSpacing: 12,
                childAspectRatio: 2.2,
              ),
              itemBuilder: (context, index) {
                final cat = categories[index];
                final color = cat['color'] as Color;
                return Card(
                  elevation: 1.5,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(14),
                    side: BorderSide(color: Colors.grey.shade200),
                  ),
                  child: InkWell(
                    borderRadius: BorderRadius.circular(14),
                    onTap: () {
                      _searchController.text = cat['name'] as String;
                      setState(() {});
                    },
                    child: Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 12.0, vertical: 8.0),
                      child: Row(
                        children: [
                          CircleAvatar(
                            radius: 20,
                            backgroundColor: color.withValues(alpha: 0.12),
                            child: Icon(cat['icon'] as IconData, color: color, size: 22),
                          ),
                          const SizedBox(width: 10),
                          Expanded(
                            child: Text(
                              cat['name'] as String,
                              style: const TextStyle(
                                fontWeight: FontWeight.w600,
                                fontSize: 14,
                                color: Color(0xFF1E293B),
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                );
              },
            ),
            const SizedBox(height: 24),

            // 4. Profesores Destacados
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                const Text(
                  'Profesores Destacados',
                  style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                    color: Color(0xFF1E293B),
                  ),
                ),
                TextButton(
                  onPressed: () async {
                    final res = await Navigator.pushNamed(
                      context,
                      AppRoutes.professors,
                    );
                    if (res == 0 && mounted) {
                      widget.onExploreMap();
                    }
                  },
                  child: const Text('Ver todos'),
                ),
              ],
            ),
            const SizedBox(height: 8),

            // Lista de tarjetas de profesores
            ListView.builder(
              shrinkWrap: true,
              physics: const NeverScrollableScrollPhysics(),
              itemCount: filteredProfessors.length,
              itemBuilder: (context, index) {
                final prof = filteredProfessors[index];
                return Card(
                  elevation: 2,
                  margin: const EdgeInsets.only(bottom: 14),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(16),
                    side: BorderSide(color: Colors.grey.shade200),
                  ),
                  child: Padding(
                    padding: const EdgeInsets.all(16.0),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Row(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            CircleAvatar(
                              radius: 24,
                              backgroundColor: const Color(0xFF003865).withValues(alpha: 0.1),
                              child: const Icon(
                                Icons.person,
                                color: Color(0xFF003865),
                                size: 28,
                              ),
                            ),
                            const SizedBox(width: 12),
                            Expanded(
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    prof['name']!,
                                    style: const TextStyle(
                                      fontWeight: FontWeight.bold,
                                      fontSize: 16,
                                      color: Color(0xFF003865),
                                    ),
                                  ),
                                  const SizedBox(height: 2),
                                  Text(
                                    prof['faculty']!,
                                    style: TextStyle(
                                      color: Colors.grey.shade600,
                                      fontSize: 13,
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ],
                        ),
                        const Divider(height: 20),

                        // Horario y Sala de clase
                        Container(
                          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
                          decoration: BoxDecoration(
                            color: const Color(0xFFF1F5F9),
                            borderRadius: BorderRadius.circular(10),
                          ),
                          child: Row(
                            children: [
                              const Icon(Icons.schedule, size: 18, color: Color(0xFF003865)),
                              const SizedBox(width: 6),
                              Text(
                                prof['classTime']!,
                                style: const TextStyle(
                                  fontWeight: FontWeight.w600,
                                  fontSize: 13,
                                ),
                              ),
                              const SizedBox(width: 12),
                              const Text('•', style: TextStyle(color: Colors.grey)),
                              const SizedBox(width: 12),
                              const Icon(Icons.meeting_room, size: 18, color: Color(0xFFEAA221)),
                              const SizedBox(width: 6),
                              Text(
                                prof['room']!,
                                style: const TextStyle(
                                  fontWeight: FontWeight.w600,
                                  fontSize: 13,
                                  color: Color(0xFF1E293B),
                                ),
                              ),
                            ],
                          ),
                        ),
                        const SizedBox(height: 14),

                        // Botón "Cómo llegar"
                        SizedBox(
                          width: double.infinity,
                          child: ElevatedButton.icon(
                            onPressed: () {
                              ScaffoldMessenger.of(context).showSnackBar(
                                SnackBar(
                                  content: Text('Trazando ruta a ${prof['room']} (${prof['name']})...'),
                                  duration: const Duration(seconds: 2),
                                ),
                              );
                              // Redirige al mapa
                              widget.onExploreMap();
                            },
                            style: ElevatedButton.styleFrom(
                              backgroundColor: const Color(0xFF003865), // Azul UCT
                              foregroundColor: Colors.white,
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(10),
                              ),
                              padding: const EdgeInsets.symmetric(vertical: 12),
                            ),
                            icon: const Icon(Icons.directions_outlined, size: 20),
                            label: const Text(
                              'Cómo llegar',
                              style: TextStyle(
                                fontWeight: FontWeight.bold,
                                fontSize: 14,
                              ),
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                );
              },
            ),
          ],
        ],
      ),
    );
  }

  // Vista de búsqueda vacía / no encontrada
  Widget _buildNotFoundView() {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 24.0, horizontal: 8.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          // Lupa gigante
          Container(
            padding: const EdgeInsets.all(22),
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              color: const Color(0xFF003865).withValues(alpha: 0.06),
            ),
            child: const Icon(
              Icons.search_off_rounded,
              size: 88,
              color: Color(0xFF003865),
            ),
          ),
          const SizedBox(height: 20),

          const Text(
            'No encontramos lo que buscas',
            textAlign: TextAlign.center,
            style: TextStyle(
              fontSize: 19,
              fontWeight: FontWeight.bold,
              color: Color(0xFF1E293B),
            ),
          ),
          const SizedBox(height: 6),

          Text(
            'Revisa los términos de búsqueda e intenta de nuevo.',
            textAlign: TextAlign.center,
            style: TextStyle(
              fontSize: 14,
              color: Colors.grey.shade600,
              height: 1.4,
            ),
          ),
          const SizedBox(height: 24),

          // Recuadro de sugerencias
          Container(
            width: double.infinity,
            padding: const EdgeInsets.all(16.0),
            decoration: BoxDecoration(
              color: const Color(0xFFF8FAFC),
              borderRadius: BorderRadius.circular(16),
              border: Border.all(color: const Color(0xFFE2E8F0), width: 1.2),
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: const [
                    Icon(Icons.lightbulb_outline, size: 20, color: Color(0xFFEAA221)),
                    SizedBox(width: 8),
                    Text(
                      'Sugerencias',
                      style: TextStyle(
                        fontSize: 15,
                        fontWeight: FontWeight.bold,
                        color: Color(0xFF1E293B),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 10),
                _buildSuggestionItem('Verifica la ortografía de búsqueda'),
                const SizedBox(height: 6),
                _buildSuggestionItem('Prueba con términos más generales'),
              ],
            ),
          ),
          const SizedBox(height: 28),

          // Botón azul: Explorar el mapa
          SizedBox(
            width: double.infinity,
            height: 48,
            child: ElevatedButton.icon(
              onPressed: () {
                _searchController.clear();
                setState(() {});
                widget.onExploreMap();
              },
              style: ElevatedButton.styleFrom(
                backgroundColor: const Color(0xFF003865),
                foregroundColor: Colors.white,
                elevation: 2,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12),
                ),
              ),
              icon: const Icon(Icons.map_outlined, size: 20),
              label: const Text(
                'Explorar el mapa',
                style: TextStyle(
                  fontSize: 15,
                  fontWeight: FontWeight.w600,
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSuggestionItem(String text) {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text(
          '• ',
          style: TextStyle(
            fontSize: 14,
            fontWeight: FontWeight.bold,
            color: Color(0xFF64748B),
          ),
        ),
        Expanded(
          child: Text(
            text,
            style: const TextStyle(
              fontSize: 13.5,
              color: Color(0xFF475569),
              height: 1.3,
            ),
          ),
        ),
      ],
    );
  }
}
