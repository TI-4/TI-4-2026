import '../entities/building.dart';
import '../entities/campus.dart';
import '../entities/category.dart';
import '../entities/room.dart';
import '../entities/structure.dart';

/// Contrato del repositorio para consumir el Campus Service.
abstract class CampusRepository {
  Future<List<Campus>> getCampuses();

  Future<List<Building>> getBuildings(String campusId);

  Future<List<Room>> getRooms(String buildingId);

  Future<List<Structure>> getStructures(String campusId);

  Future<List<Category>> getCategories();
}
