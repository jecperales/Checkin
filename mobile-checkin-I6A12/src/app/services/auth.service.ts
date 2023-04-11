import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  urlApi = "http://189.203.240.97/api-asistencia-revp/api/";
  constructor(public http:HttpClient) { }

  getLogin( uid: string,  pwd: string){
    return this.http.get(this.urlApi + "Ingeniero/Auth?uid=" + uid + "&pwd=" + pwd);
  }
}
