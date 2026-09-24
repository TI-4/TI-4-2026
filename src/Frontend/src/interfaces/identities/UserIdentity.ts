export type Role = 'ADMIN';

export interface UserIdentity {
  id: string;
  role: Role;
  name: string;
  email: string;
  registeredAt: string;
  avatar?: string;
}
