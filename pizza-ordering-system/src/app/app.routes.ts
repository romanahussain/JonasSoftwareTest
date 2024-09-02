
import { Routes } from '@angular/router';
import { OrderEntryComponent } from './order-entry/order-entry.component';

export const routes: Routes = [
    { path: 'order', component: OrderEntryComponent },
    { path: '', redirectTo: '/order', pathMatch: 'full' }
];
