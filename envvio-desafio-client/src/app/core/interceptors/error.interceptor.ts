import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unexpected error occurred';

      if (error.error instanceof ErrorEvent) {
        // Client-side or network error
        errorMessage = `Error: ${error.error.message}`;
      } else {
        // Backend returned an unsuccessful response code
        if (error.status === 0) {
          errorMessage = 'Unable to connect to the server. Please check your internet connection.';
        } else if (error.error?.message) {
          // API returned a structured error message
          errorMessage = error.error.message;
        } else {
          errorMessage = `Server Error: ${error.status} - ${error.statusText}`;
        }
      }

      console.error('HTTP Error:', {
        status: error.status,
        message: errorMessage,
        url: error.url,
        details: error.error
      });

      // TODO: Add toast/snackbar notification here when we implement UI notifications
      
      return throwError(() => new Error(errorMessage));
    })
  );
};
