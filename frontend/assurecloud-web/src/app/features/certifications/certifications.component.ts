import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-certifications',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="certifications-container">
      <h1>Certifications</h1>
    </div>
  `,
  styles: ['.certifications-container { padding: 24px; } h1 { color: #1976d2; }']
})
export class CertificationsComponent {}
