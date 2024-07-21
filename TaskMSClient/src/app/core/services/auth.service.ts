import { Injectable } from '@angular/core';
import { TokenModel } from '../models/token';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  token: TokenModel | null = null;
  tokenString!: string;

  constructor(private router: Router) { }

  checkAuthentication() {
    const responseString = localStorage.getItem("response");
    if (!responseString) {
      return this.toLogin();
    }

    const responseJson = JSON.parse(responseString);
    this.tokenString = responseJson?.accessToken;
    if (!this.tokenString) {
      return this.toLogin();
    }

    const decode: any= jwtDecode(this.tokenString);
    this.token = {
      email: decode?.Email,
      name: decode?.Name,
      userName: decode?.UserName,
      userId: decode?.UserId,
      exp: decode?.exp,
      roles: decode?.Roles
    };

    console.log(this.token);

    const now = new Date().getTime() / 1000;
    if (this.token.exp < now) {
      return this.toLogin();
    }

    return true;
  }

  toLogin() {
    this.router.navigateByUrl("/login");
    return false;
  }
  
}
