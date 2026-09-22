import { useNavigate } from 'react-router-dom';
import { useAuthState } from '../states/authState';
import type { Role } from '../constants/role';
import { Button } from '../components/Button';

export const LoginView = () => {
  const navigate = useNavigate();
  const mockLoginAs = useAuthState((state) => state.mockLoginAs);
  const logout = useAuthState((state) => state.logout);

  const handleLogin = (role: Role) => {
    mockLoginAs(role);
    navigate('/map');
  };

  const handleGuest = () => {
    logout();
    navigate('/map');
  };

  return (
    <div className="w-full min-h-screen flex items-center justify-center bg-page-dark font-sans relative overflow-hidden">

      {/* Placeholder */}
      <h1 className="text-4xl font-bold text-white tracking-widest uppercase opacity-20 text-center">
        &lt;Login View&gt;
      </h1>

      {/* Dev Tool Panel */}
      <div className="absolute bottom-6 right-6 bg-white p-4 rounded-2xl shadow-xl border border-gray-200 flex flex-col items-center gap-4 w-56 z-10">
        <div className="flex flex-col items-center gap-0">
          <h2 className="text-lg font-bold text-gray-800">Dev Tool</h2>
        </div>

        <div className="flex flex-col w-full gap-2">
          <Button onClick={handleGuest} color="gray" size="sm" className="!w-full !px-2">
            Invitado
          </Button>

          <div className="w-full h-px bg-gray-100 my-0.5"></div>

          <Button onClick={() => handleLogin('MEMBER')} color="blue" size="sm" className="!w-full !px-2">
            Estudiante
          </Button>

          <Button onClick={() => handleLogin('TEACHER')} color="dark" size="sm" className="!w-full !px-2">
            Profesor
          </Button>

          <Button onClick={() => handleLogin('OFFICIAL')} color="green" size="sm" className="!w-full !px-2">
            Funcionario
          </Button>

          <Button onClick={() => handleLogin('INCIDENTS_OFFICER')} color="blue" size="sm" className="!w-full !px-2">
            Enc. Incidentes
          </Button>

          <Button onClick={() => handleLogin('OBJECTS_OFFICER')} color="blue" size="sm" className="!w-full !px-2">
            Enc. Objetos
          </Button>

          <Button onClick={() => handleLogin('ADMIN')} color="purple" size="sm" className="!w-full !px-2">
            Admin
          </Button>
        </div>
      </div>

    </div>
  );
};
