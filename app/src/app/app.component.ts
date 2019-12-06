
import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { SaborService } from './services/sabor.service';
import { Sabor } from './models/Sabor';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  implements OnInit {
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  threeFormGroup: FormGroup;
  SaborList:Sabor[];

  constructor(private _formBuilder: FormBuilder, private saborService: SaborService) {}

  ngOnInit() {
    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: ['', Validators.required]
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
    this.threeFormGroup = this._formBuilder.group({
      threeCtrl: ['', Validators.required]
    });

    this.GetAllSabores();
  }

  GetAllSabores(){          
    this.saborService.GetSabores().subscribe(data=>{this.SaborList=data; console.log(this.SaborList);});    
 } 
}