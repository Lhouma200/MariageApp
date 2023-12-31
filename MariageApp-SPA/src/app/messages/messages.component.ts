import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination, PaginationResult } from '../_models/Pagination';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  messageType = 'Unread';

  constructor(private userService:UserService,public authService:AuthService,private route:ActivatedRoute,private alertify:AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(
      data=>{
        this.messages = data['messages'].result;
        this.pagination = data['messages'].pagination;
      }
    )
  }
  loadMessages(){
    this.userService.getMessages(this.authService.decodedToken.nameid,this.pagination.currentPage,this.pagination.itemsPerPage,this.messageType).subscribe(
      (res:PaginationResult<Message[]>)=>{
        this.messages = res.result;
        this.pagination = res.pagination;
      },
      error=>this.alertify.error(error)
    )
  }

  pageChanged(event:any):void{
    this.pagination.currentPage= event.page;
    this.loadMessages();
  }
  deleteMessage(id:number){
    this.alertify.confirm('هل أنت متأكد من حذف تلك الرسالة',()=>{
      this.userService.deleteMessage(id,this.authService.decodedToken.nameid).subscribe(
        ()=>{
        this.messages.splice(this.messages.findIndex(m=>m.id==id),1);
        this.alertify.success('تم حذف الرسالة بنجاح');
      }
      ,error=>this.alertify.error(error)
      );
    });
  }

}
