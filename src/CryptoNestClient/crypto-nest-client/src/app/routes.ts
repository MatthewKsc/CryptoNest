import { Route } from '@angular/router';
import { CryptoListComponent } from './modules/crypto-list/crypto-list.component';
import { UserManagementComponent } from './modules/user-managment/user-managment/user-management.component';

export const routes: Route[] = [
  { path: '', component: CryptoListComponent },
  { path: 'user', component: UserManagementComponent },
  { path: '**', redirectTo: "/", pathMatch: 'full' }
]
