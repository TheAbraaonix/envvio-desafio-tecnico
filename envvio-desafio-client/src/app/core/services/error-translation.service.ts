import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ErrorTranslationService {
  private errorPatterns: { pattern: RegExp; translation: (match: RegExpMatchArray) => string }[] = [
    // Vehicle already exists
    {
      pattern: /Vehicle with plate (\w+) already exists/i,
      translation: (match) => `Veículo com placa ${match[1]} já existe`
    },
    // Vehicle already in parking
    {
      pattern: /Vehicle with plate (\w+) already has an open parking session/i,
      translation: (match) => `Veículo com placa ${match[1]} já possui uma sessão de estacionamento aberta`
    },
    // Vehicle not found by plate
    {
      pattern: /Vehicle with plate (\w+) was not found/i,
      translation: (match) => `Veículo com placa ${match[1]} não foi encontrado`
    },
    // Vehicle not found by ID
    {
      pattern: /Vehicle with ID (\d+) was not found/i,
      translation: (match) => `Veículo com ID ${match[1]} não foi encontrado`
    },
    // No open parking session
    {
      pattern: /No open parking session found for vehicle (\w+)/i,
      translation: (match) => `Nenhuma sessão de estacionamento aberta encontrada para o veículo ${match[1]}`
    },
    // Parking session not found
    {
      pattern: /Parking session with ID (\d+) was not found/i,
      translation: (match) => `Sessão de estacionamento com ID ${match[1]} não foi encontrada`
    },
    // Session already closed
    {
      pattern: /Session is already closed/i,
      translation: () => `Sessão já está fechada`
    },
    // Register vehicle first
    {
      pattern: /Please register the vehicle first/i,
      translation: () => `Por favor, registre o veículo primeiro`
    }
  ];

  translateError(errorMessage: string): string {
    if (!errorMessage) {
      return 'Erro desconhecido';
    }

    // Try to match against known patterns
    for (const { pattern, translation } of this.errorPatterns) {
      const match = errorMessage.match(pattern);
      if (match) {
        return translation(match);
      }
    }

    // If no pattern matches, return original message
    // (fallback for unexpected errors)
    return errorMessage;
  }
}
