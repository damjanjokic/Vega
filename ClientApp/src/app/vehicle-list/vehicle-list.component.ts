import { MakeService } from './../services/make.service';
import { KeyValuePair } from './../models/keyValuePair';
import { VehicleService } from './../services/vehicle.service';
import { Component, OnInit } from '@angular/core';
import { Vehicle } from '../models/vehicle';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {
  private readonly PAGE_SIZE = 4
  queryResult: any = {};
  // allVehicles: Vehicle[];
  makes : KeyValuePair[];
  query: any = {
    pageSize: this.PAGE_SIZE
  };
  columns = [
    {title: 'Id'},
    {title: 'ContactName', key: 'ContactName', isSortable: true},
    {title: 'Make', key: 'make', isSortable: true},
    {title: 'Model', key: 'model', isSortable: true},
    {}
  ];

  constructor(private vehicleService: VehicleService, private makeService : MakeService) { }

  ngOnInit() {
    this.makeService.getMakes()
      .subscribe(makes => this.makes = makes);

   this.populateVehicles();
  }

  onFilterChange(){
    // var vehicles = this.allVehicles;


    // if(this.query.makeId)
    //   vehicles = vehicles.query(v => v.make.id == this.query.makeId);
    
    
    // if(this.query.modelId)
    //   vehicles = vehicles.query(v => v.model.id == this.query.modelId);

    
    // this.vehicles = vehicles;
    this.query.page = 1;
    this.populateVehicles();
  }

  resetFilter(){
    this.query = {
      page: 1,
      pageSize: this.PAGE_SIZE
    };
    this.populateVehicles();
  }

  populateVehicles(){
    
    this.vehicleService.getVehicles(this.query)
    .subscribe(result => {
      //this.vehicles /*= this.allVehicles*/  = result.items
      this.queryResult = result
    });
    console.log(this.query);
  }

  sortBy(columnName){
    if(this.query.sortBy === columnName){
      this.query.isSortAscending = !this.query.isSortAscending;
    } else{
      this.query.sortBy = columnName;
      this.query.isSortAscending = true; 
    }
    this.populateVehicles();
  }

  onPageChanged(page){
    this.query.page = page;
    this.populateVehicles();
  }

}
