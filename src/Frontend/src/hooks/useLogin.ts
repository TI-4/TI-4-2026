import { useMutation } from '@tanstack/react-query';
import { authService } from '../services/authService';

export const useLogin = () => {
  return useMutation({
    mutationFn: ({ email, password }: Record<string, string>) => authService.login(email, password),
  });
};
