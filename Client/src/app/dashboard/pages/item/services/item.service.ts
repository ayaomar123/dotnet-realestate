import { HttpClient, HttpContext } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { General } from '../../../../shared/interfaces/General';
import { Item, PaginatedResponse } from '../interfaces/item';
import { Category } from '../../category/interfaces/category';
import { City } from '../../city/interfaces/city';
import { Type } from '../../type/interfaces/type';
import { PropertyType } from '../../property-type/interfaces/property-type';
import { District } from '../../district/interfaces/district';
import { Status } from '../../status/interfaces/status';
import { IS_PUBLIC } from '../../../../core/auth/auth.interceptor';


@Injectable({
  providedIn: 'root'
})
export class ItemService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Item`;
  private readonly url = `${environment.apiUrl}`;

  get(): Observable<General<PaginatedResponse<Item>>> {
    return this.http.get<General<PaginatedResponse<Item>>>(this.apiUrl, { context: new HttpContext().set(IS_PUBLIC, true) });
  }

  getById(id: number): Observable<General<Item>> {
    return this.http.get<General<Item>>(`${this.apiUrl}/${id}`, { context: new HttpContext().set(IS_PUBLIC, true) });
  }

  getCategories(): Observable<General<Category[]>> {
    return this.http.get<General<Category[]>>(this.url + '/Category');
  }

  getCities(): Observable<General<City[]>> {
    return this.http.get<General<City[]>>(this.url + '/City');
  }

  getDistricts(): Observable<General<District[]>> {
    return this.http.get<General<District[]>>(this.url + '/District');
  }

  getTypes(): Observable<General<Type[]>> {
    return this.http.get<General<Type[]>>(this.url + '/Type');
  }

  getPropertyTypes(): Observable<General<PropertyType[]>> {
    return this.http.get<General<PropertyType[]>>(this.url + '/PropertyType');
  }

  getStatuses(): Observable<General<Status[]>> {
    return this.http.get<General<Status[]>>(this.url + '/Status');
  }

  create(data: FormData): Observable<any> {
    return this.http.post<any>(this.apiUrl, data);
  }

  update(id: number, data: FormData): Observable<any> {
    return this.http.patch<any>(`${this.apiUrl}/${id}`, data);
  }

  updateStatus(id: number): Observable<Item> {
    return this.http.put<Item>(`${this.apiUrl}/Status/${id}`, {});
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
