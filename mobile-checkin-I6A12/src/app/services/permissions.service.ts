import { Injectable } from '@angular/core';
import { AndroidPermissions } from '@ionic-native/android-permissions';
import { LocationAccuracy } from '@ionic-native/location-accuracy';
import { Capacitor } from '@capacitor/core';

@Injectable({
  providedIn: 'root'
})
export class PermissionsService {

  constructor() { }

  async askToTurnOnGPS(): Promise<boolean> {
    return await new Promise((resolve, reject) => {
      LocationAccuracy.canRequest().then((canRequest: boolean) => {
        if(canRequest){
          //La opcion accuracy sera ignorada por IOS
          LocationAccuracy.request(LocationAccuracy.REQUEST_PRIORITY_HIGH_ACCURACY).then(
            () => {
              resolve(true);
            },
            error => {
              resolve(false);
            }
          );
        }
        else{
          resolve(false);
        }
      });
    });
  }


  //Checa si la aplicacion tiene permisos de uso del GPS
  async checkGPSPermission(): Promise<boolean> {
    return await new Promise((resolve, reject) =>{
      if(Capacitor.isNativePlatform()){
        AndroidPermissions.checkPermission(AndroidPermissions.PERMISSION.ACCESS_FINE_LOCATION).then(
          result =>{
            if(result.hasPermission){
              // Si hay permisos muestra 'Turn On GPS' dialogue
              resolve(true);
            }
            else{
              //Si no tiene permisos, solicita los permisos
              resolve(false);
            }
          },
          err => {
            alert(err);
          }
        );
      }
      else{
        resolve(true);
      }
    });
  }

  //Checa si hay permiso de uso de ubicacion
  async requestGPSPermission(): Promise<string>{
    return await new Promise((resolve, reject) =>{
      LocationAccuracy.canRequest().then((canRequest: boolean) => {
        if(canRequest){
          resolve('CAN_REQUEST');
        }
        else{
          //Muestra el 'GPS Permission Request' dialog
          AndroidPermissions.requestPermission(AndroidPermissions.PERMISSION.ACCESS_FINE_LOCATION).then(
            (result) => {
              if(result.hasPermission){
                //Llama al metodo para encender el GPS
                resolve('GOT_PERMISSION');
              }
              else{
                resolve('DENIED_PERMISSION');
              }
            },
            (err) => {
              //Muestra una alerta si el usuario da click en 'No, Thanks'
              alert("requestPermission Error requesting location permissions " + err);
            }
          );
        }
      });
    });

  }





}
