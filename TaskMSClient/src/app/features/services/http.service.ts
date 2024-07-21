import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private apiUrl = 'http://localhost:5097/api/';

  constructor(
    private http: HttpClient,
    private auth: AuthService,
  
  ) { }

  get(api: string, callBack: (res: any) => void) {
    this.http.get(`http://localhost:5097/api/${api}`, {
      headers: {
        "Authorization": "Bearer " + this.auth.tokenString
      }
    })
      .subscribe({
        next: (res: any) => {
          callBack(res);
        },
        error: (err: any) => {
          console.error('Error:', err);
        }
      
      })
  }

  get1(api: string, data: any, callBack: (res: any) => void) {
    this.http.post(`http://localhost:5097/api/${api}`, data, {
      headers: {
        "Authorization": "Bearer " + this.auth.tokenString
      }
    })
      .subscribe({
        next: (res: any) => {
          callBack(res);
        },
        error: (err: any) => {
          console.error('Error:', err);
        }
        
      })
  }

  post(api: string, data: any, callBack: (res: any) => void) {
    this.http.post(`http://localhost:5097/api/${api}`, data, {
      headers: {
        "Authorization": "Bearer " + this.auth.tokenString
      }
    })
      .subscribe({
        next: (res: any) => {
          callBack(res);
        },
        error: (err: any) => {
          console.error('Error:', err);
        }
        
      })
  }

  update(api: string, data: any, callBack: (res: any) => void) {
    this.http.put(`http://localhost:5097/api/${api}`, data, {
      headers: {
        "Authorization": "Bearer " + this.auth.tokenString
      }
    })
      .subscribe({
        next: (res: any) => {
          callBack(res);
        },
        error: (err: any) => {
          console.error('Error:', err);
        }
        
      });
    
  }
  
  delete(api: string, callBack: (res: any) => void) {
    this.http.delete(`http://localhost:5097/api/${api}`, {
     
      headers: {
        "Authorization": "Bearer " + this.auth.tokenString
      }
    })
    .subscribe({
      next: (res: any) => {
        callBack(res);
      },
      error: (err: any) => {
        console.error('Error:', err);
      }
    });
  }
  getTasksFilteredByStatus(status: string): Observable<any> {
    const url = this.apiUrl + 'Tasks/FilterByStatus';
    const params = new HttpParams().set('status', status);
    return this.http.get(url, {
       headers: new HttpHeaders({
        "Authorization": "Bearer " + this.auth.tokenString
      }), 
      params: params
    });
  }
 
  
}
