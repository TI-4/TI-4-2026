import 'package:flutter/material.dart';
import 'domain/repositories/auth_repository.dart';
import 'presentation/navigation/main_navigation_screen.dart';
import 'presentation/screens/professors/professors_screen.dart';
import 'presentation/screens/reports/reports_screen.dart';
import 'presentation/screens/login/login_page.dart';
import 'presentation/screens/lost_found/lost_found_screen.dart';

void main() {
  runApp(const UctMapApp());
}

class UctMapApp extends StatelessWidget {
  const UctMapApp({super.key, this.authRepository});

  /// Auth del login; por defecto usa el backend real.
  final AuthRepository? authRepository;

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'UCT Map',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        useMaterial3: true,
        colorScheme: ColorScheme.fromSeed(
          seedColor: const Color(0xFF003865), // Azul institucional UCT
          primary: const Color(0xFF003865),
          secondary: const Color(0xFFEAA221), // Tono secundario dorado/cálido
        ),
        appBarTheme: const AppBarTheme(
          centerTitle: true,
          elevation: 0,
        ),
      ),
      initialRoute: '/',
      routes: {
        '/': (context) => const MainNavigationScreen(),
        '/professors': (context) => const ProfessorsScreen(),
        '/reports': (context) => const ReportsScreen(),
        '/lost-found': (context) => const LostFoundScreen(),
        '/login': (context) => LoginPage(authRepository: authRepository),
      },
    );
  }
}
