import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as auth0 from 'auth0-js'
import { environment } from 'src/environments/environment';
import { CommunicationService } from './communication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  // Create an observable of Auth0 instance of client
  auth0 = new auth0.WebAuth({
    domain: environment.auth.domain,
    clientID: environment.auth.clientID,
    responseType: environment.auth.responseType,
    redirectUri: environment.auth.redirectUri,
    audience: environment.auth.audience,
    scope: environment.auth.scope
  });

  expiresAt: number;
  accessToken: string;

  constructor(private router: Router, private commService: CommunicationService){ 
    this.expiresAt = +localStorage.getItem('expiresAt');
    this.accessToken = localStorage.getItem('accessToken');
  }

  login(){
    this.auth0.authorize();
  }

  handleLoginCallback(){

    this.auth0.parseHash((err, authResult) => {
      if(authResult && authResult.accessToken){
        window.location.hash = '';
        this._setSession(authResult, null);
        this.getUserInfo(authResult);
        this.router.navigate(['/dashboard']);
      }else if(err){
        console.error(`Error: ${err.error}`);
      }
    });
  }

  getUserInfo(authResult) {
    this.auth0.client.userInfo(authResult.accessToken, (err, profile) => {
      if (profile) {
        this._setSession(authResult, profile);
      }
    });
  }

  private _setSession(authResult, profile) {
    this.expiresAt = authResult.expiresIn * 1000 + Date.now();

    localStorage.setItem('permissions', authResult.scope);
    localStorage.setItem('expiresAt', this.expiresAt.toString());
    localStorage.setItem('username', profile == null ? "" : profile.name)
    localStorage.setItem('accessToken', authResult.accessToken);

  }

  getUserName(){
    return localStorage.getItem('username');
  }

  getAccessToken(){
    return localStorage.getItem('accessToken');
  }

  getUserPermissions(): string {
    return localStorage.getItem('permissions');
  }

  logout() {

    localStorage.removeItem('permissions');
    localStorage.removeItem('expiresAt');
    localStorage.removeItem('username')
    localStorage.removeItem('accessToken');

    this.auth0.logout({
      returnTo: 'http://localhost:4200',
      clientID: environment.auth.clientID
    });
  }

  get isLoggedIn(): boolean {
    return Date.now() < this.expiresAt;
  }
}
