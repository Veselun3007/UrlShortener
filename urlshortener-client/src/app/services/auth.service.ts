import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import {JwtPayload, LoginRequest, LoginResponse} from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = `${environment.apiUrl}/Auth`;

  private token = signal<string | null>(localStorage.getItem('authToken'));

  private payload = computed<JwtPayload | null>(() => {
    const t = this.token();
    if (!t) return null;
    try {
      return JSON.parse(atob(t.split('.')[1])) as JwtPayload;
    } catch {
      return null;
    }
  });

  private isTokenExpired = computed<boolean>(() => {
    const exp = this.payload()?.exp;
    if (!exp) return false;
    return exp * 1000 < Date.now();
  });

  readonly isLoggedIn = computed(() => !!this.token() && !this.isTokenExpired());

  readonly userName = computed(() => {
    const p = this.payload();
    return (p?.unique_name as string) ??
      (p?.name as string) ??
      (p?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] as string) ??
      null;
  });

  readonly isAdmin = computed(() => {
    const p = this.payload();
    if (!p) return false;

    const role = p.role ?? p['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    if (Array.isArray(role)) {
      return role.includes('Admin');
    }
    return role === 'Admin';
  });

  constructor(private http: HttpClient) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/Login`, credentials).pipe(
      tap(response => {
        localStorage.setItem('authToken', response.token);
        this.token.set(response.token);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('authToken');
    this.token.set(null);
  }
}
