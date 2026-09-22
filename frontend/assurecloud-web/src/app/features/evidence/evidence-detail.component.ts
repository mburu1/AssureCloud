import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-evidence-detail',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="evidence-detail">
      <h1>Evidence Detail</h1>
      <p>ID: {{ id }}</p>
    </div>
  `,
  styles: ['.evidence-detail { padding: 24px; }']
})
export class EvidenceDetailComponent {
  id = '';
}
