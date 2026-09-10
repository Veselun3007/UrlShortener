import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="row justify-content-center">
      <div class="col-md-5 col-lg-4">

        <div class="text-center mb-4">
          <div class="display-6">🔗</div>
          <h2 class="fw-bold">URL Shortener</h2>
          <p class="text-muted">Sign in to manage your links</p>
        </div>

        <div class="card border-0 shadow-sm">
          <div class="card-body p-4">

            <h5 class="mb-4">Sign in</h5>

            <div class="mb-3">
              <label class="form-label">Username</label>
              <input
                type="text"
                [(ngModel)]="username"
                class="form-control"
                placeholder="Enter username"
                (keyup.enter)="onLogin()" />
            </div>

            <div class="mb-3">
              <label class="form-label">Password</label>
              <div class="input-group">
                <input
                  [type]="showPassword ? 'text' : 'password'"
                  [(ngModel)]="password"
                  class="form-control"
                  placeholder="Enter password"
                  (keyup.enter)="onLogin()" />
                <button
                  type="button"
                  class="btn btn-outline-secondary"
                  (click)="showPassword = !showPassword"
                  tabindex="-1">

                  {{ showPassword ? 'Hide' : 'Show' }}
                </button>
              </div>
            </div>

            @if (error) {
              <div class="alert alert-danger py-2">
                {{ error }}
              </div>
            }

            <button
              class="btn btn-primary w-100"
              [disabled]="!username || !password"
              (click)="onLogin()">
              Sign In
            </button>

          </div>
        </div>

      </div>
    </div>
  `
})
export class LoginComponent {
  username = '';
  password = '';
  error = '';
  showPassword = false;

  constructor(private authService: AuthService, private router: Router) {}

  onLogin(): void {
    this.authService.login({ userName: this.username, password: this.password }).subscribe({
      next: () => this.router.navigate(['/']),
      error: () => this.error = 'Invalid username or password.'
    });
  }
}
