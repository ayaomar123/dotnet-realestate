import { Category } from './../../category/interfaces/category';
import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ItemService } from '../services/item.service';
import { NotificationService } from '../../../../shared/services/notification.service';
import { ImageUploadComponent } from "../../../../shared/components/image-upload/image-upload.component";
import { City } from '../../city/interfaces/city';
import { District } from '../../district/interfaces/district';
import { Type } from '../../type/interfaces/type';
import { PropertyType } from '../../property-type/interfaces/property-type';
import { Status } from '../../status/interfaces/status';
import { General } from '../../../../shared/interfaces/General';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { GoogleMapComponent } from "../../../../shared/components/google-map/google-map.component";

@Component({
  selector: 'app-create-item',
  imports: [ReactiveFormsModule, CommonModule, ImageUploadComponent, GoogleMapComponent],
  templateUrl: './create-item.component.html',
  styleUrl: './create-item.component.css'
})
export class CreateItemComponent implements OnInit {
  private readonly service = inject(ItemService);
  private readonly fb = inject(FormBuilder);
  private readonly toastr = inject(NotificationService);
  private readonly router = inject(Router);


  form!: FormGroup;
  activeTab = 'main';
  readonly tabs = ['main', 'details', 'location'];

  // Data
  categories: Category[] = [];
  cities: City[] = [];
  districts: District[] = [];
  types: Type[] = [];
  propertyTypes: PropertyType[] = [];
  statuses: Status[] = [];

  selectedImageFile?: File;

  ngOnInit(): void {
    this.initForm();
    this.loadAllLookups();
  }

  initForm(): void {
    this.form = this.fb.group({
      nameEn: ['', Validators.required],
      nameAr: ['', Validators.required],
      advertiseNo: [null, Validators.required],
      adNo: [null, Validators.required],
      categoryId: [null, Validators.required],
      typeId: [null, Validators.required],
      propertyTypeId: [null, Validators.required],
      statusId: [null, Validators.required],
      cityId: [null],
      districtId: [null],
      latitude: [null],
      longitude: [null],
      descriptionEn: [''],
      descriptionAr: [''],
      soum: [''],
      limit: [''],
      streetWidth: [''],
      space: [''],
      pricePerMeter: [''],
      hasUnits: [false],
      rangeFrom: [''],
      rangeTo: [''],
      hasPassword: [false],
      hashPassword: [''],
    });
  }

  goToNextTab(): void {
    const currentIndex = this.tabs.indexOf(this.activeTab);
    if (currentIndex < this.tabs.length - 1) {
      this.activeTab = this.tabs[currentIndex + 1];
    }
  }

  onImageSelected(file: File): void {
    this.selectedImageFile = file;
  }

  saveAll(): void {
    debugger;
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.value;
    const formData = new FormData();

    const appendField = (key: string, value: any) => {
      formData.append(key, value != null ? value : '');
    };

    appendField('NameEn', formValue.nameEn);
    appendField('NameAr', formValue.nameAr);
    appendField('AdvertiseNo', formValue.advertiseNo);
    appendField('AdNo', formValue.adNo);
    appendField('CategoryId', formValue.categoryId);
    appendField('CityId', formValue.cityId);
    appendField('DistrictId', formValue.districtId);
    appendField('MyTypeId', formValue.typeId);
    appendField('PropertyTypeId', formValue.propertyTypeId);
    appendField('StatusId', formValue.statusId);
    appendField('Latitude', formValue.latitude);
    appendField('Longitude', formValue.longitude);
    appendField('DescriptionEn', formValue.descriptionEn);
    appendField('DescriptionAr', formValue.descriptionAr);
    appendField('Soum', formValue.soum);
    appendField('Limit', formValue.limit);
    appendField('StreetWidth', formValue.streetWidth);
    appendField('Space', formValue.space);
    appendField('PricePerMeter', formValue.pricePerMeter);
    appendField('HasUnits', formValue.hasUnits);
    appendField('RangeFrom', formValue.rangeFrom);
    appendField('RangeTo', formValue.rangeTo);
    appendField('HasPassword', formValue.hasPassword);
    appendField('Password', formValue.hashPassword);

    if (this.selectedImageFile) {
      formData.append('Image', this.selectedImageFile);
    }

    this.service.create(formData).subscribe({
      next: () => {
        this.toastr.success('Item created successfully!');
        this.router.navigate(['/admin/items']);
      },
      error: (err) => {
        console.error('Error creating item:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  updateCoordinates(coords: { lat: number; lng: number }) {
    this.form.patchValue({
      latitude: coords.lat,
      longitude: coords.lng
    });
  }

  private loadAllLookups(): void {
    this.loadLookup<Category[]>(this.service.getCategories(), data => this.categories = data);
    this.loadLookup<City[]>(this.service.getCities(), data => this.cities = data);
    this.loadLookup<District[]>(this.service.getDistricts(), data => this.districts = data);
    this.loadLookup<Type[]>(this.service.getTypes(), data => this.types = data);
    this.loadLookup<PropertyType[]>(this.service.getPropertyTypes(), data => this.propertyTypes = data);
    this.loadLookup<Status[]>(this.service.getStatuses(), data => this.statuses = data);
  }



  private loadLookup<T>(
    obs: Observable<General<T>>,
    assign: (data: T) => void
  ): void {
    obs.subscribe({
      next: (res) => assign(res.data),
      error: (err) => console.error('Lookup load error:', err),
    });
  }


}

