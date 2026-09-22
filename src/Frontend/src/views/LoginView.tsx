import { useNavigate } from 'react-router-dom';
import { useAuthState } from '../states/authState';
import type { Role } from '../constants/role';

export const LoginView = () => {
  const navigate = useNavigate();
  const mockLoginAs = useAuthState((state) => state.mockLoginAs);
  const logout = useAuthState((state) => state.logout);

  const handleLogin = (role: Role) => {
    mockLoginAs(role);
    navigate('/map'); // Redirigimos al inicio después de loguear
  };

  const handleGuest = () => {
    logout(); // Nos aseguramos de limpiar cualquier sesión
    navigate('/map');
  };

  return (
    <div className="w-full h-full flex flex-col items-center justify-center bg-gray-100 space-y-4">
      <h1 className="text-3xl font-bold mb-4">Simulador de Login</h1>
      <p className="text-gray-600 mb-8">Elige un rol para entrar:</p>
      
      <button onClick={() => handleGuest()} className="px-6 py-2 bg-gray-500 text-white rounded-md hover:bg-gray-600 w-80 text-center font-bold shadow-md">
        Entrar como Invitado (Sin Cuenta)
      </button>

      <div className="h-4"></div> {/* Separador visual */}

      <button onClick={() => handleLogin('MEMBER')} className="px-6 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 w-80 text-left">
        Entrar como Miembro Normal
      </button>
      <button onClick={() => handleLogin('INCIDENTS_OFFICER')} className="px-6 py-2 bg-red-500 text-white rounded-md hover:bg-red-600 w-80 text-left">
        Entrar como Encargado de Incidentes
      </button>
      <button onClick={() => handleLogin('OBJECTS_OFFICER')} className="px-6 py-2 bg-orange-500 text-white rounded-md hover:bg-orange-600 w-80 text-left">
        Entrar como Encargado de Objetos
      </button>
      <button onClick={() => handleLogin('ADMIN')} className="px-6 py-2 bg-purple-500 text-white rounded-md hover:bg-purple-600 w-80 text-left">
        Entrar como Administrador
      </button>
    </div>
  );
};
