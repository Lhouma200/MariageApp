import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { arLocale,frLocale } from 'ngx-bootstrap/locale';
import { User } from '../_models/user';
import { Router } from '@angular/router';
defineLocale('ar', arLocale);
defineLocale('fr', frLocale);

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();
  user: User;
registerForm:FormGroup;
bsConfig: Partial<BsDatepickerConfig>

locale = 'ar';
locale1 = 'fr';
  constructor( private router: Router,public authService:AuthService, private alertify:AlertifyService,
    private fb : FormBuilder, private localeService: BsLocaleService) { 

      this.authService.lang.subscribe(
        lang=>{
          if(lang=='fr'){
            this.localeService.use(this.locale1);
          }else{
            this.localeService.use(this.locale);
          }
        }
      );
      
    }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red',
      showWeekNumbers: false
    };
 
   
      this.createRegisterForm();
    
  }
  createRegisterForm() {
    this.registerForm = this.fb.group({
      gender:['رجل'],
      username: ['', Validators.required],
      knownAs : ['',Validators.required],
      dateOfBirth:[null,Validators.required],
      city:['',Validators.required],
      country:['',Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator })
  }
    passwordMatchValidator(form:FormGroup){
    return form.get('password').value === form.get('confirmPassword').value ? null : {'mismatch':true};
  }
  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user).subscribe(
        () => { this.alertify.success('تم الإشتراك بنجاح') },
        error => { this.alertify.error(error) },
        () => {
          this.authService.login(this.user).subscribe(
            () => {
              this.router.navigate(['/members']);
            }

          )
        }
      )
    }


  }

  cancel() {

    this.cancelRegister.emit(false);
  }

}
