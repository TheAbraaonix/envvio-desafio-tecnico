import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ErrorTranslationService } from '../services/error-translation.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const errorTranslation = inject(ErrorTranslationService);
  
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'Um erro desconhecido ocorreu.';

      if (error.error instanceof ErrorEvent) {
        // Client-side or network error
        errorMessage = `Error: ${error.error.message}`;
      } else {
        // Backend returned an unsuccessful response code
        if (error.status === 0) {
            errorMessage = 'Não foi possível conectar ao servidor. Verifique sua conexão com a internet.';
        } else if (error.error?.message) {
          // API returned a structured error message - translate it
          errorMessage = errorTranslation.translateError(error.error.message);
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
      
      return throwError(() => new Error(errorMessage));
    })
  );
};
