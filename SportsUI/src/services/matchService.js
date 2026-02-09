const BASE = '/api'

async function request(url, options = {}) {
  const headers = {}
  if (options.body) {
    headers['Content-Type'] = 'application/json'
  }
  const res = await fetch(`${BASE}${url}`, {
    headers,
    ...options,
  })
  if (!res.ok) {
    const body = await res.json().catch(() => null)
    const error = new Error(body?.detail || body?.title || res.statusText)
    error.status = res.status
    error.body = body
    throw error
  }
  // 204 No Content
  if (res.status === 204) return null
  return res.json()
}

export const matchService = {
  getMatches: () => request('/matches'),
  getMatchById: (id) => request(`/matches/${id}`),
  addMatch: (data) => request('/matches', { method: 'POST', body: JSON.stringify(data) }),
  updateMatch: (id, data) => request(`/matches/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
  deleteMatch: (id) => request(`/matches/${id}`, { method: 'DELETE' }),
  simulateMatches: () => request('/matches/simulate', { method: 'POST' }),
}
