import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthState } from '../states/authState';
import type { Role } from '../constants/role';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import { useLogin } from '../hooks/useLogin';
import logoUrl from '../assets/svg/logo.svg';
import LocationIcon from '../assets/svg/icons/icon_location.svg?react';

import bg1 from '../assets/images/uct_login_image_1.jpg';
import bg2 from '../assets/images/uct_login_image_2.jpg';
import bg3 from '../assets/images/uct_login_image_3.jpg';
import bg4 from '../assets/images/uct_login_image_4.jpg';
import bg5 from '../assets/images/uct_login_image_5.jpg';
import bg6 from '../assets/images/uct_login_image_6.jpg';
import bg7 from '../assets/images/uct_login_image_7.jpg';
import bg8 from '../assets/images/uct_login_image_8.jpg';
import bg9 from '../assets/images/uct_login_image_9.jpg';
import type { LoginBackgroundData } from '../interfaces/LoginBackgroundData';

const backgroundData: LoginBackgroundData[] = [
  { img: bg1, campus: 'Campus San Francisco', location: 'Edificio Central' },
  { img: bg2, campus: 'Campus Monseñor Alejandro Menchaca Lira', location: 'Casona Malmus' },
  { img: bg3, campus: 'Campus Monseñor Alejandro Menchaca Lira', location: 'Sala de Estudio' },
  { img: bg4, campus: 'Campus Monseñor Alejandro Menchaca Lira', location: 'Entrada' },
  { img: bg5, campus: 'Campus San Juan Pablo II', location: 'Laguna' },
  { img: bg6, campus: 'Campus San Juan Pablo II', location: 'Auditorio' },
  { img: bg7, campus: 'Campus Doctor Luis Rivas del Canto', location: 'Veterinaria' },
  { img: bg8, campus: 'Campus Nuestra Señora de Lourdes', location: 'Sala Principal' },
  { img: bg9, campus: 'Campus San Juan Pablo II', location: 'Edificio 8' },
];

export const LoginView = () => {
  const navigate = useNavigate();
  const mockLoginAs = useAuthState((state) => state.mockLoginAs);
  const logout = useAuthState((state) => state.logout);
  const [currentIndex, setCurrentIndex] = useState<number>(0);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const { mutateAsync: login, isPending: isLoading, error, reset: resetError } = useLogin();

  useEffect(() => {
    if (backgroundData.length === 0) return;

    // Random image
    const initialIndex = Math.floor(Math.random() * backgroundData.length);
    setCurrentIndex(initialIndex);

    // Change image
    const intervalId = setInterval(() => {
      setCurrentIndex((prev) => (prev + 1) % backgroundData.length);
    }, 6000);

    return () => clearInterval(intervalId);
  }, []);

  const handleLogin = (role: Role) => {
    mockLoginAs(role);
    navigate('/map');
  };

  const handleGuest = () => {
    logout();
    navigate('/map');
  };

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    resetError();

    try {
      await login({ email, password });
      handleLogin('MEMBER');
    } catch (err) {
      // El error se maneja automáticamente en la variable 'error' de TanStack Query
    }
  };

  return (
    <div className="w-full min-h-screen flex items-center font-sans relative overflow-hidden bg-page-dark">

      {backgroundData.map((data, index) => {
        const isCurrent = index === currentIndex;
        const isPrevious = index === (currentIndex - 1 + backgroundData.length) % backgroundData.length;

        // Outer container classes
        let positionClass = 'opacity-0 translate-x-16';
        if (isCurrent) {
          positionClass = 'opacity-100 translate-x-0';
        } else if (isPrevious) {
          positionClass = 'opacity-0 -translate-x-16';
        }

        // Inner container classes
        let innerPosition = 'translate-x-0';
        let innerTransition = 'transition-none';

        if (isCurrent) {
          innerPosition = 'translate-x-8';
          innerTransition = 'transition-transform duration-[6000ms] ease-linear';
        } else if (isPrevious) {
          innerPosition = 'translate-x-8';
          innerTransition = 'transition-transform duration-[6000ms] ease-linear';
        }

        return (
          <div
            key={data.img}
            className={`absolute inset-0 transition-all duration-[1500ms] ease-in-out z-0 overflow-hidden ${positionClass}`}
          >
            {/* Panning image */}
            <div
              className={`absolute -inset-8 w-[calc(100%+4rem)] h-[calc(100%+4rem)] bg-cover bg-center scale-[1.25] ${innerTransition} ${innerPosition}`}
              style={{ backgroundImage: `url(${data.img})` }}
            />
          </div>
        );
      })}

      <div className="absolute inset-0 bg-black/40 z-0 pointer-events-none"></div>

      {backgroundData.map((data, index) => {
        const isCurrent = index === currentIndex;
        return (
          <div
            key={`${data.img}-info`}
            className={`absolute top-12 right-12 flex items-center gap-4 z-20 transition-opacity duration-[1500ms] ease-in-out ${isCurrent ? 'opacity-100' : 'opacity-0 pointer-events-none'}`}
          >
            <div className="flex flex-col items-end text-right">
              <span className="text-xl font-bold text-white tracking-wide drop-shadow-md">
                {data.campus}
              </span>
              <span className="text-sm font-medium text-white/80 drop-shadow-md">
                {data.location}
              </span>
            </div>
            <LocationIcon className="w-10 h-10 text-white" />
          </div>
        );
      })}

      {/* Left panel */}
      <div className="w-full max-w-lg h-[calc(100vh-4rem)] my-8 ml-8 bg-gradient-to-b from-page-blue from-50% to-page-yellow to-50% p-1.5 rounded-[34px] shadow-2xl relative z-10">
        <div className="w-full h-full bg-white rounded-[28px] p-12 flex flex-col justify-center">

          {/* Header */}
          <div className="flex items-center mb-10">
            <img src={logoUrl} alt="UCT Logo" className="w-24 h-24 object-contain" />
            <div className="ml-5 flex flex-col justify-center">
              <span className="text-gray-800 font-black text-4xl leading-none tracking-wide block">UCT</span>
              <span className="text-gray-800 font-black text-4xl leading-none tracking-wide block">MAP</span>
            </div>
          </div>

          {/* Heading */}
          <div className="flex flex-col gap-2 mb-10">
            <h1 className="text-3xl font-bold text-gray-800 tracking-tight">Iniciar Sesión</h1>
            <p className="text-gray-500 font-medium">Ingresa tus credenciales institucionales para continuar.</p>
          </div>

          {/* Form */}
          <form onSubmit={onSubmit} className="flex flex-col gap-6">
            <Input
              label="Email"
              labelColor="text-gray-700"
              placeholder="Ej. juan.perez@uct.cl"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
            <Input
              label="Contraseña"
              labelColor="text-gray-700"
              type="password"
              placeholder="••••••••"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />

            {error && (
              <div className="text-red-600 text-sm font-medium bg-red-50 p-3 rounded-lg border border-red-200">
                {error.message}
              </div>
            )}

            <Button type="submit" variant="solid" color="blue" size="lg" className="w-full mt-2" disabled={isLoading}>
              {isLoading ? 'Cargando...' : 'Iniciar Sesión'}
            </Button>
          </form>

          <div className="flex items-center my-4">
            <div className="flex-grow border-t border-gray-200"></div>
            <span className="px-3 text-xs text-gray-400 font-medium">O bien</span>
            <div className="flex-grow border-t border-gray-200"></div>
          </div>

          <Button
            type="button"
            variant="outline"
            color="gray"
            size="lg"
            className="w-full"
            onClick={handleGuest}
          >
            Continuar como invitado
          </Button>

        </div>
      </div>

      {/* Dev Tool Panel */}
      <div className="absolute bottom-6 right-6 bg-white p-4 rounded-2xl shadow-xl border border-gray-200 flex flex-col items-center gap-4 w-56 z-20">
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
