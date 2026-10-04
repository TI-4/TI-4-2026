import { create } from "zustand";
import type { Role } from "../constants/role";
import type { UserIdentity } from "../interfaces/identities/UserIdentity";

interface AuthState {
  user: UserIdentity | null;
  token: string | null;
  isAuthenticated: boolean;

  login: (token: string, user: UserIdentity) => void;
  logout: () => void;

  // --- TEMPORAL MOCK TEST ---
  mockLoginAs: (role: Role) => void;
}

export const useAuthState = create<AuthState>((set) => ({
  user: null,
  token: localStorage.getItem('jwt_token'),
  isAuthenticated: !!localStorage.getItem('jwt_token'),

  login: (token, user) => {
    localStorage.setItem('jwt_token', token);
    set({ user, token, isAuthenticated: true });
  },

  logout: () => {
    localStorage.removeItem('jwt_token');
    set({ user: null, token: null, isAuthenticated: false });
  },

  // --- TEMPORAL MOCK TEST ---
  mockLoginAs: (role) => {
    const mockUser = {
      id: '123',
      role: role,
      name: `${role}`,
      email: `${role.toLowerCase()}@uct.cl`,
      registeredAt: new Date().toISOString()
    } as UserIdentity;
    const mockToken = 'mock_jwt_token_for_testing';
    localStorage.setItem('jwt_token', mockToken);
    set({ user: mockUser, token: mockToken, isAuthenticated: true });
  }
}));
