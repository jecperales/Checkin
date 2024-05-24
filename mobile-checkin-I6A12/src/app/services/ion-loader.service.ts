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
      //duration : 5000
    }).then((res) => {
      res.present();
    });
    //loading.present();
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
