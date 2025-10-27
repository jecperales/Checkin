import { Injectable } from '@angular/core';
import { LoadingController } from '@ionic/angular';

@Injectable({
  providedIn: 'root'
})
export class IonLoaderService {

  constructor(private loadingCtrl: LoadingController) { }


  async showLoading(msg: string) {
    const  loading = await this.loadingCtrl.create({
      message : msg,
    }).then((res) => {
      res.present();
    })
    .catch((err) => {
      console.log("Error al crear el loader: " + err);
    });
  }

  async hideLoader() {
    this.loadingCtrl.dismiss()
    .then((res) => {
      console.log("Loading dismissed!", res);
    })
    .catch((err) => {
      console.log("Error", err);
    });
  }

}
