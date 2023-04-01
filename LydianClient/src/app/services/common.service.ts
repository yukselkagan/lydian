import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor() { }

  showToast(message:string, type:string="normal"){
    if(type == "error"){
      document.getElementById("generalToast")?.classList.remove("bg-primary");
      document.getElementById("generalToast")?.classList.add("bg-danger");
    }else{
      document.getElementById("generalToast")?.classList.remove("bg-primary");
      document.getElementById("generalToast")?.classList.add("bg-lydian");
    }

    document.getElementById("generalToast")?.classList.add("show");
    let toastTextElement = document.getElementById("generalToastText") as HTMLInputElement;
    toastTextElement.innerHTML = message;
  }

  public createForLoopArray(size: any){
    return new Array(size);
  }


}
