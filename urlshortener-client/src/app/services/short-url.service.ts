import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ShortUrlDto, CreateShortUrlRequest } from '../models/short-url.model';
import {environment} from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ShortUrlService {
  private apiUrl = `${environment.apiUrl}/ShortUrls`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<ShortUrlDto[]> {
    return this.http.get<ShortUrlDto[]>(`${this.apiUrl}/GetAll`);
  }

  getById(id: string): Observable<ShortUrlDto> {
    return this.http.get<ShortUrlDto>(`${this.apiUrl}/GetById/${id}`);
  }

  create(request: CreateShortUrlRequest): Observable<ShortUrlDto> {
    return this.http.post<ShortUrlDto>(`${this.apiUrl}/Create`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Delete/${id}`);
  }
}
