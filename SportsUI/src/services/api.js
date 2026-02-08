import axios from 'axios'

export const api = axios.create({
  baseURL: '/api',
})

api.interceptors.response.use(
  (response) => {
    console.log('API Response:', response.config.url, response.data)
    return response
  },
  (error) => {
    console.error('API Error:', error.config?.url, error.message, error.response?.data)
    return Promise.reject(error)
  },
)

export const leagueAPI = {
  getLeagues: () => api.get('/leagues'),
  getLeagueById: (id) => api.get(`/leagues/${id}`),
  addLeague: (data) => api.post('/leagues', data),
  updateLeague: (id, data) => api.put(`/leagues/${id}`, data),
  deleteLeague: (id) => api.delete(`/leagues/${id}`),
}

export const teamAPI = {
  getTeams: (params) => api.get('/teams', { params }),
  getTeamById: (id) => api.get(`/teams/${id}`),
  addTeam: (data) => api.post('/teams', data),
  updateTeam: (id, data) => api.put(`/teams/${id}`, data),
  deleteTeam: (id) => api.delete(`/teams/${id}`),
}

export const playerAPI = {
  getPlayers: (params) => api.get('/players', { params }),
  getPlayerById: (id) => api.get(`/players/${id}`),
  addPlayer: (data) => api.post('/players', data),
  updatePlayer: (id, data) => api.put(`/players/${id}`, data),
  deletePlayer: (id) => api.delete(`/players/${id}`),
}

export const matchAPI = {
  getMatches: (params) => api.get('/matches', { params }),
  getMatchById: (id) => api.get(`/matches/${id}`),
  addMatch: (data) => api.post('/matches', data),
  updateMatch: (id, data) => api.put(`/matches/${id}`, data),
  deleteMatch: (id) => api.delete(`/matches/${id}`),
  simulateMatches: (params) => api.post('/matches/simulate', null, { params }),
}
