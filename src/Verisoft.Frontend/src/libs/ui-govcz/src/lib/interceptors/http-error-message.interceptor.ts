import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { SnackbarService } from '../components';

@Injectable()
export class HttpErrorMessageInterceptor implements HttpInterceptor {

  snackbarService = inject(SnackbarService);

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((errorResponse: HttpErrorResponse) => {
        if (!errorResponse.ok) {

          if (errorResponse.status === 400) {
            console.error('Bad request');
          } if (errorResponse.status === 403) {
            this.snackbarService.showError('Unauthorized request');
            console.error('Unauthorized request');
          } if (errorResponse.status === 404) {
            console.error('Entity not found');
          } if (errorResponse.status === 422) {
            console.error('Error occured during validation');
            console.log(errorResponse);
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
