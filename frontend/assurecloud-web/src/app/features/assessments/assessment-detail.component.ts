import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-assessment-detail',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="assessment-detail">
      <h1>Assessment Detail</h1>
      <p>ID: {{ id }}</p>
    </div>
  `,
  styles: ['.assessment-detail { padding: 24px; }']
})
export class AssessmentDetailComponent {
  id = '';
}
