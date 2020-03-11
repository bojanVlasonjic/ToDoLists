import { Injectable } from '@angular/core';
import { Router } from '@angular/router'
import { CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ToastService } from '../services/toast.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private toastService: ToastService) {

  }

  canActivate(route: ActivatedRouteSnapshot): boolean {

    if (!this.authService.isLoggedIn) {
      this.toastService.showMessage('Please login to proceed');
      return false;
    }

    let permissions = route.data.permissions as Array<string>;

    // verify whether the user contains all of the required permissions
    for (let permission of permissions) {
      if(!this.authService.getUserPermissions().includes(permission)) {
        this.toastService.showMessage('You are not allowed to access that page');
        return false;
      }
    }
   
    

    return true;
  }
  
}
