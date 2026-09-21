import 'dart:convert';

import 'package:flutter/services.dart';

import '../models/campus_model.dart';

class CampusLocalDataSource {
  Future<List<CampusModel>> getCampuses() async {
    final jsonString = await rootBundle.loadString('assets/data/campus.json');

    final Map<String, dynamic> jsonData = jsonDecode(jsonString);

    final campusesJson = jsonData['campus'] as List<dynamic>;

    return campusesJson
        .map((campus) => CampusModel.fromJson(campus as Map<String, dynamic>))
        .toList();
  }
}
