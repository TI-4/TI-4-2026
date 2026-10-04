import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthState } from '../../states/authState';

import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';
import { useLogin } from '../../hooks/useLogin';

interface LoginFormProps {
  onSwitchToRegister: () => void;
}

export const LoginForm = ({ onSwitchToRegister }: LoginFormProps) => {
  const navigate = useNavigate();
  const realLogin = useAuthState((state) => state.login);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const { mutateAsync: login, isPending: isLoading, error, reset: resetError } = useLogin();

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    resetError();

    try {
      const response = await login({ email, password });
      
      realLogin(response.token || '', {
        id: response.userId,
        email: response.email,
        name: 'Usuario Estudiante',
        role: 'MEMBER',
        registeredAt: new Date().toISOString()
      });
      
      navigate('/map');
    } catch (err) {
      
    }
  };

  return (
    <>
      <div className="flex flex-col gap-2 mb-8">
        <h1 className="text-3xl font-bold text-gray-800 tracking-tight">Iniciar Sesión</h1>
        <p className="text-gray-500 font-medium">Ingresa tus credenciales institucionales.</p>
      </div>

      <form onSubmit={onSubmit} className="flex flex-col gap-5">
        <Input
          label="Email"
          labelColor="text-gray-700"
          placeholder="usuario@uct.cl"
          value={email}
          onChange={(e: any) => setEmail(e.target.value)}
        />
        <Input
          label="Contraseña"
          labelColor="text-gray-700"
          type="password"
          placeholder="••••••••"
          value={password}
          onChange={(e: any) => setPassword(e.target.value)}
        />

        {error && (
          <div className="text-red-600 text-sm font-medium bg-red-50 p-2.5 rounded-lg border border-red-200">
            {error.message}
          </div>
        )}

        <Button type="submit" variant="solid" color="blue" size="lg" className="w-full mt-1" disabled={isLoading}>
          {isLoading ? 'Cargando...' : 'Iniciar Sesión'}
        </Button>
      </form>

      <div className="mt-5 text-center text-xl font-bold text-gray-600">
        ¿No tienes cuenta?{' '}
        <button
          type="button"
          onClick={onSwitchToRegister}
          className="text-blue-600 hover:underline"
        >
          Regístrate
        </button>
      </div>
    </>
  );
};
