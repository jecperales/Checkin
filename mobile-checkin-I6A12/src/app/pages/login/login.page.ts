import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertController } from '@ionic/angular';
import { AuthService } from '../../services/auth.service';
import { LoadingController } from '@ionic/angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {

  uid: any;
  password: any;
  engineer: any ={
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
  
  constructor(public apiAuth:AuthService, private alertCtrl: AlertController, private route: Router, private loadingCtrl: LoadingController) {
  }

  ngOnInit() {
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


  async showLoading() {
    const loading = await this.loadingCtrl.create({
      message: 'Espere...',
      duration: 2000,
    });

    loading.present();
  }

  
  login(uid: string, password: string)
  {
    if(uid == undefined || uid.trim()=="")
    {
      this.presentAlert("Datos no válidos.", "", "Introduzca un nombre de usuario"); 
      return;           
    }

    if(password == undefined || password.trim()=="")
    {
      this.presentAlert("Datos no válidos.", "", "Introduzca una contraseña"); 
      return;           
    }

    this.apiAuth.getLogin(this.uid, this.password).subscribe(
      {
        next: (res) =>{
          if(res){
            this.engineer = res;
            //this.showLoading();          

            //Cargamos la pagina del mapa y enviamos los datos del ingeniero logueado
            this.route.navigate(['/asistencia'],{state: this.engineer});
          }
          else{
            this.presentAlert("Autenticación.", "Credenciales incorrectas.", "Usuario y/o contraseña incorrectos.");            
          }    
        },
        error: (err) => {
          console.log("Error: ");
          console.error(err);
        },
        complete: () => {
          console.info("Complete");
          
        }
      }
    );

  }
}
