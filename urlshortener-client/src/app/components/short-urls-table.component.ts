import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ShortUrlService } from '../services/short-url.service';
import { AuthService } from '../services/auth.service';
import { ShortUrlDto } from '../models/short-url.model';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-short-urls-table',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="d-flex justify-content-between align-items-center mb-4">
      <div>
        <h2 class="mb-1">Short URLs</h2>
        <p class="text-muted mb-0">
          Manage and access shortened links
        </p>
      </div>
    </div>

    @if (authService.isLoggedIn()) {
      <div class="card border-0 shadow-sm mb-4">
        <div class="card-body">
          <h5 class="card-title">Create short URL</h5>

          <div class="input-group">
            <input
              type="url"
              [(ngModel)]="newUrl"
              class="form-control"
              placeholder="https://example.com"
              (keyup.enter)="createUrl()" />

            <button
              class="btn btn-primary"
              [disabled]="!newUrl.trim()"
              (click)="createUrl()">
              Shorten URL
            </button>
          </div>

          @if (errorMessage()) {
            <div class="alert alert-danger mt-3 mb-0">
              {{ errorMessage() }}
            </div>
          }
        </div>
      </div>
    }

    @if (loading()) {
      <div class="text-center py-5">
        <div class="spinner-border text-primary"></div>
        <p class="text-muted mt-3">Loading URLs...</p>
      </div>
    } @else if (urls().length === 0) {
      <div class="card border-0 shadow-sm">
        <div class="card-body text-center py-5">
          <h5>No short URLs yet</h5>
          <p class="text-muted mb-0">
            Create your first shortened URL to get started.
          </p>
        </div>
      </div>
    } @else {
      <div class="card border-0 shadow-sm">
        <div class="table-responsive">
          <table class="table table-hover align-middle mb-0">
            <thead class="table-light">
            <tr>
              <th>Original URL</th>
              <th>Short URL</th>
              <th>Created By</th>
              <th>Created</th>
              <th class="text-center">Actions</th>
            </tr>
            </thead>

            <tbody>
              @for (item of urls(); track item.id) {
                <tr>
                  <td>
                    <a
                      [href]="item.originalUrl"
                      target="_blank"
                      rel="noopener noreferrer"
                      class="text-decoration-none">
                      {{ item.originalUrl }}
                    </a>
                  </td>

                  <td>
                    <a
                      [href]="getShortUrlLink(item.shortCode)"
                      target="_blank"
                      rel="noopener noreferrer"
                      class="fw-semibold text-decoration-none">
                      /{{ item.shortCode }}
                    </a>
                  </td>

                  <td>
                    <span class="badge bg-light text-dark border">
                      {{ item.createdBy }}
                    </span>
                  </td>

                  <td class="text-muted">
                    {{ item.createdDate | date:'medium' }}
                  </td>

                  <td class="text-center">
                    <div class="d-inline-flex align-items-center gap-1">
                      <a
                        [routerLink]="['/urls', item.id]"
                        class="btn btn-sm btn-outline-info border-0 p-1"
                        title="View Details">
                        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
                          <circle cx="12" cy="12" r="10"></circle>
                          <line x1="12" y1="16" x2="12" y2="12"></line>
                          <line x1="12" y1="8" x2="12.01" y2="8"></line>
                        </svg>
                      </a>

                      @if (canDelete(item)) {
                        <button
                          class="btn btn-sm btn-outline-danger border-0 p-1"
                          (click)="deleteUrl(item)"
                          title="Delete URL">
                          <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
                            <polyline points="3 6 5 6 21 6"></polyline>
                            <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
                            <line x1="10" y1="11" x2="10" y2="17"></line>
                            <line x1="14" y1="11" x2="14" y2="17"></line>
                          </svg>
                        </button>
                      }
                    </div>
                  </td>
              }
            </tbody>
          </table>
        </div>
      </div>
    }
  `
})
export class ShortUrlsTableComponent implements OnInit {
  urls = signal<ShortUrlDto[]>([]);
  newUrl = '';
  errorMessage = signal('');
  loading = signal(false);

  constructor(
    private shortUrlService: ShortUrlService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadUrls();
  }

  loadUrls(): void {
    this.loading.set(true);

    this.shortUrlService.getAll().subscribe({
      next: data => {
        this.urls.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.errorMessage.set('Failed to load short URLs.');
        this.loading.set(false);
      }
    });
  }

  canDelete(item: ShortUrlDto): boolean {
    return this.authService.isAdmin() ||
      item.createdBy === this.authService.userName();
  }

  getShortUrlLink(shortCode: string): string {
    return `${environment.baseUrl}/s/${shortCode}`;
  }

  createUrl(): void {
    const url = this.newUrl.trim();

    if (!url) {
      return;
    }

    this.shortUrlService.create({ url }).subscribe({
      next: createdItem => {
        this.urls.update(list => [createdItem, ...list]);
        this.newUrl = '';
        this.errorMessage.set('');
      },
      error: err => {
        this.errorMessage.set(
          err.error?.detail ?? 'Failed to create short URL.'
        );
      }
    });
  }

  deleteUrl(item: ShortUrlDto): void {
    if (!confirm(`Delete short URL /${item.shortCode}?`)) {
      return;
    }

    this.shortUrlService.delete(item.id).subscribe({
      next: () => {
        this.urls.update(list =>
          list.filter(x => x.id !== item.id)
        );
      },
      error: err => {
        alert(
          err.error?.detail ??
          'Cannot delete this URL.'
        );
      }
    });
  }
}
