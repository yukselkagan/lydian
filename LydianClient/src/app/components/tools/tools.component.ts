import { Router } from '@angular/router';
import { MethodShareService } from './../../services/method-share.service';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-tools',
  templateUrl: './tools.component.html',
  styleUrls: ['./tools.component.scss']
})
export class ToolsComponent implements OnInit {
  @ViewChild('lydianSidebar') lydianSidebarElementRef! : ElementRef

  constructor(private methodShareService: MethodShareService, private router: Router) { }

  ngOnInit(): void {

    this.methodShareService.openSidebarEvent.subscribe((inputValue) => {
      //console.log("Transfer value " + inputValue);
      this.openSidebar();
    });

  }


  closeSidebar(){
    this.lydianSidebarElementRef.nativeElement.classList.remove('sidebar-show');
  }

  openSidebar(){
    this.lydianSidebarElementRef.nativeElement.classList.add('sidebar-show');
  }

  navigateWithSidebar(target:string){
    this.closeSidebar();
    this.router.navigateByUrl(target);
  }


}
