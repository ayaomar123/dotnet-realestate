import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { General } from '../../../../shared/interfaces/General';
import { Status } from '../interfaces/status';

@Injectable({
  providedIn: 'root'
})
export class StatusService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Status`;

  get(): Observable<General<Status[]>> {
    return this.http.get<General<Status[]>>(this.apiUrl);
  }

  create(data: FormData): Observable<any> {
    return this.http.post<any>(this.apiUrl, data);
  }

  update(id: number, data: FormData): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, data);
  }

  updateStatus(id: number): Observable<Status> {
    return this.http.put<Status>(`${this.apiUrl}/Status/${id}`, {});
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
