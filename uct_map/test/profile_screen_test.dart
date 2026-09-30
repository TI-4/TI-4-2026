import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:uct_map/presentation/screens/profile/profile_screen.dart';
import 'package:uct_map/presentation/screens/profile/saved_places_screen.dart';
import 'package:uct_map/presentation/screens/profile/my_reports_screen.dart';
import 'package:uct_map/presentation/screens/profile/my_lost_items_screen.dart';
import 'package:uct_map/presentation/screens/profile/profile_settings_screen.dart';

void main() {
  Widget createWidgetUnderTest() {
    return const MaterialApp(
      home: Scaffold(
        body: ProfileScreen(),
      ),
    );
  }

  testWidgets('ProfileScreen muestra avatar con lápiz y los 5 botones, sin rol sin sesión', (tester) async {
    await tester.pumpWidget(createWidgetUnderTest());

    // 1. Verificar lápiz de edición sobre el avatar
    expect(find.byIcon(Icons.edit), findsOneWidget);

    // 2. Sin sesión no hay rol asignado: no se muestra ningún pill de rol
    expect(find.text('Estudiante'), findsNothing);

    // 3. Verificar los 5 botones principales según el diseño Figma
    expect(find.text('Mis favoritos o guardados'), findsOneWidget);
    expect(find.text('Mis reportes'), findsOneWidget);
    expect(find.text('Objetos reportados'), findsOneWidget);
    expect(find.text('Configuración'), findsOneWidget);
    expect(find.text('Cerrar sesión'), findsOneWidget);

    // 4. Verificar que muestra únicamente la cantidad numérica y no texto '¡1 Alerta!'
    expect(find.text('¡1 Alerta!'), findsNothing);
    expect(find.text('1'), findsOneWidget); // Conteo simple de notificaciones
  });

  testWidgets('ProfileScreen sin notificaciones no muestra badge en Objetos reportados', (tester) async {
    await tester.pumpWidget(const MaterialApp(
      home: Scaffold(
        body: ProfileScreen(notificationsCount: 0),
      ),
    ));

    expect(find.text('Objetos reportados'), findsOneWidget);
    expect(find.text('¡1 Alerta!'), findsNothing);
  });

  testWidgets('Navegación a Mis favoritos o guardados', (tester) async {
    await tester.pumpWidget(createWidgetUnderTest());

    await tester.tap(find.text('Mis favoritos o guardados'));
    await tester.pumpAndSettle();

    expect(find.byType(SavedPlacesScreen), findsOneWidget);
    expect(find.text('Mis Favoritos y Guardados'), findsOneWidget);
    expect(find.text('Biblioteca Central San Francisco'), findsOneWidget);
  });

  testWidgets('Navegación a Mis reportes', (tester) async {
    await tester.pumpWidget(createWidgetUnderTest());

    await tester.tap(find.text('Mis reportes'));
    await tester.pumpAndSettle();

    expect(find.byType(MyReportsScreen), findsOneWidget);
    expect(find.text('Mis Reportes de Incidencias'), findsOneWidget);
  });

  testWidgets('Navegación a Objetos reportados con notificaciones', (tester) async {
    await tester.pumpWidget(createWidgetUnderTest());

    await tester.tap(find.text('Objetos reportados'));
    await tester.pumpAndSettle();

    expect(find.byType(MyLostItemsScreen), findsOneWidget);
    expect(find.text('¡Tu objeto ha sido encontrado!'), findsOneWidget);
  });

  testWidgets('Navegación a Configuración', (tester) async {
    await tester.pumpWidget(createWidgetUnderTest());

    await tester.tap(find.text('Configuración'));
    await tester.pumpAndSettle();

    expect(find.byType(ProfileSettingsScreen), findsOneWidget);
    expect(find.text('Notificaciones Push'), findsOneWidget);
  });

  testWidgets('Botón Cerrar sesión abre diálogo de confirmación', (tester) async {
    await tester.pumpWidget(createWidgetUnderTest());

    final logoutButton = find.text('Cerrar sesión');
    await tester.ensureVisible(logoutButton);
    await tester.pumpAndSettle();

    await tester.tap(logoutButton);
    await tester.pumpAndSettle();

    expect(find.text('¿Estás seguro de que deseas cerrar sesión? Deberás ingresar nuevamente con tu correo institucional @uct.cl.'), findsOneWidget);
    expect(find.text('Cancelar'), findsOneWidget);
  });
}
