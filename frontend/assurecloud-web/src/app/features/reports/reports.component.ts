import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="reports-container">
      <h1>Reports</h1>
    </div>
  `,
  styles: ['.reports-container { padding: 24px; } h1 { color: #1976d2; }']
})
export class ReportsComponent {}
