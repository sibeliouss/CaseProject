import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private apiUrl: string = "http://localhost:5097/api";

  constructor(
    private http: HttpClient,
    private auth: AuthService,
  ) { }

  get(api: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/${api}`, {
      headers: {
        "Authorization": "Bearer " + this.auth.tokenString
      }
    });
  }

  post(api: string, data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/${api}`, data, {
      headers: {
        "Authorization": "Bearer " + this.auth.tokenString
      }
    });
  }
}
