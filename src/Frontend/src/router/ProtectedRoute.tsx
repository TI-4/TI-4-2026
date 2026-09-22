import type { Role } from "../constants/role";
import { useAuthState } from "../states/authState";

interface ProtectedRouteProps {
  allowedRoles?: Role[];
}

export const ProtectedRoute = ({ allowedRoles }: ProtectedRouteProps) => {
  const { isAuthenticated, user } = useAuthState();

  // Avoid unauthorized users requests
  if (!isAuthenticated || !user) {
    return <Navigate to="/map" replace />;
  }

  if (allowedRoles && allowedRoles.length > 0 && !allowedRoles.includes(user.role)) {
    return <Navigate to="/map" replace />;
  }

  // Authorized user
  return <Outlet/>;
}
