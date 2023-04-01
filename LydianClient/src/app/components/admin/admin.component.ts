import { Router } from '@angular/router';
import { CommonService } from './../../services/common.service';
import { ProductService } from './../../services/product.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormControl, Validators, ValidatorFn, ValidationErrors, AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  @ViewChild('productImage') productImageInputElement! : HTMLInputElement

  constructor(private productService: ProductService, private commonService : CommonService,
    private router: Router) { }

  ngOnInit(): void {
    console.log(this.productImage);
  }


  productForm : FormGroup = new FormGroup({
    productName: new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(20)]),
    price : new FormControl(0, [Validators.required, Validators.min(0.1), Validators.max(1000)]),
    description: new FormControl(null, Validators.required),
    categoryId: new FormControl(1, Validators.required)
  });


  submitProductForm(){
    const formData = new FormData();
    this.productForm.markAllAsTouched();
    this.fileInputTouched = true;

    if(this.productForm.valid){
      formData.append('productName', this.productForm.get('productName')?.value );
      formData.append('price', this.productForm.controls['price'].value );
      formData.append('description', this.productForm.controls['description'].value );
      formData.append('productImage', this.productImage, this.productImage.name);
      formData.append('categoryId', this.productForm.controls['categoryId'].value);

      //inspect
      for (var pair of formData.entries() ) {
        console.log(pair[0]+ ', ' + pair[1]);
      }

      this.productService.createProduct(formData).subscribe({
        next: (response) => {
          this.router.navigateByUrl("/");
        },
        error : (error) => {
          console.log(error);
        }
      })

    }else{
      this.commonService.showToast("Need to complete form");
    }


  }

  productImage!: File;
  fileInputTouched :boolean = false;

  onFileChange(event:any){
    this.productImage = event.target.files[0];
    this.fileInputTouched = true;
  }


}
