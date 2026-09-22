import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-program-detail',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="program-detail">
      <h1>Program Detail</h1>
      <p>ID: {{ id }}</p>
    </div>
  `,
  styles: ['.program-detail { padding: 24px; }']
})
export class ProgramDetailComponent {
  id = '';
}
