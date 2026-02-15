import type { Match } from '../types'

interface ApiErrorBody {
  detail?: string
  title?: string
}

class ServiceError extends Error {
  status: number
  body: ApiErrorBody | null

  constructor(message: string, status: number, body: ApiErrorBody | null) {
    super(message)
    this.name = 'ServiceError'
    this.status = status
    this.body = body
  }
}

const BASE = '/api/v1'

async function request<T>(url: string, options: RequestInit = {}): Promise<T> {
  const headers: Record<string, string> = {}
  if (options.body) {
    headers['Content-Type'] = 'application/json'
  }
  const res = await fetch(`${BASE}${url}`, {
    headers,
    ...options,
  })
  if (!res.ok) {
    const body: ApiErrorBody | null = await res.json().catch(() => null)
    throw new ServiceError(body?.detail || body?.title || res.statusText, res.status, body)
  }
  // 204 No Content
  if (res.status === 204) return null as T
  return res.json()
}

export const matchService = {
  getMatches: (): Promise<Match[]> => request<Match[]>('/matches'),
  getMatchById: (id: number | string): Promise<Match> => request<Match>(`/matches/${id}`),
  addMatch: (data: Partial<Match>): Promise<Match> => request<Match>('/matches', { method: 'POST', body: JSON.stringify(data) }),
  updateMatch: (id: number | string, data: Partial<Match>): Promise<Match> =>
    request<Match>(`/matches/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
  deleteMatch: (id: number | string): Promise<null> => request<null>(`/matches/${id}`, { method: 'DELETE' }),
  simulateMatches: (): Promise<null> => request<null>('/matches/simulate', { method: 'POST' }),
}
