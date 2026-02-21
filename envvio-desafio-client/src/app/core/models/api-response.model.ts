import { ErrorCode } from './error-code.enum';

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  errorCode?: ErrorCode;
  data: T;
  statusCode: number;
  errors?: string[];
}
