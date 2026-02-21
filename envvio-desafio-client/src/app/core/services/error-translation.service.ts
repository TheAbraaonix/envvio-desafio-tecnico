import { Injectable } from '@angular/core';
import { ErrorCode } from '../models/error-code.enum';

@Injectable({
  providedIn: 'root'
})
export class ErrorTranslationService {
  
  // Type-safe translation map
  private readonly translations: Record<ErrorCode, string> = {
    [ErrorCode.SUCCESS]: 'Operação realizada com sucesso',
    
    // Generic errors
    [ErrorCode.VALIDATION_ERROR]: 'Erro de validação nos dados fornecidos',
    [ErrorCode.RESOURCE_NOT_FOUND]: 'Recurso não encontrado',
    [ErrorCode.CONFLICT]: 'Conflito ao processar a solicitação',
    [ErrorCode.BAD_REQUEST]: 'Requisição inválida',
    
    // Vehicle errors
    [ErrorCode.VEHICLE_NOT_FOUND]: 'Veículo não encontrado',
    [ErrorCode.VEHICLE_ALREADY_EXISTS]: 'Veículo com esta placa já existe',
    
    // Parking errors
    [ErrorCode.VEHICLE_ALREADY_IN_PARKING]: 'Veículo já possui uma sessão de estacionamento aberta',
    [ErrorCode.PARKING_SESSION_NOT_FOUND]: 'Sessão de estacionamento não encontrada',
    [ErrorCode.SESSION_ALREADY_CLOSED]: 'Sessão já está fechada',
    [ErrorCode.NO_OPEN_SESSION]: 'Nenhuma sessão aberta encontrada para este veículo',
    
    // Internal errors
    [ErrorCode.INTERNAL_SERVER_ERROR]: 'Erro interno do servidor',
    [ErrorCode.DATABASE_ERROR]: 'Erro ao acessar o banco de dados'
  };

  translateError(errorCode?: ErrorCode, fallbackMessage?: string): string {
    if (!errorCode) {
      return fallbackMessage || 'Erro desconhecido';
    }

    return this.translations[errorCode] || fallbackMessage || 'Erro desconhecido';
  }
}
