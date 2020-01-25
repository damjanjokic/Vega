import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FeatureService {

  constructor(private http: HttpClient) { }

  getFeatures(){
    return this.http.get('/api/features')
    .pipe(map((response: any) => response));
  }
}
