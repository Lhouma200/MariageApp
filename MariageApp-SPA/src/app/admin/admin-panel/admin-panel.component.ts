import { Component, OnInit, ViewChild } from '@angular/core';
import { AllMembersReportComponent } from 'src/app/_reports/all-members-report/all-members-report.component';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  @ViewChild('report') report:AllMembersReportComponent;
  @ViewChild('report1') report1:AllMembersReportComponent;


  constructor(public authService :AuthService) { }

  ngOnInit(): void {
  }
  printAll(){
    this.report.printAll();
  }
  printAll1(){
    this.report1.printAll1();
  }


}
