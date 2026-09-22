import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-organizations',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="organizations-container">
      <h1>Organizations</h1>
      <button (click)="createOrganization()">New Organization</button>
    </div>
  `,
  styles: [`
    .organizations-container { padding: 24px; }
    h1 { color: #1976d2; }
    button { margin-top: 16px; }
  `]
})
export class OrganizationsComponent {
  constructor(private router: Router) {}
  createOrganization() { this.router.navigate(['/organizations/new']); }
}
