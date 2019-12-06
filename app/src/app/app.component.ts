
import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { SaborService } from './services/sabor.service';
import { Sabor } from './models/Sabor';
import { ItemAdicionalService } from './services/item-adicional.service';
import { ItemAdicional } from './models/ItemAdicional';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  implements OnInit {
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  threeFormGroup: FormGroup;
  saborList:Sabor[];
  itensAdicionais:ItemAdicional[];

  constructor(private _formBuilder: FormBuilder, private saborService: SaborService, 
              private itemAdicionalService: ItemAdicionalService) {}

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
    this.GetAllItensAdicionais();
  }

  GetAllSabores(){          
    this.saborService.GetAll().subscribe(data=>{this.saborList=data; console.log(this.saborList);});    
 } 

 GetAllItensAdicionais(){          
  this.itemAdicionalService.GetAll().subscribe(data=>{this.itensAdicionais=data; console.log(this.itensAdicionais);});    
} 
}