import { AuthenticatedUser } from '../models';

export interface AuthState {
  user: AuthenticatedUser | undefined;
}

export const initialState: AuthState = {
  user: undefined,
};
