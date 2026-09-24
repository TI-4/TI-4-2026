enum UserRole {
  estudiante,
  profesor,
  administrador,
  funcionarioObjetos,
  funcionarioQuejas,
  invitado,
  desconocido;

  static UserRole fromString(dynamic value) {
    if (value is Map) {
      value = value['nombre'] ?? value['name'] ?? value['rol'];
    }
    final v = value.toString().trim().toLowerCase();
    final n = v.replaceAll(RegExp(r'[\s_\-]+'), '');
    if (n.contains('estudiante') || n.contains('student')) return UserRole.estudiante;
    if (n.contains('docente') || n.contains('profesor') || n.contains('teacher')) {
      return UserRole.profesor;
    }
    if (n.contains('admin')) return UserRole.administrador;
    if (n.contains('objeto') || n.contains('object')) return UserRole.funcionarioObjetos;
    if (n.contains('queja') || n.contains('complaint')) return UserRole.funcionarioQuejas;
    if (n.contains('invitado')) return UserRole.invitado;
    return UserRole.desconocido;
  }

  String toShortString() {
    switch (this) {
      case UserRole.estudiante:
        return 'estudiante';
      case UserRole.profesor:
        return 'profesor';
      case UserRole.administrador:
        return 'administrador';
      case UserRole.funcionarioObjetos:
        return 'funcionario_objetos';
      case UserRole.funcionarioQuejas:
        return 'funcionario_quejas';
      case UserRole.invitado:
        return 'invitado';
      case UserRole.desconocido:
        return 'desconocido';
    }
  }
}
