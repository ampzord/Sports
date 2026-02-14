import axios from 'axios'

const api = axios.create({
  baseURL: '/api/v1',
})

api.interceptors.response.use(
  (response) => response,
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
