import { Router, ActivatedRoute } from '@angular/router';
import { MethodShareService } from './../../services/method-share.service';
import { AuthenticationService } from './../../services/authentication.service';
import { DataTransferService } from './../../services/data-transfer.service';
import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { CommonInformation } from 'src/app/models/common-information';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  constructor(private dataTransferService : DataTransferService,
    private authenticationService : AuthenticationService,
    private methodShareService : MethodShareService,
    private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {

    this.commonInformationSubscription = this.dataTransferService.currentCommonInformation
      .subscribe((commonData) => this.commonInformation = commonData );

  }

  isAuthenticated : boolean = false;

  subscription: Subscription = new Subscription();
  commonInformationSubscription: Subscription = new Subscription();
  commonInformation:CommonInformation = new CommonInformation();

  searchInput:any = "";

  onSearch(){
    console.log(this.searchInput);
    if(this.searchInput == null || this.searchInput == ""){
      this.router.navigate([], {
        relativeTo: this.activatedRoute,
        queryParams: {
          search: null,
        },
        queryParamsHandling: 'merge',
        skipLocationChange: false
      });
    }else{
      this.router.navigate([], {
        relativeTo: this.activatedRoute,
        queryParams: {
          search: this.searchInput,
        },
        queryParamsHandling: 'merge',
        skipLocationChange: false
      });
    }
  }

  logout(){
    this.authenticationService.logout();
  }

  openSidebar(){
    this.methodShareService.openSidebar();
  }


}
