import { Component, OnInit } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { LEADING_TRIVIA_CHARS } from '@angular/compiler/src/render3/view/template';
import { getMissingNgModuleMetadataErrorData } from '@angular/compiler';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating App';
  users:any;
  constructor( private http:HttpClient){}
  ngOnInit():void{
   this.getUser();
   
  }
    getUser(){
      this.http.get('https://localhost:5001/api/users').subscribe(response=>{
        this.users=response;
      },error=>{console.log(error);
      })

    }
    
  
}
