import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-report-detail',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="report-detail">
      <h1>Report Detail</h1>
      <p>ID: {{ id }}</p>
    </div>
  `,
  styles: ['.report-detail { padding: 24px; }']
})
export class ReportDetailComponent {
  id = '';
}
