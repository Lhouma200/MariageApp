import { Component, OnInit, ViewChild, AfterViewChecked } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit , AfterViewChecked {
  
  @ViewChild('memberTabs',  { static: true }) memberTabs: TabsetComponent;
  @ViewChild('memberTabs1',  { static: true }) memberTabs1: TabsetComponent;

  user: User;
  created:string;
  age:string;
  showIntro:boolean=true;
  showLook:boolean=true;
  paid:boolean=false;
  options: Intl.DateTimeFormatOptions = {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  };

  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  constructor(private userService: UserService,public authService:AuthService, private alertify: AlertifyService,
    private route: ActivatedRoute) { }

  ngAfterViewChecked(): void {
    setTimeout(() => {
      this.paid = this.authService.paid;
    }, 0);
  }

  ngOnInit() {
    // this.loadUser();
    this.paid = this.authService.paid;
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.route.queryParams.subscribe(
     params=>{
       const selectedTab = params['tab'];
       this.memberTabs.tabs[selectedTab>0?selectedTab:0].active=true;
       this.memberTabs1.tabs[selectedTab>0?selectedTab:0].active=true;
     }
    )

    this.galleryOptions=[{
      width:'500px',height:'500px',imagePercent:100,thumbnailsColumns:4,
      imageAnimation:NgxGalleryAnimation.Slide,preview:false
    }]

    this.galleryImages=this.getImages();
    this.created = new Date(this.user.created).toLocaleString('ar-EG',this.options).replace('،','');
    this.age = this.user.age.toLocaleString('ar-EG');
    this.showIntro=true;
    this.showLook=true;
    this.authService.lang.subscribe(
      lang=>{
        if(lang=='fr'){
         this.age= this.user.age.toString()
        }else{
   
        }
        if(lang=='fr'){
         this.created=this.user.created.toString()
         }else{
         
         }
      }
    );
    

  }
  selectTab(tabId:number){
    this.memberTabs.tabs[tabId].active=true;
  }
  selectTab1(tabId:number){
    this.memberTabs1.tabs[tabId].active=true;
  }
  getImages(){
    const imageUrls=[];
    for(let i =0;i<this.user.photos.length;i++){
      imageUrls.push({
        small:this.user.photos[i].url,
        medium:this.user.photos[i].url,
        big:this.user.photos[i].url,
      })
    };
    return imageUrls;
  }

  deselect(){
this.authService.hubConnection.stop()

  }
  sendLike(id:number){
    this.userService.sendLike(this.authService.decodedToken.nameid,id).subscribe(
      ()=>{this.alertify.success('لقد قمت بالإعجاب بـ'+this.user.knownAs);},
      error=>{this.alertify.error(error);}
    )
  }

  // loadUser(){
  //   this.userService.getUser(+this.route.snapshot.params['id']).subscribe(
  //     (user:User)=>{this.user=user},
  //     error=>{this.alertify.error(error)}
  //   )
  // }



}