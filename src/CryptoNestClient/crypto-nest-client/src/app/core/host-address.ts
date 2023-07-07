import { environment } from '../../environments/environment';

export const hostAddress = (): string => environment.apiUrl || location.origin;
