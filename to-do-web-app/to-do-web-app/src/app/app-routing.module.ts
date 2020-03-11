import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ManageToDoListComponent } from './components/manage-to-do-list/manage-to-do-list.component';
import { AuthGuard } from './guards/auth.guard';
import { ToDoListShareComponent } from './components/to-do-list-share/to-do-list-share.component';


const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'share/:id',
    component: ToDoListShareComponent
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard],
    data: {permissions: ['read:to-do-list']}
  },
  {
    path: 'to-do-list',
    component: ManageToDoListComponent,
    canActivate: [AuthGuard],
    data: {permissions: ['write:to-do-list']}
  },
  {
    path: 'to-do-list/:id',
    component: ManageToDoListComponent,
    canActivate: [AuthGuard],
    data: {permissions: ['write:to-do-list']}
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
