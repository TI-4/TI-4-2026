import { create } from "zustand";
import type { Role } from "../constants/role";
import type { UserIdentity } from "../interfaces/identities/UserIdentity";

interface AuthState {
  user: UserIdentity | null;
  isAuthenticated: boolean;

  login: (token: string, user: UserIdentity) => void;
  logout: () => void;

  // --- TEMPORAL MOCK TEST ---
  mockLoginAs: (role: Role) => void;
}

export const useAuthState = create<AuthState>((set) => ({
  user: null,
  isAuthenticated: false,
  login: (_token, user) => set({ user, isAuthenticated: true }),
  logout: () => set({ user: null, isAuthenticated: false }),

  // --- TEMPORAL MOCK TEST ---
  mockLoginAs: (role) => {
    const mockUser = {
      id: '123e4567',
      role: role,
      name: `Usuario ${role}`,
      email: `${role.toLowerCase()}@uct.cl`,
      registeredAt: new Date().toISOString()
    } as UserIdentity;
    set({ user: mockUser, isAuthenticated: true });
  }
}));
