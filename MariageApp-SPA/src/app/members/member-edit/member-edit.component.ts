import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm
  @ViewChild('editForm1') editForm1: NgForm
  user: User
  created:string;
  age:string;
  lastActive :string;
  options :  Intl.DateTimeFormatOptions = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };

  photoUrl:string;
  @HostListener('window:beforeunload',['$event'])
 unLoadNotification($event:any){
   if(this.editForm.dirty){
     $event.returnValue=true;
   }
 }
  constructor(private route: ActivatedRoute, private alertify: AlertifyService,private userService:UserService,
    public authService:AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.authService.currentPhotoUrl.subscribe(photoUrl=>this.photoUrl=photoUrl);
    this.created = new Date(this.user.created).toLocaleString('ar-EG',this.options).replace('،','');
    this.age = this.user.age.toLocaleString('ar-EG');
    this.authService.lang.subscribe(
      lang=>{
    if(lang=='fr'){
       this.age = this.user.age.toLocaleString('fr');
    }
    else{
      this.age = this.user.age.toLocaleString('ar-EG');
    }
    if(lang=='fr'){
      this.created = new Date(this.user.created)
      .toLocaleString('fr', this.options)
      .replace('،', '');
   }
   else{
    this.created = new Date(this.user.created)
    .toLocaleString('ar-EG', this.options)
    .replace('،', '');
   }
   if(lang=='fr'){
    this.lastActive = new Date(this.user.lastActive)
    .toLocaleTimeString('fr', this.options)
    .replace('،', '');
 }
 else{
  this.lastActive = new Date(this.user.lastActive)
  .toLocaleString('ar-EG', this.options)
  .replace('،', '');
 }

      }

      );
  }
  updateUser() {
    
    
    this.userService.updateUser(this.authService.decodedToken.nameid,this.user).subscribe(()=>{
     this.alertify.success('تم تعديل الملف الشخصي بنجاح');
     this.editForm.reset(this.user);
     this.editForm1.reset(this.user);
    },error=>this.alertify.error(error))
         
   }
   updateMainPhoto(photoUrl){
    this.user.photoURL=photoUrl;
  }
 

}
