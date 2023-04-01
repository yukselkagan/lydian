import { PaymentSuccessComponent } from './components/payment-success/payment-success.component';
import { OrdersComponent } from './components/orders/orders.component';
import { ProductComponent } from './components/product/product.component';
import { AdminComponent } from './components/admin/admin.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CartComponent } from './components/cart/cart.component';
import { AuthenticationGuard } from './guards/authentication.guard';

const routes: Routes = [
  {path : 'home', component : HomeComponent},
  {path : 'login', component : LoginComponent},
  {path : 'register', component : RegisterComponent},
  {path : 'admin', component : AdminComponent},
  {path : 'featured', component : HomeComponent},
  {path : 'category/:categoryName', component : HomeComponent},
  {path : 'cart', component : CartComponent, canActivate: [AuthenticationGuard]},
  {path : 'orders', component : OrdersComponent, canActivate: [AuthenticationGuard]},
  {path : 'payment-success', component : PaymentSuccessComponent},
  {path : 'product', component : ProductComponent},
  {path : 'product/:productId', component : ProductComponent},
  {path : '', component : HomeComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
