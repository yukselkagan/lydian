import { Router } from '@angular/router';
import { AppResponse } from './../models/app-response';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DataTransferService } from './data-transfer.service';
import { Subscription } from 'rxjs';
import { CommonInformation } from '../models/common-information';


@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private httpClient: HttpClient, private dataTransferService: DataTransferService,
    private router : Router) {

  }

  subscription: Subscription = new Subscription();
  commonInformationSubscription: Subscription = new Subscription();
  commonInformation:CommonInformation = new CommonInformation();

  baseUrl:string = "https://localhost:44301/api";

  postRegisterForm(model:any){
    return this.httpClient.post<any>(this.baseUrl+"/User/Register", model);
  }

  postLoginForm(model:any){
    return this.httpClient.post<any>(this.baseUrl+"/User/Login", model);
  }

  getUserFromApi(){
    return this.httpClient.get<any>(this.baseUrl+"/User/GetUserSelf");
  }

  changeAuthenticationStatus(inputBoolean: boolean){
    let newInformation = this.commonInformation;
    newInformation.isAuthenticated = inputBoolean;
    this.dataTransferService.changeCommonInformation(newInformation);
  }

  public logout(){
    this.removeOldToken();
    this.changeAuthenticationStatus(false);
    this.clearCommonInformation();
    this.router.navigateByUrl('/');
  }

  public removeOldToken(){
    localStorage.removeItem('token');
  }

  public clearCommonInformation(){
    let newInformation = new CommonInformation();
    this.dataTransferService.changeCommonInformation(newInformation);
  }

  getUserInformation(){
    this.getUserFromApi().subscribe({
      next : (response) => {
        let newCommonInformation = new CommonInformation();

        console.log(response);
        newCommonInformation.userId = response['userId'];
        newCommonInformation.email = response['email'];
        this.changeCommonInformation(newCommonInformation);
      },
      error : (message) => {
        console.log(message);
        this.handleInvalidToken();
        //this.logout();
      }
    });
  }

  getToken(): any {
    return localStorage.getItem('token');
  }

  changeCommonInformation(input: CommonInformation){
    let newInformation = this.commonInformation;
    newInformation.isAuthenticated = true;
    newInformation.userId = input.userId;
    newInformation.email = input.email;

    this.dataTransferService.changeCommonInformation(newInformation);
  }

  public handleInvalidToken(){
    let token = localStorage.getItem("token");

    if (this.isTokenExpired(token)) {
      //console.log("token expired");
      this.refreshToken();
    } else {
      this.logout();
    }

  }


  public refreshToken(){
    let accessToken = localStorage.getItem("token");
    let refreshToken = localStorage.getItem("refreshToken");

    let refreshTokenRequest = {'userId': 0, 'accessToken': accessToken,
      'refreshToken': refreshToken}

    this.httpClient.post<any>(this.baseUrl+'/user/refresh', refreshTokenRequest).subscribe({
      next: (response) => {
        //console.log(response);
        let token = response['accessToken'];
        localStorage.setItem('token', token);
        this.getUserInformation();
      },
      error: (errorResponse) => {
        console.log(errorResponse);
        this.logout();
      }
    })

  }


  private isTokenExpired(token: any) {
    const expiration = (JSON.parse(atob(token.split('.')[1]))).exp;
    return (Math.floor((new Date).getTime() / 1000)) >= expiration;
  }




}
