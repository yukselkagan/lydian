import { CartService } from './../../services/cart.service';
import { ProductService } from './../../services/product.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from 'src/app/models/product';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit {

  constructor(private activatedRoute : ActivatedRoute, private productService: ProductService,
    private cartService: CartService, private router: Router) { }

  ngOnInit(): void {
    this.productId = this.activatedRoute.snapshot.paramMap.get('productId') as string;
    this.getProductInformation(this.productId);
  }

  productId: any = 5;
  product: Product = new Product();
  public selectedQuantity: number = 1;
  totalPrice = 0;

  getProductInformation(productId: any){
    this.productService.getProduct(productId).subscribe({
      next : (response) => {
        console.log(response);
        this.product = response;
        this.quickTotalPrice();
      },
      error : (error) => {
        console.log(error);
      }
    })
  }


  public createForLoopArray(size: any){
    return new Array(size);
  }

  public quickTotalPrice(){
    this.totalPrice = this.product.price * this.selectedQuantity;
  }

  public changeQuantity(event: any){
    this.totalPrice = this.product.price * event;
  }


  public addToCart(){
    var postData = { 'productId': this.productId, 'quantity': this.selectedQuantity }

    this.cartService.addItemToCart(postData).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigateByUrl('/cart');
      },
      error: (errorResponse) => {
        console.log(errorResponse)
      }
    })

  }

  processImagePath(image:any){
    let defaultPath = 'assets/images/img2.jpg';

    let fullPath = 'https://localhost:44301/ProductImages/'+image;
    return fullPath;
  }




}
