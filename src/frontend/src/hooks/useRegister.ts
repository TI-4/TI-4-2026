import type { RegisterVariables } from '../interfaces/RegisterVariables';
import { useMutation } from '@tanstack/react-query';
import { authService } from '../services/authService';

export const useRegister = () => {
  return useMutation({
    mutationFn: (data: RegisterVariables) => authService.register(data.name, data.email, data.password),
  });
};
