import { OrderService } from './../../services/order.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-payment-success',
  templateUrl: './payment-success.component.html',
  styleUrls: ['./payment-success.component.scss']
})
export class PaymentSuccessComponent implements OnInit {

  constructor(private activatedRoute : ActivatedRoute, private orderService: OrderService) { }

  ngOnInit(): void {
    this.paymentId = this.activatedRoute.snapshot.paramMap.get('id') as string;
    this.activatedRoute.queryParamMap.subscribe(params => {
      this.paymentId = params.get('id');
      this.completePayment(this.paymentId);
    });

  }

  paymentId:any = "0";
  paymentCompleted = false;


  completePayment(code:any){
    this.orderService.confirmPayment(code).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (errorResponse) => {
        console.log(errorResponse);
      }
    })
  }



}
