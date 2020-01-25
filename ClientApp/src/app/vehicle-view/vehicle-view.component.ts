import { VehicleService } from './../services/vehicle.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-vehicle-view',
  templateUrl: './vehicle-view.component.html',
  styleUrls: ['./vehicle-view.component.css']
})
export class VehicleViewComponent implements OnInit {

  vehicle: {};
  vehicleId: number;


  constructor(
    private vehicleService : VehicleService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    route.params.subscribe(p=>{
      this.vehicleId = +p['id'];
    });
  }

  ngOnInit() {

    this.vehicleService.getVehicle(this.vehicleId)
    .subscribe(
      v => this.vehicle = v,
      err => {
        if (err.status == 404) {
          this.router.navigate(['/vehicles']);
          return; 
        }
      });
  }

  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService.delete(this.vehicleId)
        .subscribe(x => {
          this.router.navigate(['/vehicles']);
        });
    }
  }

}
