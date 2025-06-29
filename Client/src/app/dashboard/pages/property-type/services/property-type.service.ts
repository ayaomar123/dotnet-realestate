import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { General } from '../../../../shared/interfaces/General';
import { PropertyType } from '../interfaces/property-type';

@Injectable({
  providedIn: 'root'
})
export class PropertyTypeService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/PropertyType`;

  get(): Observable<General<PropertyType[]>> {
    return this.http.get<General<PropertyType[]>>(this.apiUrl);
  }

  create(data: FormData): Observable<any> {
    return this.http.post<any>(this.apiUrl, data);
  }

  update(id: number, data: FormData): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, data);
  }

  updateStatus(id: number): Observable<PropertyType> {
    return this.http.put<PropertyType>(`${this.apiUrl}/Status/${id}`, {});
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
