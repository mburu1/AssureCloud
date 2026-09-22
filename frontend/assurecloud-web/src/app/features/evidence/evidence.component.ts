import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-evidence',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="evidence-container">
      <h1>Evidence</h1>
    </div>
  `,
  styles: ['.evidence-container { padding: 24px; } h1 { color: #1976d2; }']
})
export class EvidenceComponent {}
