import { Navigate, Outlet } from 'react-router-dom';
import { useAuthState } from '../states/authState';
import type { Role } from '../constants/role';

interface ProtectedRouteProps {
  allowedRoles?: Role[];
  requireAuth?: boolean;
}

export const ProtectedRoute = ({ allowedRoles, requireAuth }: ProtectedRouteProps) => {
  const { isAuthenticated, user } = useAuthState();

  // 1. Invitado: Si la ruta exige autenticación o roles, y no está logueado, lo devolvemos al mapa
  if ((requireAuth || (allowedRoles && allowedRoles.length > 0)) && (!isAuthenticated || !user)) {
    return <Navigate to="/map" replace />;
  }

  // 2. Validación cruzada: Si la ruta exige roles específicos y el usuario no los tiene
  if (allowedRoles && allowedRoles.length > 0 && !allowedRoles.includes(user!.role)) {
    // Ej: Entra un PROFESOR a una vista de ['ADMIN', 'INCIDENTS_OFFICER'] -> Rechazado
    return <Navigate to="/map" replace />;
  }

  // 3. Autorizado
  return <Outlet />;
};
