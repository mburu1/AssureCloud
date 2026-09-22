import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-audit-detail',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="audit-detail">
      <h1>Audit Detail</h1>
      <p>ID: {{ id }}</p>
    </div>
  `,
  styles: ['.audit-detail { padding: 24px; }']
})
export class AuditDetailComponent {
  id = '';
}
