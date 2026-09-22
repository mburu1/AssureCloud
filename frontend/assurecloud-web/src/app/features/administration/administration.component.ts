import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-administration',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="administration-container">
      <h1>Administration</h1>
    </div>
  `,
  styles: ['.administration-container { padding: 24px; } h1 { color: #1976d2; }']
})
export class AdministrationComponent {}
