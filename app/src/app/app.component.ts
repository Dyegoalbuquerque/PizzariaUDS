
import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { SaborService } from './services/sabor.service';
import { Sabor } from './models/Sabor';
import { ItemAdicionalService } from './services/item-adicional.service';
import { ItemAdicional } from './models/ItemAdicional';
import { Pedido } from './models/Pedido';
import { PedidoService } from './services/pedido.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  implements OnInit {
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  threeFormGroup: FormGroup;
  pedido:Pedido;
  sabor:Sabor;
  itemAdicional: ItemAdicional;
  saborList:Sabor[];
  itensAdicionais:ItemAdicional[];
  naoPodeAvancar:boolean;
  selecionouTamanho: boolean;
  selecionouSabor: boolean;

  constructor(private _formBuilder: FormBuilder, private saborService: SaborService, 
              private itemAdicionalService: ItemAdicionalService, private pedidoService: PedidoService) {
      this.pedido = new Pedido();
      this.itemAdicional = new ItemAdicional();
      this.naoPodeAvancar = true;
      this.selecionouTamanho = false;
      this.selecionouSabor = false;
  }

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
    this.saborService.GetAll().subscribe(data=>{this.saborList=data; });    
  } 

 GetAllItensAdicionais(){          
  this.itemAdicionalService.GetAll().subscribe(data=>{this.itensAdicionais=data; });    
 } 

 SelectTamanho(){
  this.selecionouTamanho = true;
 }

 SelectSabor(){
  this.selecionouSabor = true;
 } 

 ClickAvancar(){
   if(this.selecionouTamanho && this.selecionouSabor){
     
    this.naoPodeAvancar = false;

     if(this.pedido.id == undefined || this.pedido.id == 0){
        this.pedidoService.Insert(this.pedido).subscribe(data => {this.pedido = data;});
     }
   }
 }
}