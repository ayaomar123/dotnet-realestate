import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { General } from '../../../../shared/interfaces/General';
import { Item, PaginatedResponse } from '../interfaces/item';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Item`;

  get(): Observable<General<PaginatedResponse<Item>>> {
    return this.http.get<General<PaginatedResponse<Item>>>(this.apiUrl);
  }

  create(data: FormData): Observable<any> {
    return this.http.post<any>(this.apiUrl, data);
  }

  update(id: number, data: FormData): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, data);
  }

  updateStatus(id: number): Observable<Item> {
    return this.http.put<Item>(`${this.apiUrl}/Status/${id}`, {});
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
