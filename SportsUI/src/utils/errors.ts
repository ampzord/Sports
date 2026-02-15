import axios from 'axios'

/**
 * Extracts a user-friendly error message from any caught error.
 * Handles Axios errors (from api.ts) and ServiceErrors (from matchService.ts).
 */
export function getApiErrorMessage(err: unknown, fallback = 'An unexpected error occurred'): string {
  // Axios errors (leagues, teams, players)
  if (axios.isAxiosError(err)) {
    const data = err.response?.data as { detail?: string; title?: string } | undefined
    return data?.detail || data?.title || err.message || fallback
  }

  // Standard Error (including ServiceError from matchService.ts)
  if (err instanceof Error) {
    return err.message || fallback
  }

  return fallback
}
