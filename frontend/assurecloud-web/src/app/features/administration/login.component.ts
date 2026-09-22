import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="login-container">
      <h1>AssureCloud Login</h1>
      <form (ngSubmit)="onSubmit()">
        <input type="email" placeholder="Email" [(ngModel)]="email" name="email" required />
        <input type="password" placeholder="Password" [(ngModel)]="password" name="password" required />
        <button type="submit">Login</button>
      </form>
    </div>
  `,
  styles: [`
    .login-container { max-width: 400px; margin: 0 auto; padding: 24px; }
    h1 { color: #1976d2; }
    form { display: flex; flex-direction: column; gap: 12px; }
    input { padding: 10px; border: 1px solid #ccc; border-radius: 4px; }
    button { padding: 10px; background: #1976d2; color: white; border: none; border-radius: 4px; cursor: pointer; }
  `]
})
export class LoginComponent {
  email = '';
  password = '';

  constructor(private router: Router) {}

  onSubmit() {
    this.router.navigate(['/dashboard']);
  }
}
