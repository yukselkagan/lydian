import { EventEmitter, Injectable } from '@angular/core';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MethodShareService {

  constructor() { }

  openSidebarEvent = new EventEmitter();
  openSidebarSubscription: Subscription = new Subscription();


  openSidebar(){
    console.log("global sidebar");
    this.openSidebarEvent.emit("Test value");


  }


}
