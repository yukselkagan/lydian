import { Router } from '@angular/router';
import { AuthenticationService } from './../../services/authentication.service';
import { CommonService } from './../../services/common.service';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { RegisterForm } from 'src/app/models/register-form';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private commonService : CommonService,
    private authenticationService : AuthenticationService, private router : Router) { }

  ngOnInit(): void {
  }

  checkPasswords: ValidatorFn = (group: AbstractControl):  ValidationErrors | null => {
    let password = group.get('password')?.value;
    let confirmPassword = group.get('passwordConfirm')?.value
    if(password == confirmPassword){
      return null;
    }else{
      return { notSame: true };
    }
  }

  registerForm : FormGroup = new FormGroup({
    email : new FormControl(null, Validators.required),
    password : new FormControl(null, Validators.required),
    passwordConfirm : new FormControl(null, Validators.required),
  },
  {
    validators : this.checkPasswords
  });

  submitRegisterForm(){
    this.registerForm.markAllAsTouched();

    let newRegisterForm = new RegisterForm();
    newRegisterForm.email = this.registerForm.controls['email'].value;
    newRegisterForm.password = this.registerForm.controls['password'].value;

    let testMode = false;

    if(testMode){
      newRegisterForm.email = "jack";
      newRegisterForm.password = "123";
    }


    console.log(this.registerForm);

    //console.log(this.registerForm.errors);
    //console.log(newRegisterForm.email);
    //console.log(this.registerForm.valid);


    if(this.registerForm.valid || testMode){
      this.authenticationService.postRegisterForm(newRegisterForm).subscribe({
        next : (response) => {
          let token = response['accessToken'];
          localStorage.setItem('token', token);
          this.authenticationService.changeAuthenticationStatus(true);
          this.authenticationService.getUserInformation();
          this.router.navigateByUrl("/");
        },
        error : (error) => {
          this.commonService.showToast(error.error);
        }
      });
    }else{
      this.commonService.showToast("Need to complete form");
    }


  }



}
