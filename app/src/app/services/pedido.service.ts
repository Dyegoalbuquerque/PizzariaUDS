import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Pedido } from '../models/Pedido';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PedidoService {

  ApiUrl='http://localhost:5001/Api/Pedido';    
  constructor(private httpclient: HttpClient) { }    
    
  GetAll():Observable<Pedido[]>{    
    return this.httpclient.get<Pedido[]>(this.ApiUrl);    
  }    
    
  GetById(Id:number):Observable<Pedido>{    
    return this.httpclient.get<Pedido>(this.ApiUrl+'/'+Id);    
  }    
  Insert(pedido:Pedido){    
    return this.httpclient.post<Pedido>(this.ApiUrl,pedido);    
  }    
    
  Update(pedido:Pedido):Observable<Pedido>{    
    return this.httpclient.put<Pedido>(this.ApiUrl,pedido);    
  }    
    
  Delete(Id:string){    
    return this.httpclient.delete(this.ApiUrl+'/'+Id);    
  }

  Validate(pedido: Pedido): boolean{
    return pedido.modoPreparo.sabor.id > 0 && pedido.modoPreparo.tamanho > 0;   
  }
}
