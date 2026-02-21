/**
 * Formats a number as Brazilian Real currency
 * @param value - Numeric value to format
 * @returns Formatted currency string (e.g., "R$ 15,00")
 */
export function formatCurrency(value: number): string {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(value);
}
