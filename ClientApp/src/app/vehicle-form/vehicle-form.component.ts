import { Contact } from './../models/contact';
import { VehicleService } from './../services/vehicle.service';
import { FeatureService } from './../services/feature.service';
import { MakeService } from './../services/make.service';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, forkJoin  } from 'rxjs';
import 'rxjs/add/observable/forkJoin';
import { SaveVehicle } from '../models/saveVehicle';
import * as _ from 'underscore';
import { Vehicle } from '../models/vehicle';


@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {

  makes : any[];
  models: any[];
  mk : any[];
  md: any[];
  features: any[];
  vehicle : SaveVehicle = {
    id: 0,
    modelId: 0,
    makeId: 0,
    isRegistered: false,
    features: [],
    contact: {
      name: '',
      phone: '',
      email: ''
    }
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private makeService: MakeService,
    private featureService: FeatureService,
    private vehicleService: VehicleService,
    private toastr: ToastrService
    ) { 
      route.params.subscribe(p => {
        this.vehicle.id = +p['id'] || 0;
      });
    }

  ngOnInit() {

    var source = [
      this.makeService.getMakes(),
      this.featureService.getFeatures(),
    ]

    console.log(source);

    if(this.vehicle.id)
      source.push(this.vehicleService.getVehicle(this.vehicle.id));

    Observable.forkJoin(source).subscribe(data => {
      this.makes = data[0];
      this.features = data[1];

      if(this.vehicle.id){
        this.setVehicle(data[2]);
        this.populateModels();
      }
    }, err=>{
      if(err.status == 404)
        this.router.navigate(['/']); 
    });
    
  }

  private setVehicle(v : Vehicle ){
    this.vehicle.id = v.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.contact = v.contact;
    this.vehicle.features = _.pluck(v.features, 'id');
  }

  onMakeChange(){
    this.populateModels();
    delete this.vehicle.modelId
  }

  private populateModels(){
    var selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
    this.models = selectedMake ? selectedMake.models : [];
  }

  onFeatureToggle(featureId, $event){
    if($event.target.checked)
      this.vehicle.features.push(featureId);
    else{
      var index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index, 1);
    }
  }

  submit(){
    var result$ = (this.vehicle.id) ? this.vehicleService.update(this.vehicle) : this.vehicleService.create(this.vehicle); 
    result$.subscribe(vehicle => {
        this.toastr.success("Vehicle record has been updated","Success"),
        this.router.navigate(['/vehicles/', vehicle.id])
    })
  }

  delete(){
    if(confirm("Are you sure")){
      this.vehicleService.delete(this.vehicle.id)
      .subscribe(
        x => { 
          this.router.navigate(['/home']);
        });
    }
  }

}
