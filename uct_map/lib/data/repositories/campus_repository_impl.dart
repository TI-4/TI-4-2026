import '../../domain/entities/building.dart';
import '../../domain/entities/campus.dart';
import '../../domain/entities/category.dart';
import '../../domain/entities/room.dart';
import '../../domain/entities/structure.dart';
import '../../domain/repositories/campus_repository.dart';
import '../datasources/campus_remote_ds.dart';

class CampusRepositoryImpl implements CampusRepository {
  final CampusRemoteDataSource remoteDataSource;

  CampusRepositoryImpl({required this.remoteDataSource});

  @override
  Future<List<Campus>> getCampuses() {
    return remoteDataSource.getCampuses();
  }

  @override
  Future<List<Building>> getBuildings(String campusId) {
    return remoteDataSource.getBuildings(campusId);
  }

  @override
  Future<List<Room>> getRooms(String buildingId) {
    return remoteDataSource.getRooms(buildingId);
  }

  @override
  Future<List<Structure>> getStructures(String campusId) {
    return remoteDataSource.getStructures(campusId);
  }

  @override
  Future<List<Category>> getCategories() {
    return remoteDataSource.getCategories();
  }
}
