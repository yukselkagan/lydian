import { ActivatedRoute } from '@angular/router';
import { ProductService } from './../../services/product.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private productService : ProductService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.categoryName = this.activatedRoute.snapshot.paramMap.get('categoryName') as string;

    this.activatedRoute.queryParamMap.subscribe(params => {
      this.searchInput = params.get('search');
      if(this.searchInput != null){
        this.filterBySearchInput(this.searchInput);
      }else{
        this.filterBySearchInput("");
      }
      //this.completePayment(this.paymentId);
      console.log(this.searchInput);
    });

    console.log(this.categoryName);

    if(this.categoryName){
      this.productService.getProductsByCategoryName(this.categoryName).subscribe({
        next: (response) => {
          //console.log(response);
          this.productList = response;
          this.unfilteredProductList = this.productList;
          let count = this.productList.length;
          this.calculateVerticalPart(count);
        },
        error: (errorResponse) => {
          console.log(errorResponse);
        }
      })

    }else{
      this.productService.getAllProduct().subscribe((response) => {
        this.productList = response;
        this.unfilteredProductList = this.productList;
        let count = this.productList.length;
        this.calculateVerticalPart(count);
      });
    }




  }

  categoryName = "";
  searchInput:any = "";

  productList:any = [];
  unfilteredProductList:any = [];


  isAuthenticated:boolean = false;

  horizontalItemNumber: number = 4;


  temparray = [1,2,3,4,5];

  horizontalListing = [1,2,3,4];

  verticalParts: any = [];
  horizontalParts = [1,2,3,4];

  filterForm: FormGroup = new FormGroup({
    priceRange: new FormControl('0')
  })

  filterBySearchInput(searchInput:any){
    let unfilteredList = this.unfilteredProductList;
    this.productList = unfilteredList.filter( (item:any) => {
      if(item['productName'].toLowerCase().includes(searchInput.toLowerCase()) ){
        return true
      }else{
        return false;
      }
    });
  }

  submitFilterForm(){
    let priceRangeId = this.filterForm.controls['priceRange'].value;
    let unfilteredList = this.unfilteredProductList;

    let filteredProducts = unfilteredList.filter((item:any) => {
      if(priceRangeId == 1){
        if(item['price'] <= 10){
          return true;
        }else{
          return false;
        }
      }else if(priceRangeId == 2){
        if(item['price'] > 10 && item['price'] <= 20 ){
          return true;
        }else{
          return false;
        }
      }else if(priceRangeId == 3){
        if(item['price'] > 20 ){
          return true;
        }else{
          return false;
        }
      }else{
        return true;
      }

    });

    this.productList = filteredProducts;
  }


  calculateVerticalPart(count: any){
    let verticalPartNumber = 1;
    if(count > 0){
      verticalPartNumber = Math.ceil(count / this.horizontalItemNumber);
    }

    this.verticalParts = [];
    for(let i=1; i <= verticalPartNumber; i++){
      this.verticalParts.push(1);
    }
  }

  controlItemExists(targetIndex : number){
    if(typeof this.productList[targetIndex] === 'undefined'){
      return false;
    }else{
      return true;
    }
  }

  processImagePath(image:any){
    let fullPath = 'https://localhost:44301/ProductImages/'+image;
    return fullPath;
  }



}
