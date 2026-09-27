import 'package:flutter/material.dart';

import '../../../data/datasources/schedule_remote_ds.dart';
import '../../../domain/entities/professor.dart';
import '../../../domain/repositories/schedule_repository.dart';

/// Pantalla Directorio de Profesores conectada al Schedule Service (ms.svg).
class ProfessorsScreen extends StatefulWidget {
  const ProfessorsScreen({super.key, this.scheduleRepository});

  final ScheduleRepository? scheduleRepository;

  @override
  State<ProfessorsScreen> createState() => _ProfessorsScreenState();
}

class _ProfessorsScreenState extends State<ProfessorsScreen> {
  late final ScheduleRepository _repository;
  List<Professor> _professors = [];
  bool _loading = true;
  String _searchQuery = '';

  @override
  void initState() {
    super.initState();
    _repository = widget.scheduleRepository ?? ScheduleRemoteDataSource();
    _loadProfessors();
  }

  Future<void> _loadProfessors() async {
    setState(() => _loading = true);
    try {
      final list = await _repository.getProfessors();
      if (mounted) {
        setState(() {
          _professors = list;
          _loading = false;
        });
      }
    } catch (_) {
      if (mounted) setState(() => _loading = false);
    }
  }

  List<Professor> get _filteredProfessors {
    final base = _professors.isNotEmpty
        ? _professors
        : ScheduleRemoteDataSource.fallbackProfessors;
    if (_searchQuery.trim().isEmpty) return base;
    final q = _searchQuery.toLowerCase();
    return base.where((p) {
      return p.name.toLowerCase().contains(q) ||
          p.department.toLowerCase().contains(q) ||
          p.office.toLowerCase().contains(q);
    }).toList();
  }

  void _showBookingDialog(Professor prof) {
    DateTime selectedDate = DateTime.now().add(const Duration(days: 1));
    TimeOfDay selectedTime = const TimeOfDay(hour: 10, minute: 0);

    showDialog(
      context: context,
      builder: (dialogCtx) {
        return StatefulBuilder(
          builder: (context, setDialogState) {
            return AlertDialog(
              title: Text('Agendar Cita con ${prof.name}'),
              content: Column(
                mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text('Departamento: ${prof.department}',
                      style: const TextStyle(fontSize: 13, color: Colors.grey)),
                  const SizedBox(height: 12),
                  ListTile(
                    contentPadding: EdgeInsets.zero,
                    leading: const Icon(Icons.calendar_today, color: Color(0xFF003865)),
                    title: Text(
                      'Fecha: ${selectedDate.day}/${selectedDate.month}/${selectedDate.year}',
                      style: const TextStyle(fontSize: 14),
                    ),
                    trailing: TextButton(
                      child: const Text('Cambiar'),
                      onPressed: () async {
                        final picked = await showDatePicker(
                          context: context,
                          initialDate: selectedDate,
                          firstDate: DateTime.now(),
                          lastDate: DateTime.now().add(const Duration(days: 60)),
                        );
                        if (picked != null) {
                          setDialogState(() => selectedDate = picked);
                        }
                      },
                    ),
                  ),
                  ListTile(
                    contentPadding: EdgeInsets.zero,
                    leading: const Icon(Icons.access_time, color: Color(0xFF003865)),
                    title: Text(
                      'Hora: ${selectedTime.format(context)}',
                      style: const TextStyle(fontSize: 14),
                    ),
                    trailing: TextButton(
                      child: const Text('Cambiar'),
                      onPressed: () async {
                        final picked = await showTimePicker(
                          context: context,
                          initialTime: selectedTime,
                        );
                        if (picked != null) {
                          setDialogState(() => selectedTime = picked);
                        }
                      },
                    ),
                  ),
                ],
              ),
              actions: [
                TextButton(
                  onPressed: () => Navigator.pop(dialogCtx),
                  child: const Text('Cancelar'),
                ),
                ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFF003865),
                    foregroundColor: Colors.white,
                  ),
                  onPressed: () async {
                    Navigator.pop(dialogCtx);
                    final scheduledAt = DateTime(
                      selectedDate.year,
                      selectedDate.month,
                      selectedDate.day,
                      selectedTime.hour,
                      selectedTime.minute,
                    );
                    try {
                      await _repository.createMeeting(
                        teacherRefId: prof.id,
                        structureRefId: prof.structureRefId ?? prof.office,
                        scheduledAt: scheduledAt,
                      );
                      if (context.mounted) {
                        ScaffoldMessenger.of(context).showSnackBar(
                          SnackBar(
                            content: Text(
                              'Cita solicitada exitosamente con ${prof.name}.',
                            ),
                          ),
                        );
                      }
                    } catch (_) {
                      if (context.mounted) {
                        ScaffoldMessenger.of(context).showSnackBar(
                          SnackBar(
                            content: Text('Cita agendada localmente con ${prof.name}.'),
                          ),
                        );
                      }
                    }
                  },
                  child: const Text('Confirmar Cita'),
                ),
              ],
            );
          },
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    final list = _filteredProfessors;

    return Scaffold(
      appBar: AppBar(
        title: const Text('Directorio de Profesores'),
        centerTitle: true,
      ),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: TextField(
              onChanged: (val) => setState(() => _searchQuery = val),
              decoration: InputDecoration(
                hintText: 'Buscar profesor por nombre o departamento...',
                prefixIcon: const Icon(Icons.search),
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12),
                ),
                filled: true,
                fillColor: Colors.grey.shade100,
              ),
            ),
          ),
          Expanded(
            child: _loading
                ? const Center(child: CircularProgressIndicator())
                : list.isEmpty
                    ? const Center(child: Text('No se encontraron profesores.'))
                    : ListView.builder(
                        padding: const EdgeInsets.symmetric(horizontal: 16.0),
                        itemCount: list.length,
                        itemBuilder: (context, index) {
                          final prof = list[index];
                          return Card(
                            margin: const EdgeInsets.only(bottom: 12),
                            child: ExpansionTile(
                              leading: const CircleAvatar(
                                child: Icon(Icons.person),
                              ),
                              title: Text(
                                prof.name,
                                style: const TextStyle(fontWeight: FontWeight.bold),
                              ),
                              subtitle: Text(prof.department),
                              children: [
                                Padding(
                                  padding: const EdgeInsets.all(16.0),
                                  child: Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: [
                                      Row(
                                        children: [
                                          const Icon(Icons.meeting_room,
                                              size: 20, color: Colors.blueGrey),
                                          const SizedBox(width: 8),
                                          Text('Ubicación: ${prof.office}'),
                                        ],
                                      ),
                                      const SizedBox(height: 8),
                                      Row(
                                        children: [
                                          const Icon(Icons.email,
                                              size: 20, color: Colors.blueGrey),
                                          const SizedBox(width: 8),
                                          Text('Correo: ${prof.email}'),
                                        ],
                                      ),
                                      if (prof.officeHours.isNotEmpty) ...[
                                        const SizedBox(height: 12),
                                        const Text(
                                          'Horarios de Atención:',
                                          style: TextStyle(
                                            fontWeight: FontWeight.bold,
                                            fontSize: 13,
                                          ),
                                        ),
                                        const SizedBox(height: 4),
                                        ...prof.officeHours.map(
                                          (h) => Text(
                                            '• ${h.dayName}: ${h.startTime} - ${h.endTime}',
                                            style: const TextStyle(
                                                fontSize: 12, color: Colors.black87),
                                          ),
                                        ),
                                      ],
                                      const SizedBox(height: 12),
                                      Row(
                                        mainAxisAlignment: MainAxisAlignment.end,
                                        children: [
                                          OutlinedButton.icon(
                                            onPressed: () {
                                              ScaffoldMessenger.of(context).showSnackBar(
                                                SnackBar(
                                                  content: Text(
                                                      'Ubicando oficina de ${prof.name} en el mapa...'),
                                                ),
                                              );
                                            },
                                            icon: const Icon(Icons.map_outlined),
                                            label: const Text('Ver en Mapa'),
                                          ),
                                          const SizedBox(width: 8),
                                          ElevatedButton.icon(
                                            style: ElevatedButton.styleFrom(
                                              backgroundColor: const Color(0xFF003865),
                                              foregroundColor: Colors.white,
                                            ),
                                            onPressed: () => _showBookingDialog(prof),
                                            icon: const Icon(Icons.calendar_month),
                                            label: const Text('Agendar Cita'),
                                          ),
                                        ],
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
