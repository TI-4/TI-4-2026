import '../../domain/entities/campus.dart';
import '../../domain/repositories/campus_repository.dart';
import '../datasources/campus_local_datasource.dart';

class CampusRepositoryImpl implements CampusRepository {
  final CampusLocalDataSource localDataSource;

  CampusRepositoryImpl({required this.localDataSource});

  @override
  Future<List<Campus>> getCampuses() async {
    return await localDataSource.getCampuses();
  }
}
