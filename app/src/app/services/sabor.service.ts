import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Sabor } from '../models/Sabor';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SaborService {

  ApiUrl='http://localhost:5001/Api/Sabor';    
  constructor(private httpclient: HttpClient) { }    
    
  GetAll():Observable<Sabor[]>{    
  return this.httpclient.get<Sabor[]>(this.ApiUrl);    
  } 
}
