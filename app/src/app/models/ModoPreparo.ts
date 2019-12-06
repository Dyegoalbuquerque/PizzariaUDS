import {Sabor} from './Sabor';
import { ItemPreco} from './ItemPreco';

export class ModoPreparo{    
     Id: number;      
     Sabor: Sabor;
     ItemPreco: ItemPreco;
     Descricao: string;
     Tamanho: number;
     TempoDePreparo: number;
     ModoPreparoIngredientes:[]
}