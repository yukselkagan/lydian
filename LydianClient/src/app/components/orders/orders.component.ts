import { Router } from '@angular/router';
import { OrderService } from './../../services/order.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {

  constructor(private orderService: OrderService, private router: Router) { }

  ngOnInit(): void {

    this.getOrder();

  }

  orders = [];

  getOrder(){
    this.orderService.getOrders().subscribe({
      next: (response) => {
        console.log(response);
        this.orders = response;
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
        console.log(response);
        let paymentLink = response['data'];
        window.open(paymentLink, '_blank');
        console.log(paymentLink);
      },
      error: (errorResponse) => {
        console.log(errorResponse);
      }
    })
  }


}
