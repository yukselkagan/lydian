import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private httpClient: HttpClient) { }

  baseUrl:string = "https://localhost:44301/api";

  createOrder(model:any){
    return this.httpClient.post<any>(this.baseUrl+'/order', null);
  }

  getOrders(){
    return this.httpClient.get<any>(this.baseUrl+'/order')
  }

  orderPayment(model: any){
    return this.httpClient.post<any>(this.baseUrl+'/order/payment', model);
  }

  confirmPayment(code:any){
    return this.httpClient.post<any>(this.baseUrl+`/order/payment/confirm/${code}`, null);
  }



}
