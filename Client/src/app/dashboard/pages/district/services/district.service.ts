import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { General } from '../../../../shared/interfaces/General';
import { District } from '../interfaces/district';
import { City } from '../../city/interfaces/city';

@Injectable({
  providedIn: 'root'
})
export class DistrictService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/District`;
  private readonly apiCityUrl = `${environment.apiUrl}/City`;

  get(): Observable<General<District[]>> {
    return this.http.get<General<District[]>>(this.apiUrl);
  }

  getCities(): Observable<General<City[]>> {
    return this.http.get<General<District[]>>(this.apiCityUrl);
  }

  create(data: FormData): Observable<any> {
    return this.http.post<any>(this.apiUrl, data);
  }

  update(id: number, data: FormData): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, data);
  }

  updateStatus(id: number): Observable<District> {
    return this.http.put<District>(`${this.apiUrl}/Status/${id}`, {});
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
