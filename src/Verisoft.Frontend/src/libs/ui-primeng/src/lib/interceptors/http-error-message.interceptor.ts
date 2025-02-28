import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { SnackbarService } from '../components/snackbar';

@Injectable()
export class HttpErrorMessageInterceptor implements HttpInterceptor {
  constructor(private readonly snackbarService: SnackbarService) {}
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((errorResponse: HttpErrorResponse) => {
        if (!errorResponse.ok) {
          if (errorResponse.status === 403) {
            this.snackbarService.showError('Unauthorized request');
            console.error('Unauthorized request');
          } else {
            this.snackbarService.showError(errorResponse.message);
            console.error(errorResponse);
          }
        }

        return throwError(() => errorResponse);
      })
    );
  }
}
