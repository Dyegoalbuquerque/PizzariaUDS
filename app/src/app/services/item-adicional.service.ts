import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ItemAdicional } from '../models/ItemAdicional';

@Injectable({
  providedIn: 'root'
})
export class ItemAdicionalService{

  ApiUrl='http://localhost:5001/Api/ItemAdicional';    
  constructor(private httpclient: HttpClient) { }    
    
  GetAll():Observable<ItemAdicional[]>{    
  return this.httpclient.get<ItemAdicional[]>(this.ApiUrl);    
  } 
}