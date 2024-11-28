import { Component } from '@angular/core';
import { City } from '../models/city';
import { CitiesService } from '../services/cities.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.css']
})
export class CitiesComponent {
  cities: City[] = [];
  postCityForm: FormGroup;
  isPostCityFormSubmitted: boolean = false;

  constructor(private citiesService: CitiesService) {
    this.postCityForm = new FormGroup({
        cityName: new FormControl(null, [Validators.required])
    });
  }

  ngOnInit() {
    this.loadCities();
  }

  loadCities() {
    this.citiesService.getCities()
      .subscribe({

        next: (response: City[]) => {
          this.cities = response;
        },

        error: (error: any) => {
          console.log(error);
        },

        complete: () => { }
      });
  }
  get postCity_CityNameControl(): any {
    return this.postCityForm.controls['cityName'];
  }

  public postCitySubmitted() {
    //TODO
    this.isPostCityFormSubmitted = true;
    console.log(this.postCityForm.value);

    this.citiesService.postCity(this.postCityForm.value).subscribe({
      next: (response: City) => {
        console.log(response);

        //this.loadCities();
        this.cities.push(new City(response.cityID, response.cityName));

        this.postCityForm.reset();
        this.isPostCityFormSubmitted = false;
      },
      error: (error: any) => { console.log(error) },
      complete: () => { }

    });
  }

}
