import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AboutService } from '../services/about.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="mb-4">
      <h2 class="mb-1">About URL Shortener</h2>
      <p class="text-muted">
        Learn more about this application.
      </p>
    </div>

    <div class="card border-0 shadow-sm">
      <div class="card-body p-4">
        <div class="d-flex justify-content-between align-items-center mb-3">
          <h5 class="mb-0">About</h5>

          <small class="text-muted">
            Updated {{ updatedDate() | date:'medium' }}
          </small>
        </div>

        <p class="mb-0" style="white-space: pre-line;">
          {{ content() }}
        </p>
      </div>
    </div>

    @if (authService.isAdmin()) {
      <div class="card border-0 shadow-sm mt-4">
        <div class="card-body p-4">
          <h5>Edit About Page</h5>

          <textarea
            [ngModel]="editContent()"
            (ngModelChange)="editContent.set($event)"
            class="form-control"
            rows="7">
          </textarea>

          <div class="text-end mt-3">
            <button
              class="btn btn-primary"
              (click)="save()">
              Save Changes
            </button>
          </div>
        </div>
      </div>
    }
  `
})
export class AboutComponent implements OnInit {
  content = signal('');
  updatedDate = signal('');
  editContent = signal('');

  constructor(
    private aboutService: AboutService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadAbout();
  }

  loadAbout(): void {
    this.aboutService.get().subscribe(data => {
      if (data) {
        this.content.set(data.content);
        this.updatedDate.set(data.updatedDate);
        this.editContent.set(data.content);
      }
    });
  }

  save(): void {
    this.aboutService.update({ content: this.editContent() }).subscribe({
      next: (data) => {
        alert('Updated successfully!');

        if (data) {
          this.content.set(data.content);
          this.updatedDate.set(data.updatedDate);
        } else {
          this.content.set(this.editContent());
          this.updatedDate.set(new Date().toISOString());
        }

        this.editContent.set(this.content());
      },
      error: () => alert('Forbidden: Only Admin can update this page.')
    });
  }
}
