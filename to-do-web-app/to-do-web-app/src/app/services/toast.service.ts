import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})

export class ToastService {

    isDisplayed: boolean = false;

    showMessage(message: string) {

        if(!this.isDisplayed) {
            document.getElementById('notification-window').style.minHeight = '10ex';
            document.getElementById('notification-message').innerHTML = message;
            window.setTimeout(() => this.hideMessage(), 3000);
            this.isDisplayed = true;
        }
    } 

    hideMessage() {
        if(this.isDisplayed) {
            document.getElementById('notification-window').style.minHeight = '0';
            this.isDisplayed = false;
        }
        
    }

}