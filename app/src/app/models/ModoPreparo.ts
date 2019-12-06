import {Sabor} from './Sabor';
import { ItemPreco} from './ItemPreco';

export class ModoPreparo{    
     id: number;      
     sabor: Sabor;
     itemPreco: ItemPreco;
     descricao: string;
     tamanho: number;
     tempoDePreparo: number;
     modoPreparoIngredientes:[]

     constructor(){
          this.sabor = new Sabor();
          this.itemPreco = new ItemPreco();
     }
}