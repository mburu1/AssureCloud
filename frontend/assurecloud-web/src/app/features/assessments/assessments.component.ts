import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-assessments',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="assessments-container">
      <h1>Assessments</h1>
    </div>
  `,
  styles: ['.assessments-container { padding: 24px; } h1 { color: #1976d2; }']
})
export class AssessmentsComponent {}
