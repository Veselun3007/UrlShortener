import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ShortUrlService } from '../services/short-url.service';
import { ShortUrlDto } from '../models/short-url.model';

@Component({
  selector: 'app-short-url-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="mb-4">
      <a routerLink="/" class="text-decoration-none">
        ← Back to URLs
      </a>
    </div>

    @if (loading()) {
      <div class="text-center py-5">
        <div class="spinner-border text-primary"></div>
        <p class="text-muted mt-3">Loading URL details...</p>
      </div>
    } @else if (urlDetails()) {
      <div class="card border-0 shadow-sm">
        <div class="card-body p-4">
          <h2 class="mb-4">Short URL Details</h2>

          <dl class="row mb-0">
            <dt class="col-sm-3">ID</dt>
            <dd class="col-sm-9 text-break">
              {{ urlDetails()!.id }}
            </dd>

            <dt class="col-sm-3">Original URL</dt>
            <dd class="col-sm-9 text-break">
              <a
                [href]="urlDetails()!.originalUrl"
                target="_blank"
                rel="noopener noreferrer">
                {{ urlDetails()!.originalUrl }}
              </a>
            </dd>

            <dt class="col-sm-3">Short Code</dt>
            <dd class="col-sm-9">
              <code>{{ urlDetails()!.shortCode }}</code>
            </dd>

            <dt class="col-sm-3">Created By</dt>
            <dd class="col-sm-9">
              {{ urlDetails()!.createdBy }}
            </dd>

            <dt class="col-sm-3">Created Date</dt>
            <dd class="col-sm-9">
              {{ urlDetails()!.createdDate | date:'medium' }}
            </dd>
          </dl>
        </div>
      </div>
    } @else {
      <div class="alert alert-danger">
        Short URL could not be loaded.
      </div>
    }
  `
})
export class ShortUrlDetailComponent implements OnInit {
  urlDetails = signal<ShortUrlDto | null>(null);
  loading = signal(true);

  constructor(
    private route: ActivatedRoute,
    private shortUrlService: ShortUrlService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.loading.set(false); // Виправлено на .set()
      return;
    }

    this.shortUrlService.getById(id).subscribe({
      next: data => {
        this.urlDetails.set(data);
        this.loading.set(false);
      },
      error: error => {
        console.error('DETAIL: error =', error);
        this.loading.set(false);
      }
    });
  }
}
