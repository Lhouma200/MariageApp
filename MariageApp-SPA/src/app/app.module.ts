import { BrowserModule } from '@angular/platform-browser';
import { NgModule,Pipe, PipeTransform } from '@angular/core';

import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import {  BsDropdownModule, ButtonsModule, PaginationModule, TabsModule } from 'ngx-bootstrap';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'

import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';





import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvidor } from './_services/error.interceptor';
import { AlertifyService } from './_services/alertify.service';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';

import { appRoutes } from './routes';
import { AuthGuard } from './_guards/auth.guard';
import { UserService } from './_services/user.service';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import { TimeAgoPipe } from 'time-ago-pipe';
import { ListResolver } from './_resolvers/lists.resolver';
import { MessageResolver } from './_resolvers/message.resolver ';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { PaymentComponent } from './payment/payment.component';



@Pipe({
  name: 'timeAgo',
  pure: false
})
export class TimeAgoExtendsPipe extends TimeAgoPipe {}


export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [							
      AppComponent,
   
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      ListsComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent,MemberEditComponent, PhotoEditorComponent,
      TimeAgoExtendsPipe,
      MemberMessagesComponent,
      PaymentComponent
   ],
  imports: [
    BrowserModule,
   
    NgxGalleryModule,
    FileUploadModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
   
   
   
     
     ReactiveFormsModule,
     ButtonsModule.forRoot(),
     PaginationModule.forRoot(),
     BsDatepickerModule.forRoot(),
   BsDropdownModule.forRoot(),
    RouterModule.forRoot(appRoutes),
    TabsModule.forRoot(),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:5112'],
        blacklistedRoutes: ['localhost:5112/auth']
      }
    })
   
   
  ],
  providers: [
    AuthService,
    ErrorInterceptorProvidor,
    AlertifyService,
    AuthGuard,
    PreventUnsavedChangesGuard,
    UserService,
    MemberDetailResolver,
    MemberListResolver,
    MemberEditResolver,
    ListResolver,
    MessageResolver
   
  ],
  bootstrap: [AppComponent],
 
})
export class AppModule { }
