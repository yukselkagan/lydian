import { OrderService } from './../../services/order.service';
import { CartItem } from './../../models/cart-item';
import { CartService } from './../../services/cart.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {

  constructor(private cartService: CartService, private orderService: OrderService) { }

  ngOnInit(): void {

    this.getCartItems();

  }

  cartItems: CartItem[] = [];

  productsPrice:number = 0;
  deliveryPrice:number = 0;
  totalPrice:number = 0;

  getCartItems(){
    this.cartService.getCartItems().subscribe({
      next: (response) => {
        //console.log(response);
        this.cartItems = response['cartItems'];
        this.calculatePrices();
      },
      error: (errorResponse) => {
        console.log(errorResponse);
      }
    })
  }

  calculatePrices(){
    let productsPrice = 0;
    for (let index = 0; index < this.cartItems.length; index++) {
      const item = this.cartItems[index];
      productsPrice += item.product.price * item.quantity;
    }
    this.productsPrice = productsPrice;
    this.totalPrice = this.productsPrice + this.deliveryPrice;
  }

  placeOrder(){
    this.orderService.createOrder(null).subscribe({
      next: (response) => {
        console.log(response);
        this.payOrder(response['orderId']);
      },
      error: (errorResponse) => {
        console.log(errorResponse);
      }
    })
  }


  payOrder(orderId: number){
    var postData = { 'orderId': orderId };
    this.orderService.orderPayment(postData).subscribe({
      next: (response) => {
        let paymentLink = response['data'];
        window.open(paymentLink, "_self");
      },
      error: (errorResponse) => {
        console.log(errorResponse);
      }
    })
  }


  processImagePath(image:any){
    let fullPath = 'https://localhost:44301/ProductImages/'+image;
    return fullPath;
  }









}
