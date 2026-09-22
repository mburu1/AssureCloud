import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-programs',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="programs-container">
      <h1>Programs</h1>
    </div>
  `,
  styles: ['.programs-container { padding: 24px; } h1 { color: #1976d2; }']
})
export class ProgramsComponent {}
