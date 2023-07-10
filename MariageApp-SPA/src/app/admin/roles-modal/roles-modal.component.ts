import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {
  @Output() updateSelectedRoles = new EventEmitter();
  user:User;
  roles:any[];
 
  constructor(public bsModalRef: BsModalRef, public authService:AuthService) {}

  ngOnInit() {
  }
  
  updateRoles(){
    this.updateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }
}