import { Component, ElementRef, OnInit, NgZone, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AsistenciaService } from '../../services/asistencia.service';
import { AlertController, ToastController } from '@ionic/angular';
import { Capacitor } from '@capacitor/core';
import { PermissionsService } from '../../services/permissions.service';
import { IonLoaderService } from 'src/app/services/ion-loader.service';

import {
  GoogleMap,
  MapInfoWindow,
  MapGeocoder,
  MapGeocoderResponse
} from '@angular/google-maps';


@Component({
  selector: 'app-asistencia',
  templateUrl: './asistencia.page.html',
  styleUrls: ['./asistencia.page.scss'],
})
export class AsistenciaPage implements OnInit {

  @ViewChild('search')
  public searchElementRef!: ElementRef;
  @ViewChild('myGoogleMap', { static: false })
  map!: GoogleMap;
  @ViewChild(MapInfoWindow, { static: false })
  info!: MapInfoWindow;

  counter: number = 0;

  canUseGPS: boolean = false;

  address = '';
  latitude!: any;
  longitude!: any;
  zoom = 12;
  maxZoom = 15;
  minZoom = 8;
  center!: google.maps.LatLngLiteral;
  options: google.maps.MapOptions = {
    zoomControl: true,
    scrollwheel: false,
    disableDoubleClickZoom: true,
    mapTypeId: '',
    draggable:false,
    streetViewControl: false,
    //mapTypeControl: false,
    rotateControl: false
  };
  markers = [] as any;

  engineer: any = {
    id_ingeniero: Number,
    engineer_id: String,
    name: String,
    last_name_p: String,
    last_name_m: String,
    street: String,
    suburb: String,
    town: String,
    state: String,
    cell_phone: String,
    user: String,
    password: String, 
    id_profile: Number,
    id_proyecto: Number,
    pais: String,
    pais_reporte: String
  }

  asistence: any = {
    id_asistencia: Number,
    latitud: Number,
    longitud: Number,
    fecha_hora_movil: Date,
    fecha_hora_registro: Date,
    id_ingeniero: Number,
    fecha_hora_salida: Date,
    s_latitud: Number,
    s_longitud: Number
  }

  now = new Date();

  dateCheckin: string = "Aun no registra su entrada.";
  dateCheckout: string = "Aun no registra su salida.";

  iconIn: string = "skull";
  iconColorIn: string = "danger";

  iconOut: string = "skull";
  iconColorOut: string = "danger";

  inOut="Entrada";
  permisosOk: boolean = false;


  constructor(
    private router: Router, 
    private ngZone: NgZone, 
    private geoCoder: MapGeocoder, 
    private apiAsistance: AsistenciaService,
    private alertCtrl: AlertController,
    private permissions:PermissionsService,
    private loader: IonLoaderService) {

    this.engineer = this.router.getCurrentNavigation()?.extras.state;

  }

  ngOnInit() {
    
    //this.getCurrentPosition();

    if(this.engineer == undefined){
      this.engineer="";

      //Si el objeto engineer no esta inicializado regresamos al login (Es como una validacion de sesion pero sin serlo)
      this.router.navigate(['/login']);
    }
    else{
      this.loader.showLoading("Consultando permisos...").then(() => {      
        this.checkPerm();
        this.loader.hideLoader();        
      });

      this.getTodayAsistence();
    }

  }

  async checkPerm()
  {
    var hasPermission = await this.permissions.checkGPSPermission();
    var canUseGPS = await this.permissions.askToTurnOnGPS();
    console.log("asistencia.page.ts.checkPerm() => _canUseGPS...");
    console.log(canUseGPS);

    var res = false;

    if(hasPermission)
    {
      if(Capacitor.isNativePlatform())
      {
        if(canUseGPS)
        {
          res = true;
        }
        // else
        // {
        //   this.presentAlert("Devices", "GPS", "Por favor encienda so GPS y conceda los permisos para acceder a su ubicacion.");
        //   res = false;
        // }
      }
      else
      {
        res = true;        
      }
    }
    else
    {
      var permission = await this.permissions.requestGPSPermission();

      if(permission === "CAN_REQUEST" || permission === "GOT_PERMISSION")
      {
        if(canUseGPS)
        {
          res = true;
        }
        // else
        // {
        //   this.presentAlert("Devices", "GPS", "Por favor encienda so GPS y conceda los permisos para acceder a su ubicacion.");
        //   res = false;
        // }
      }
      else
      {
        await this.presentAlert("Permisos", "Acceso denegado - Negaciones: " + this.counter, "El usuario ha denegado los permisos de geolocalizacion. Puede que sea necesario activarlos manualmente.");          
        res = false;        
      }

    }

    return res;

  }
 
  async getTodayAsistence()
  {
    
    let date = new Date()
    let day = date.getDate();
    let month = date.getMonth() + 1; //Se necesita sumar +1 porque la numeracion va del 0 al 11 de enero a diciembre
    let year = date.getFullYear();

    let formattedDate = year.toString() + "-" + (month < 10 ? '0'+ month.toString():month.toString()) + "-" + (day < 10 ? '0' + day.toString() : day.toString());

    await this.apiAsistance.getTodayAsistence(formattedDate, this.engineer.id_ingeniero).subscribe(
      {
        next:(res) => {
          if(res){
            this.asistence = res;
            console.log("Asistencias del dia:");
            console.log(res);
            console.log(this.asistence);

            this.dateCheckin = this.asistence.fecha_hora_registro.toString().replace("T"," ");
            this.dateCheckout = this.asistence.fecha_hora_salida.toString().replace("T"," ");

            if(this.dateCheckin != undefined && this.dateCheckin > "1900-01-01"){
              this.inOut= "Salida";
            }

            if(this.dateCheckout < "1900-01-01")
            {
              this.dateCheckout = "Aun no registra su salida.";
            }
          }
          else
          {
            console.log("No hay datos de asistencia");
            console.log(res);            
            this.asistence.id_asistencia = 0;
            console.log(this.asistence);
          }
        },
        error: (err) => {
          console.log(err);
        },
        complete: () => { 
          console.log("getTodayAsistance complete...");

        }
      }
    );
  }

  async  saveInOut(){
    
    if(this.asistence.id_asistencia == 0)//Insertamos un registro nuevo
    {
      this.loader.showLoading("Obteniendo Coordenadas...")
      .then(async () => {      
        
        var permissionsOk = await this.checkPerm();
        
        if(permissionsOk)
        {

          let coordenadas = await this.getCurrentPosition();

          this.asistence.latitud = coordenadas.latitud;//this.latitude;
          this.asistence.longitud = coordenadas.longitud;//this.longitude;
          this.asistence.fecha_hora_movil = this.GetFormattedDate(new Date());
          this.asistence.fecha_hora_registro = this.asistence.fecha_hora_movil;
          this.asistence.id_ingeniero = this.engineer.id_ingeniero;
          this.asistence.fecha_hora_salida="0001-01-01T00:00:00";
          this.asistence.s_latitud = 0;
          this.asistence.s_longitud = 0;

          this.apiAsistance.postAsistence(this.asistence).subscribe(
            {
              next: (res) =>{
                if(res == 1){
                  this.presentAlert("Asistencia","Entrada","Su entrada ha sido registrada con éxito");

                  this.dateCheckin = this.asistence.fecha_hora_registro.toString().replace("T"," ")
                  this.inOut = "Salida"

                  this.getTodayAsistence();
                } 
                else if(res == -1){
                  this.presentAlert("Asistencia","Entrada","Ya hay un registro previo existente.");
                  this.inOut;
                  this.getTodayAsistence()
                }
                else{
                  this.presentAlert("Asistencia","Entrada","El registro no se guardo, Intente de nuevo");
                }
              },
              error: (err) =>{
                this.presentAlert("Error Exception.", "Error desconocido", "Ocurrio un error al registrar si asitencia. Favor de contactar a su administrador." + err);
              },
              complete: () => {
                console.log("saveInOut complete...");
              }
            }
          );
        }
        else
        {
          this.loader.hideLoader(); 
          return;
        } 

        this.loader.hideLoader();        
      })
      .catch((err) => {
        console.log("Error al guardar la entra: " + err);

      })
      .finally(() => {
        this.loader.hideLoader(); 

      });
           
    }
    else//Actualizamos el registro
    {
       
      this.loader.showLoading("Obteniendo Coordenadas...").then(async () => {      
        
        var permissionsOk = await this.checkPerm();
        
        if(permissionsOk)
        {
          let coordenadas = await this.getCurrentPosition();

          this.asistence.fecha_hora_salida = await this.GetFormattedDate(new Date());
          this.asistence.s_latitud = coordenadas.latitud;
          this.asistence.s_longitud = coordenadas.longitud;

          console.log(this.asistence);

          await this.apiAsistance.postAsistencia(this.asistence).subscribe({
            next: (res) => {
              if(res){
                this.presentAlert("Asistencia", "Salida", "Su salida ha sido registrada con éxito");
                this.dateCheckout = this.asistence.fecha_hora_salida.toString().replace("T"," ")
              }
              else{
                this.presentAlert("Asistencia", "Salida", "No se registro su salid. Server response: " + res)
              }
              
            },
            error: (err) => {
              console.log(err);
              this.presentAlert("Error Exception.", "Error desconocido", "Ocurrio un error al registrar su salida. Favor de contactar a su administrador. " + err);
            }
          });
        }
        else
        {
          this.loader.hideLoader(); 
          return;
        }

        this.loader.hideLoader();        
      }).
      catch((err) => {
        console.log(err);
      }). 
      finally(() => {
        this.loader.hideLoader();
      });
          
    }
  }  

  async postGPSPermission(canUseGPS: boolean) {
    if (canUseGPS) { await this.getCurrentPosition(); }
    else {
      // await Toast.show({
      //   text: 'Please turn on GPS to get location'
      // })
      this.presentAlert("Devices", "GPS", "Por favor encienda so GPS para acceder a su ubicacion.");
    }
  }

  async getCurrentPosition(): Promise<{latitud: number, longitud: number}>{
    return new Promise((resolve, reject) => {
      if(!navigator.geolocation){
        reject(new Error("La geolocalizacion no esta permitida en este navegador"));
      }
      else{
        navigator.geolocation.getCurrentPosition(
          (position) => {
            this.center = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude,
                  };
            // Set marker position
            this.setMarkerPosition(position.coords.latitude, position.coords.longitude);              
            this.getAddress(position.coords.latitude, position.coords.longitude); 

            resolve({
              latitud: position.coords.latitude,
              longitud: position.coords.longitude
            });
          },
          (error) => {
            reject(error);
          }
        );
      }
    });
  }

  async setMarkerPosition(latitude: any, longitude: any) {
    // Set marker position
    this.markers = [
      {
        position: {
          lat: latitude,
          lng: longitude,
        },
        options: {
          animation: google.maps.Animation.DROP,
          draggable: false,
        },
      },
    ];
  }

  eventHandler(event: any, name: string) {
    //console.log(event, name);
    console.log(name);
    switch (name) {
      case 'mapDblclick': // Agrega el marcador con un doble click
        break;

      case 'mapDragMarker': //
        break;

      case 'mapDragend':
        //this.getAddress(event.latLng.lat(), event.latLng.lng());    
        break;

      case 'zoomChanged':
     
        break;

      default:
        break;
    }
  }

  getAddress(latitude: any, longitude: any) {
    this.geoCoder
      .geocode({ location: { lat: latitude, lng: longitude } })
      .subscribe((addr: MapGeocoderResponse) => {
        if (addr.status === 'OK') {
          if (addr.results[0]) {
            this.zoom = 12;
            this.address = addr.results[0].formatted_address;
          } else {
            this.address = "";
            window.alert('Sin resultados');
          }
        } else {
          this.address = "";
          window.alert('Geocoder error: ' + addr.status);
        }
      });
  }

  GetFormattedDate(date: Date) {
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var day  = ("0" + (date.getDate())).slice(-2);
    var year = date.getFullYear();
    var hour =  ("0" + (date.getHours())).slice(-2);
    var min =  ("0" + (date.getMinutes())).slice(-2);
    var seg = ("0" + (date.getSeconds())).slice(-2);
    return year + "-" + month + "-" + day + "T" + hour + ":" +  min + ":" + seg;
  }

  async presentAlert(header:string, subHeader: string, message: string, ) {
    const alert = await this.alertCtrl.create({
      backdropDismiss: false,
      header: header,
      subHeader: subHeader,
      message: message,      
      buttons: ['OK'],
    });

    await alert.present();
  }

}
