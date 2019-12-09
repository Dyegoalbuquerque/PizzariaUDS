import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ModoPreparo } from '../models/ModoPreparo';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModoPreparoService {

  ApiUrl='http://localhost:5001/Api/ModoPreparo';    
  constructor(private httpclient: HttpClient) { }    
    
  GetAll():Observable<ModoPreparo[]>{    
  return this.httpclient.get<ModoPreparo[]>(this.ApiUrl);    
  } 
}
