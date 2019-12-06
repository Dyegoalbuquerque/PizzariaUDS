
import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { SaborService } from './services/sabor.service';
import { Sabor } from './models/Sabor';
import { ItemAdicionalService } from './services/item-adicional.service';
import { ItemAdicional } from './models/ItemAdicional';
import { Pedido } from './models/Pedido';
import { PedidoService } from './services/pedido.service';
import { ItemPreco } from './models/ItemPreco';


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
  saborList:Sabor[];
  itensAdicionais:ItemAdicional[];
  naoPodeAvancar:boolean;
  selecionouTamanho: boolean;
  selecionouSabor: boolean;
  itensAdicionaisCheck: boolean[];

  constructor(private _formBuilder: FormBuilder, private saborService: SaborService, 
              private itemAdicionalService: ItemAdicionalService, private pedidoService: PedidoService) {
      this.pedido = new Pedido();
      this.naoPodeAvancar = true;
      this.selecionouTamanho = false;
      this.selecionouSabor = false;
      this.itensAdicionaisCheck = [];
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
  this.itemAdicionalService.GetAll().subscribe(data=>{
    this.itensAdicionais=data;

    for(var i = 0; i < this.itensAdicionais.length; i++){
      this.itensAdicionaisCheck.push(false);
    }
  });    
 } 

 ShowValorPizza(): number {
   let valorAdicionais = 0;
   for(var i =0; i < this.pedido.itensAdicionais.length; i++)
   {
     let item = this.pedido.itensAdicionais[i];
     if(item.itemPreco != null){
        valorAdicionais += item.itemPreco.valor;
     }
   }
  return this.pedido.valor != null ? this.pedido.valor - valorAdicionais : 0;
}

ShowNomePizza(): string{

  if(this.saborList != null){
    this.sabor = this.saborList.filter((sabor) => {
      return sabor.id === this.pedido.modoPreparo.sabor.id;
    })[0];
  }

  if(this.sabor != null){
      return this.sabor.nome;
  }

  return "";
}

ShowTamanhoPizza(): string{
     if(this.pedido.modoPreparo != null){

        if(this.pedido.modoPreparo.tamanho == 1){
          return "Grande";
        }else if(this.pedido.modoPreparo.tamanho == 2){
          return "Média"
        }else {
          return "Pequena";
        }
     }

     return "";
}

ShowValorAdicional(item: ItemAdicional): number {
  return item.itemPreco != null ? item.itemPreco.valor : 0;
}

 SelectTamanho(){
  this.selecionouTamanho = true;
 }

 SelectSabor(){
  this.selecionouSabor = true;
 } 

 GetDetalhesPedido(id : number){
   this.pedidoService.GetById(id).subscribe(data=>{ this.pedido=data });  
 }

 ClickAvancarMontarPizza(){
 
   if(this.selecionouTamanho && this.selecionouSabor){
     
    this.naoPodeAvancar = false;

     if(this.pedido.id == undefined || this.pedido.id == 0){
        this.pedidoService.Insert(this.pedido).subscribe(data => {this.pedido = data;});
     }
   } 
 }

 ClickAvancarPersonalizarPizza(){
  
      for(var i =0; i < this.itensAdicionaisCheck.length; i++){

        if(this.itensAdicionaisCheck[i]){
         let item =new ItemAdicional();
          item.id = this.itensAdicionais[i].id;
          this.pedido.itensAdicionais.push(item);
        }
      };
   
      this.pedidoService.Update(this.pedido).subscribe(data => {
        this.pedido = data;
        this.GetDetalhesPedido(this.pedido.id);
      });   
  }

  Reload(){
    location.reload();
  }
}