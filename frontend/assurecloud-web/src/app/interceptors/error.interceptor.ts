import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): any {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let message = error.message;

        if (error.status === 401) {
          localStorage.removeItem('access_token');
          this.router.navigate(['/login']);
          message = 'Authentication required. Please log in.';
        } else if (error.status === 403) {
          message = 'You do not have permission to access this resource.';
        } else if (error.status === 404) {
          message = 'Resource not found.';
        } else if (error.status === 429) {
          message = 'Too many requests. Please try again later.';
        } else if (error.status >= 500) {
          message = 'A server error occurred. Please try again later.';
        }

        return throwError(() => ({ ...error, message }));
      })
    );
  }
}
