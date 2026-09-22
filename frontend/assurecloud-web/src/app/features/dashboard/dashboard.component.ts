import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  template: `
    <div class="dashboard-container">
      <h1>Dashboard</h1>
      <p>Welcome to AssureCloud. Your compliance dashboard is loading...</p>
    </div>
  `,
  styles: [`
    .dashboard-container {
      padding: 24px;
    }
    h1 {
      color: #1976d2;
    }
  `]
})
export class DashboardComponent {}
