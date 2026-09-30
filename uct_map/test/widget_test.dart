import 'package:flutter_map/flutter_map.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:uct_map/main.dart';

void main() {
  testWidgets('Smoke test de inicio de la aplicación UCT Map', (WidgetTester tester) async {
    // Construir la app y renderizar el primer frame.
    await tester.pumpWidget(const UctMapApp());
    await tester.pump();

    // Verificar que cargue la pantalla principal con el mapa interactivo FlutterMap.
    expect(find.byType(FlutterMap), findsOneWidget);
    // Verificar que los destinos del navbar estén presentes.
    expect(find.text('Mapa'), findsOneWidget);
    expect(find.text('Buscar'), findsOneWidget);
    expect(find.text('Objetos'), findsOneWidget);
    expect(find.text('Reportes'), findsOneWidget);
    expect(find.text('Perfil'), findsOneWidget);
  });
}
