import { Injectable } from '@angular/core';
import { City } from '../models/city';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})

export class CitiesService {
  cities: City[] = [];

  constructor(private httpClient: HttpClient) {
    
  }

  public getCities(): Observable<City[]> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", "Bearer token");

    return this.httpClient.get<City[]>("https://localhost:7221/api/v1/cities", { headers: headers });
  } 
}
