import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AboutPageResponse, UpdateAboutPageRequest } from '../models/about.model';
import {environment} from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AboutService {
  private apiUrl = `${environment.apiUrl}/About`;

  constructor(private http: HttpClient) {}

  get(): Observable<AboutPageResponse> {
    return this.http.get<AboutPageResponse>(`${this.apiUrl}/Get`);
  }

  update(request: UpdateAboutPageRequest): Observable<AboutPageResponse> {
    return this.http.put<AboutPageResponse>(`${this.apiUrl}/Update`, request);
  }
}
