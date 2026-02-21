import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ErrorTranslationService } from '../services/error-translation.service';
import { ApiResponse } from '../models/api-response.model';
import { environment } from '../../environments/environment';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const errorTranslation = inject(ErrorTranslationService);
  
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'Um erro desconhecido ocorreu.';

      if (error.error instanceof ErrorEvent) {
        // Client-side error
        errorMessage = `Error: ${error.error.message}`;
      } else {
        // Backend error
        if (error.status === 0) {
          errorMessage = 'Não foi possível conectar ao servidor. Verifique sua conexão.';
        } else if (error.error as ApiResponse<any>) {
          const apiError = error.error as ApiResponse<any>;
          
          // Use error code for translation (scalable!)
          errorMessage = errorTranslation.translateError(
            apiError.errorCode,
            apiError.message  // Fallback to English message if code not found
          );
        }
      }

      if (!environment.production) {
          console.error('HTTP Error:', {
          status: error.status,
          errorCode: (error.error as ApiResponse<any>)?.errorCode,
          message: errorMessage,
          url: error.url
        });
      }
      
      return throwError(() => new Error(errorMessage));
    })
  );
};
