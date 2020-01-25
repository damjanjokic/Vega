import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { pipe } from 'rxjs';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class MakeService {

  constructor(private http:HttpClient) { }

  getMakes(){
    return this.http.get('/api/makes')
    .pipe(map((response: any) => response));
  }
}
