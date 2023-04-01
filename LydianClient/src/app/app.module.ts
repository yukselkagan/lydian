import { TokenInterceptor } from './interceptors/token.interceptor';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToolsComponent } from './components/tools/tools.component';
import { AttachmentComponent } from './components/attachment/attachment.component';
import { BrainComponent } from './components/brain/brain.component';
import { TemplateVariableDirective } from './directives/template-variable.directive';
import { AdminComponent } from './components/admin/admin.component';
import { ProductComponent } from './components/product/product.component';
import { CartComponent } from './components/cart/cart.component';
import { OrdersComponent } from './components/orders/orders.component';
import { PaymentSuccessComponent } from './components/payment-success/payment-success.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    ToolsComponent,
    AttachmentComponent,
    BrainComponent,
    TemplateVariableDirective,
    AdminComponent,
    ProductComponent,
    CartComponent,
    OrdersComponent,
    PaymentSuccessComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    { provide : HTTP_INTERCEPTORS, useClass : TokenInterceptor, multi : true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
