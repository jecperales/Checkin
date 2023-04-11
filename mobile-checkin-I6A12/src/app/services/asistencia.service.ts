import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AsistenciaService {

  urlApi = "http://189.203.240.97/api-asistencia-revp/api/";

  constructor(public http:HttpClient) { }

  getTodayAsistence(date:String, uid: Number){
    return this.http.get(this.urlApi + "Asistencia/Asistencia?fecha=" + date + "&id=" + uid );
  }

  postAsistence(asistence:any){
    return this.http.post(this.urlApi + "Asistencia", asistence);
  }

  putAsistencia(asistencia: any){
    return this.http.put(this.urlApi + "Asistencia/Update/", asistencia);
  }

}
