import { AfterViewInit, Directive, ElementRef, OnInit } from '@angular/core';
// @ts-ignore
import * as words from '../../assets/jsons/dictionary.json'
import { AuthService } from '../_services/auth.service';

@Directive({
  selector: '[lang]'
})
export class LangDirective implements OnInit, AfterViewInit  {
  _words = [];

  constructor(private ref : ElementRef,private authService:AuthService) { }
 
  ngOnInit(): void {
   this._words =words.default;
  }
  ngAfterViewInit() {
    this.authService.lang.subscribe(
      lang=>{
        if(lang == 'fr'){
          try{
              var word = this._words.filter(word=>word['ar'].match(this.ref.nativeElement.innerText));
              if(word[0]['ar']==this.ref.nativeElement.innerText)
              this.ref.nativeElement.innerText = word[0]['fr'];
          }catch{}
        }

        if(lang == 'ar'){
          try{
              var word = this._words.filter(word=>word['fr'].match(this.ref.nativeElement.innerText));
              if(word[0]['fr']==this.ref.nativeElement.innerText)
              this.ref.nativeElement.innerText = word[0]['ar'];
          }catch{}
        }
      }
    )
  }

}


