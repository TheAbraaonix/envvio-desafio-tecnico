/**
 * Formats a UTC date/time to local timezone in DD/MM/YYYY HH:mm format
 * @param utcTime - UTC date string or Date object
 * @returns Formatted date string in local timezone (DD/MM/YYYY HH:mm)
 */
export function formatLocalTime(utcTime: Date | string): string {
  // Parse UTC date and convert to local time for display
  let date: Date;
  if (typeof utcTime === 'string') {
    const utcString = utcTime.endsWith('Z') ? utcTime : utcTime + 'Z';
    date = new Date(utcString);
  } else {
    date = new Date(utcTime);
  }
  
  // Format manually to avoid DatePipe issues in utility functions
  const day = String(date.getDate()).padStart(2, '0');
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const year = date.getFullYear();
  const hours = String(date.getHours()).padStart(2, '0');
  const minutes = String(date.getMinutes()).padStart(2, '0');
  
  return `${day}/${month}/${year} ${hours}:${minutes}`;
}

/**
 * Formats a UTC date/time to local timezone showing only the hour (DD/MM/YYYY HH:00)
 * Useful for hourly reports where minutes are not relevant
 * @param utcTime - UTC date string or Date object
 * @returns Formatted date string in local timezone (DD/MM/YYYY HH:00)
 */
export function formatLocalHour(utcTime: Date | string): string {
  // Parse UTC date and convert to local time for display
  let date: Date;
  if (typeof utcTime === 'string') {
    const utcString = utcTime.endsWith('Z') ? utcTime : utcTime + 'Z';
    date = new Date(utcString);
  } else {
    date = new Date(utcTime);
  }
  
  // Format as DD/MM/YYYY HH:00 in local timezone
  const day = String(date.getDate()).padStart(2, '0');
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const year = date.getFullYear();
  const hours = String(date.getHours()).padStart(2, '0');
  
  return `${day}/${month}/${year} ${hours}:00`;
}

/**
 * Calculates duration between a past time and now
 * @param startTime - Start time (UTC)
 * @returns Formatted duration string (e.g., "2h 30m")
 */
export function calculateDuration(startTime: Date | string): string {
  const now = new Date();
  
  // Parse as UTC
  let start: Date;
  if (typeof startTime === 'string') {
    const utcString = startTime.endsWith('Z') ? startTime : startTime + 'Z';
    start = new Date(utcString);
  } else {
    start = new Date(startTime);
  }
  
  const diffMs = now.getTime() - start.getTime();
  const diffMins = Math.floor(diffMs / 60000);
  const hours = Math.floor(diffMins / 60);
  const minutes = diffMins % 60;
  
  // Handle negative duration
  if (diffMins < 0) {
    return '0h 0m';
  }
  
  return `${hours}h ${minutes}m`;
}

/**
 * Formats a TimeSpan string (hh:mm:ss.fffffff) to readable format
 * @param duration - TimeSpan string from backend
 * @returns Formatted duration string (e.g., "2h 30m")
 */
export function formatDuration(duration: string): string {
  if (!duration) return '0h 0m';
  
  try {
    // Split on ':' to get [hours, minutes, seconds.milliseconds]
    const parts = duration.split(':');
    if (parts.length < 2) return '0h 0m';
    
    const hours = parseInt(parts[0], 10);
    const minutes = parseInt(parts[1], 10);
    
    return `${hours}h ${minutes}m`;
  } catch (error) {
    console.error('Error formatting duration:', error);
    return '0h 0m';
  }
}
