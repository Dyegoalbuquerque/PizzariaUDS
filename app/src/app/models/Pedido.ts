import { ModoPreparo } from './ModoPreparo';
import { ItemAdicional } from './ItemAdicional';

export class Pedido{    
    Id: number;      
    Data: Date;      
    Status: number;
    Valor: number;
    TempoPreparo: number;
    ModoPreparo: ModoPreparo;
    Quantidade: number;
    ItensAdicionais: ItemAdicional[];
}