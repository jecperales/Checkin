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

  dateCheckin: string = "Aun no registra su entrada";
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

    var res = false;

    if(hasPermission)
    {
      if(Capacitor.isNativePlatform())
      {
        if(canUseGPS)
        {
          res = true;
        }
        else
        {
          this.presentAlert("Devices", "GPS", "Por favor encienda so GPS y conceda los permisos para acceder a su ubicacion.");
          res = false;
        }
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
        else
        {
          this.presentAlert("Devices", "GPS", "Por favor encienda so GPS y conceda los permisos para acceder a su ubicacion.");
          res = false;
        }
      }
      else
      {
        await this.presentAlert("Permisos", "Acceso denegado", "El usuario ha denegado los permisos de geolocalizacion.");
        res = false;
      }

    }

    return res;

  }

  // async checkPermissions(){
  //   var hasPermission = await this.permissions.checkGPSPermission();
  //   if(hasPermission){
  //     if(Capacitor.isNativePlatform()){
  //       this.canUseGPS = await this.permissions.askToTurnOnGPS();
  //       this.postGPSPermission(this.canUseGPS);
  //     }
  //     else{
  //       //this.postGPSPermission(true);
  //       //this.canUseGPS = true;
  //     }
  //   }
  //   else{
  //     var permission = await this.permissions.requestGPSPermission();
  //     if(permission === "CAN_REQUEST" || permission === "GOT_PERMISSION"){
  //       if(Capacitor.isNativePlatform()){
  //         this.canUseGPS = await this.permissions.askToTurnOnGPS();
  //         this.postGPSPermission(this.canUseGPS);
  //       }
  //       else{
  //         //this.postGPSPermission(true);
  //         //this.canUseGPS = true;
  //       }
  //     }
  //     else{
  //       await this.presentAlert("Permisos", "Acceso denegado", "El usuario ha denegado los permisos de geolocalizacion.");
  //       this.canUseGPS = false;
  //     }
  //   }
  // }

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
            this.asistence.id_asistencia = 0;
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

  async saveInOut(){
    
    if(this.asistence.id_asistencia == 0)//Insertamos un registro nuevo
    {
        //await this.getCurrentPosition();
        //this.checkPermissions();  
        var permissionsOk = await this.checkPerm();
        
        if(permissionsOk)
        {
          await this.getCurrentPosition();
        }
        else
        {
          return;
        }


        this.asistence.latitud = this.latitude;
        this.asistence.longitud = this.longitude;
        this.asistence.fecha_hora_movil = this.GetFormattedDate(new Date());
        this.asistence.fecha_hora_registro = this.asistence.fecha_hora_movil;
        this.asistence.id_ingeniero = this.engineer.id_ingeniero;
        this.asistence.fecha_hora_salida="0001-01-01T00:00:00";
        this.asistence.s_latitud = 0;
        this.asistence.s_longitud = 0;

        await this.apiAsistance.postAsistence(this.asistence).subscribe(
          {
            next: (res) =>{
              if(res){
                this.presentAlert("Asistencia","Entrada","Su entrada ha sido registrada con éxito");

                this.dateCheckin = this.asistence.fecha_hora_registro.toString().replace("T"," ")
                this.inOut = "Salida"

                this.getTodayAsistence();
              } 
            },
            error: (err) =>{
              this.presentAlert("Error Exception.", "Error desconocido", "Ocurrio un error al registrar si asitencia. Favor de contactar a su administrador.");
            },
            complete: () => {
              console.log("saveInOut complete...");
            }
          }
        );
   
    }
    else//Actualizamos el registro
    {
      //this.getCurrentPosition();
      //await this.checkPermissions();
      var permissionsOk = await this.checkPerm();
        
      if(permissionsOk)
      {
        await this.getCurrentPosition();
      }
      else
      {
        return;
      }
      
      this.asistence.fecha_hora_salida = await this.GetFormattedDate(new Date());
      this.asistence.s_latitud = this.latitude;
      this.asistence.s_longitud = this.longitude;

      console.log(this.asistence);

      await this.apiAsistance.putAsistencia(this.asistence).subscribe({
        next: (res) => {
          this.presentAlert("Asistencia", "Salida", "Su salida ha sido registrada con éxito");
          this.dateCheckout = this.asistence.fecha_hora_salida.toString().replace("T"," ")
        },
        error: (err) => {
          console.log(err);
          this.presentAlert("Error Exception.", "Error desconocido", "Ocurrio un error al registrar su asitencia. Favor de contactar a su administrador.");
        }
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

  async getCurrentPosition(){
    await navigator.geolocation.getCurrentPosition((position) => {
      this.latitude = position.coords.latitude;
      this.longitude = position.coords.longitude;
      // console.log("latitud: " + this.latitude + " - " +"longitud: " + this.longitude);

      this.center = {
        lat: position.coords.latitude,
        lng: position.coords.longitude,
      };
        // Set marker position
      this.setMarkerPosition(this.latitude, this.longitude);
  
      this.getAddress(this.latitude, this.longitude);          
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
