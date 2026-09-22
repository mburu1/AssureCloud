import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-certification-detail',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="certification-detail">
      <h1>Certification Detail</h1>
      <p>ID: {{ id }}</p>
    </div>
  `,
  styles: ['.certification-detail { padding: 24px; }']
})
export class CertificationDetailComponent {
  id = '';
}
