import { Property } from './property.model';
import { UserRole } from './user-role.enum';

export interface User {
  id?: number;
  username: string;
  password: string;
  userRole?: UserRole;
  properties?: Property[];
}
