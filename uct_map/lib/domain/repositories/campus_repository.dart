import '../entities/building.dart';
import '../entities/campus.dart';
import '../entities/room.dart';

/// Contrato del repositorio para consumir el Campus Service vía API Gateway.
abstract class CampusRepository {
  Future<List<Campus>> getCampuses();
  Future<List<Building>> getBuildings(String campusId);
  Future<List<Room>> getRooms(String buildingId);
}
