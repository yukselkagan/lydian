import { Router } from '@angular/router';
import { AuthenticationService } from './../../services/authentication.service';
import { LoginForm } from './../../models/login-form';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authenticationService : AuthenticationService, private router : Router) { }

  ngOnInit(): void {
  }

  loginForm : FormGroup = new FormGroup({
    email : new FormControl(null, Validators.required),
    password : new FormControl(null, Validators.required)
  });

  submitLoginForm(){
    this.loginForm.markAllAsTouched();

    let newLoginForm = new LoginForm();
    newLoginForm.email = this.loginForm.controls['email'].value;
    newLoginForm.password = this.loginForm.controls['password'].value;

    let testMode = false;

    if(testMode){
      newLoginForm.email = "tolga";
      newLoginForm.password = "123";
    }

    this.authenticationService.postLoginForm(newLoginForm).subscribe((response) => {
      let token = response['accessToken'];
      let refreshToken = response['refreshToken'];

      localStorage.setItem('token', token);
      localStorage.setItem('refreshToken', refreshToken);

      this.authenticationService.changeAuthenticationStatus(true);
      this.authenticationService.getUserInformation();
      this.router.navigateByUrl("/");
    });

    console.log("login form");
  }


}
