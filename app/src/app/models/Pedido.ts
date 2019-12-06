import { ModoPreparo } from './ModoPreparo';
import { ItemAdicional } from './ItemAdicional';

export class Pedido{    
    id: number;      
    data: Date;      
    status: number;
    valor: number;
    tempoPreparo: number;
    modoPreparo: ModoPreparo;
    quantidade: number;
    itensAdicionais: ItemAdicional[];

    constructor(){
        this.modoPreparo = new ModoPreparo();
        this.itensAdicionais = [];
    }
}