import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(private httpClient: HttpClient) { }

  baseUrl:string = "https://localhost:44301/api";

  addItemToCart(model:any){
    return this.httpClient.post<any>(this.baseUrl+'/cart/cart-item', model);
  }

  getCartItems(){
    return this.httpClient.get<any>(this.baseUrl+'/cart')
  }



}
