import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {  pipe } from 'rxjs';
import { map } from 'rxjs/operators';
import { SaveVehicle } from '../models/saveVehicle';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {

  private readonly vehicleEndpoint = '/api/vehicles/';

  constructor(private http: HttpClient) {

   }


   create(vehicle){
     return this.http.post(this.vehicleEndpoint, vehicle)
     .pipe(map((response: any) => response));
   }

   getVehicles(filter){
     return this.http.get(this.vehicleEndpoint + '?' + this.toQueryString(filter))
     .pipe(map((response:any) => response))
   }

   toQueryString(obj){
    // prop = value &
    var parts = [];
    for(var property in obj){
      var value = obj[property];
      if(value != null && value != undefined)
        parts.push(encodeURIComponent(property)+'='+encodeURIComponent(value));
    }
    console.log(parts.join('&'));
    return parts.join('&');
   }

   getVehicle(id){
     return this.http.get(this.vehicleEndpoint+id)
     .pipe(map((response:any) => response));
   }

   update(vehicle : SaveVehicle){
     return this.http.put(this.vehicleEndpoint+vehicle.id, vehicle)
     .pipe(map((response:any) => response));
   }

   delete(id){
     return this.http.delete(this.vehicleEndpoint+id)
     .pipe(map((response:any) => response));
   }
}
