import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppResponse } from '../models/app-response';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private httpClient : HttpClient) { }

  baseUrl:string = "https://localhost:44301/api";

  getAllProduct(){
    return this.httpClient.get<AppResponse>(this.baseUrl+"/Product/GetAll");
  }

  createProduct(model:FormData){
    return this.httpClient.post<any>(this.baseUrl+'/Product/AddProduct', model);
  }

  public getProduct(productId:any){
    return this.httpClient.get<any>(this.baseUrl+'/Product/Get/' + productId);
  }

  public getProductsByCategoryId(categoryId:any){
    let paramsForRequest = new HttpParams().set('categoryId', categoryId);
    return this.httpClient.get<any>(this.baseUrl+'Product/GetByCategory', {params: paramsForRequest});
  }

  public getProductsByCategoryName(categoryName:any){
    let paramsForRequest = new HttpParams().set('categoryName', categoryName);
    return this.httpClient.get<any>(this.baseUrl+'/Product/GetByCategoryName', {params: paramsForRequest});
  }


}
