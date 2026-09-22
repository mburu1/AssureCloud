import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-organization-detail',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="organization-detail">
      <h1>Organization Detail</h1>
      <p>ID: {{ id }}</p>
    </div>
  `,
  styles: ['.organization-detail { padding: 24px; }']
})
export class OrganizationDetailComponent {
  id = '';
}
