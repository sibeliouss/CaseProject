import { Routes } from '@angular/router';
import { LoginComponent } from './core/pages/login/login.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { DetailTaskComponent } from './features/pages/detail-task/detail-task.component';
import { HomeComponent } from './features/pages/home/home.component';
import { inject } from '@angular/core';
import { AuthService } from './core/services/auth.service';



export const routes: Routes = [

    {path:'login',component:LoginComponent},
  
    {
        path: '',
        component: NavbarComponent,
        // Navbara bağlı herhangi bir şey çağrıldığında tetiklenir.
        canActivateChild: [() => inject(AuthService).checkAuthentication()], 
        children: [
            {
                path:'home', component:HomeComponent
                
            },
           {
            path:'task-details/:id',  component: DetailTaskComponent
           }
        ] }
];
