import 'package:flutter/material.dart';
import '../screens/map/map_screen.dart';
import '../screens/search/search_screen.dart';
import '../screens/lost_found/lost_found_screen.dart';
import '../screens/reports/reports_screen.dart';
import '../screens/profile/profile_screen.dart';
import '../screens/login/auth_required_view.dart';
import '../../application/session/session_controller.dart';
import '../widgets/uct_logo.dart';
import '../widgets/custom_drawer.dart';

class MainNavigationScreen extends StatefulWidget {
  const MainNavigationScreen({super.key});

  @override
  State<MainNavigationScreen> createState() => _MainNavigationScreenState();
}

class _MainNavigationScreenState extends State<MainNavigationScreen> {
  int _currentIndex = 0;
  final _session = SessionController();

  static const _protectedTabs = [2, 3];

  final List<String> _titles = const [
    'Mapa del Campus',
    'Búsqueda',
    'Objetos Perdidos',
    'Reportes de Incidencias',
    'Mi Perfil',
  ];

  void _onTabTapped(int index) {
    setState(() {
      _currentIndex = index;
    });
  }

  @override
  void dispose() {
    _session.dispose();
    super.dispose();
  }

  Future<void> _openLogin(String section) async {
    final result =
        await Navigator.pushNamed(context, '/login', arguments: section);
    if (result is Map && result['ok'] == true && mounted) {
      // TODO(tarea 3): sesión real desde el endpoint de auth.
      _session.signInDemo((result['email'] ?? '').toString());
      setState(() {});
    }
  }

  Widget _protected(int index, Widget real, String section) {
    if (_session.isAuthenticated || !_protectedTabs.contains(index)) {
      return real;
    }
    return AuthRequiredView(onLogin: () => _openLogin(section));
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    final isSearchTab = _currentIndex == 1;

    final List<Widget> screens = [
      const MapScreen(),
      SearchScreen(
        onExploreMap: () => _onTabTapped(0),
      ),
      _protected(2, const LostFoundScreen(), 'Objetos perdidos'),
      _protected(3, const ReportsScreen(), 'Reportes de incidencias'),
      ProfileScreen(
        session: _session,
        onNavigateToTab: _onTabTapped,
        onLogout: () {
          _session.signOut();
          setState(() {});
        },
      ),
    ];

    return Scaffold(
      appBar: AppBar(
        backgroundColor: theme.colorScheme.primary,
        foregroundColor: Colors.white,
        // En la pantalla de buscar sale solo el botón hamburguesa a la izquierda para categorías
        leading: isSearchTab
            ? Builder(
                builder: (context) {
                  return IconButton(
                    icon: const Icon(Icons.menu, color: Colors.white),
                    tooltip: 'Categorías',
                    onPressed: () {
                      Scaffold.of(context).openDrawer();
                    },
                  );
                },
              )
            : Builder(
                builder: (context) {
                  return InkWell(
                    onTap: () {
                      Scaffold.of(context).openDrawer();
                    },
                    borderRadius: BorderRadius.circular(8),
                    child: const Padding(
                      padding: EdgeInsets.all(8.0),
                      child: UctLogo(height: 36, width: 36),
                    ),
                  );
                },
              ),
        title: Text(
          _titles[_currentIndex],
          style: const TextStyle(
            fontSize: 18,
            fontWeight: FontWeight.bold,
            color: Colors.white,
          ),
        ),
        centerTitle: true,
        // En la pantalla de buscar no sale el avatar de la derecha
        actions: isSearchTab
            ? null
            : [
                Padding(
                  padding: const EdgeInsets.only(right: 12.0),
                  child: GestureDetector(
                    onTap: () => _onTabTapped(4), // Ir al perfil
                    child: Tooltip(
                      message: 'Ver Perfil del Estudiante',
                      child: Container(
                        decoration: BoxDecoration(
                          shape: BoxShape.circle,
                          border: Border.all(
                            color: _currentIndex == 4
                                ? theme.colorScheme.secondary
                                : Colors.white.withValues(alpha: 0.8),
                            width: 2,
                          ),
                        ),
                        child: const CircleAvatar(
                          radius: 17,
                          backgroundColor: Colors.white,
                          child: Icon(
                            Icons.person,
                            color: Color(0xFF003865),
                            size: 22,
                          ),
                        ),
                      ),
                    ),
                  ),
                ),
              ],
      ),
      drawer: CustomDrawer(
        currentIndex: _currentIndex,
        onSelectTab: _onTabTapped,
      ),
      body: IndexedStack(
        index: _currentIndex,
        children: screens,
      ),
      bottomNavigationBar: NavigationBar(
        selectedIndex: _currentIndex,
        onDestinationSelected: _onTabTapped,
        destinations: const [
          NavigationDestination(
            icon: Icon(Icons.map_outlined),
            selectedIcon: Icon(Icons.map),
            label: 'Mapa',
          ),
          NavigationDestination(
            icon: Icon(Icons.search_outlined),
            selectedIcon: Icon(Icons.search),
            label: 'Buscar',
          ),
          NavigationDestination(
            icon: Icon(Icons.inventory_2_outlined),
            selectedIcon: Icon(Icons.inventory_2),
            label: 'Objetos',
          ),
          NavigationDestination(
            icon: Icon(Icons.report_problem_outlined),
            selectedIcon: Icon(Icons.report_problem),
            label: 'Reportes',
          ),
          NavigationDestination(
            icon: Icon(Icons.person_outline),
            selectedIcon: Icon(Icons.person),
            label: 'Perfil',
          ),
        ],
      ),
    );
  }
}
