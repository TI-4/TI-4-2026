import 'package:flutter/material.dart';
import '../../../core/navigation/app_routes.dart';
import '../../../core/theme/app_colors.dart';

class ProfessorsScreen extends StatelessWidget {
  final Function(int)? onNavigateToTab;

  const ProfessorsScreen({super.key, this.onNavigateToTab});

  final List<Map<String, String>> _professors = const [
    {
      'name': 'Dr. Roberto González',
      'department': 'Ingeniería de Software',
      'office': 'Oficina 304 - Edificio Central',
      'email': 'rgonzalez@uct.cl',
    },
    {
      'name': 'Dra. Marcela Soto',
      'department': 'Ciencias de la Computación',
      'office': 'Oficina 210 - Edificio C',
      'email': 'msoto@uct.cl',
    },
    {
      'name': 'Mg. Carlos Peña',
      'department': 'Redes y Telecomunicaciones',
      'office': 'Oficina 105 - Laboratorios',
      'email': 'cpena@uct.cl',
    },
  ];

  void _goToMap(BuildContext context, String profName) {
    ScaffoldMessenger.of(context).clearSnackBars();
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Ubicando oficina de $profName en el mapa...'),
        duration: const Duration(seconds: 2),
      ),
    );

    if (onNavigateToTab != null) {
      if (Navigator.canPop(context)) {
        Navigator.pop(context, 0);
      }
      onNavigateToTab!(0);
    } else if (Navigator.canPop(context)) {
      Navigator.pop(context, 0);
    } else {
      Navigator.pushReplacementNamed(
        context,
        AppRoutes.initial,
        arguments: 0,
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Directorio de Profesores',
          style: TextStyle(
            fontSize: 18,
            fontWeight: FontWeight.bold,
            color: Colors.white,
          ),
        ),
        backgroundColor: AppColors.uctBlue,
        foregroundColor: Colors.white,
        centerTitle: true,
      ),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: TextField(
              decoration: InputDecoration(
                hintText: 'Buscar profesor por nombre o departamento...',
                prefixIcon: const Icon(Icons.search, color: AppColors.uctBlue),
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12),
                  borderSide: const BorderSide(color: AppColors.fieldBorder),
                ),
                enabledBorder: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12),
                  borderSide: const BorderSide(color: AppColors.fieldBorder),
                ),
                focusedBorder: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12),
                  borderSide:
                      const BorderSide(color: AppColors.uctBlue, width: 2),
                ),
                filled: true,
                fillColor: Colors.grey.shade50,
              ),
            ),
          ),
          Expanded(
            child: ListView.builder(
              padding: const EdgeInsets.symmetric(horizontal: 16.0),
              itemCount: _professors.length,
              itemBuilder: (context, index) {
                final prof = _professors[index];
                return Card(
                  margin: const EdgeInsets.only(bottom: 12),
                  elevation: 1,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(14),
                    side: const BorderSide(color: AppColors.fieldBorder),
                  ),
                  child: ExpansionTile(
                    leading: CircleAvatar(
                      backgroundColor:
                          AppColors.uctBlue.withValues(alpha: 0.1),
                      child: const Icon(Icons.person, color: AppColors.uctBlue),
                    ),
                    title: Text(
                      prof['name']!,
                      style: const TextStyle(
                        fontWeight: FontWeight.bold,
                        color: AppColors.ink,
                      ),
                    ),
                    subtitle: Text(
                      prof['department']!,
                      style: const TextStyle(
                        color: AppColors.subtitle,
                        fontSize: 13,
                      ),
                    ),
                    children: [
                      Padding(
                        padding: const EdgeInsets.all(16.0),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Row(
                              children: [
                                const Icon(Icons.meeting_room,
                                    size: 20, color: AppColors.uctGold),
                                const SizedBox(width: 8),
                                Expanded(
                                  child: Text(
                                    'Ubicación: ${prof['office']}',
                                    style: const TextStyle(
                                      color: AppColors.ink,
                                      fontSize: 13,
                                    ),
                                  ),
                                ),
                              ],
                            ),
                            const SizedBox(height: 8),
                            Row(
                              children: [
                                const Icon(Icons.email,
                                    size: 20, color: AppColors.uctBlue),
                                const SizedBox(width: 8),
                                Expanded(
                                  child: Text(
                                    'Correo: ${prof['email']}',
                                    style: const TextStyle(
                                      color: AppColors.ink,
                                      fontSize: 13,
                                    ),
                                  ),
                                ),
                              ],
                            ),
                            const SizedBox(height: 14),
                            Align(
                              alignment: Alignment.centerRight,
                              child: ElevatedButton.icon(
                                onPressed: () =>
                                    _goToMap(context, prof['name']!),
                                style: ElevatedButton.styleFrom(
                                  backgroundColor: AppColors.uctBlue,
                                  foregroundColor: Colors.white,
                                  shape: RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(10),
                                  ),
                                  padding: const EdgeInsets.symmetric(
                                    horizontal: 16,
                                    vertical: 10,
                                  ),
                                ),
                                icon: const Icon(Icons.map_outlined, size: 18),
                                label: const Text(
                                  'Ver en Mapa',
                                  style: TextStyle(fontWeight: FontWeight.bold),
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}
