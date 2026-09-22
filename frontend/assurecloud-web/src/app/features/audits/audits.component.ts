import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-audits',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="audits-container">
      <h1>Audits</h1>
    </div>
  `,
  styles: ['.audits-container { padding: 24px; } h1 { color: #1976d2; }']
})
export class AuditsComponent {}
