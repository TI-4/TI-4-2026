import type { LoginVariables } from '../interfaces/LoginVariables';
import { useMutation } from '@tanstack/react-query';
import { authService } from '../services/authService';

export const useLogin = () => {
  return useMutation({
    mutationFn: (data: LoginVariables) => authService.login(data.email, data.password),
  });
};
