import axios from 'axios'
import type { AxiosResponse } from 'axios'
import type { League, Team, Player } from '../types'

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
  getLeagues: (): Promise<AxiosResponse<League[]>> => api.get('/leagues'),
  getLeagueById: (id: number | string): Promise<AxiosResponse<League>> => api.get(`/leagues/${id}`),
  addLeague: (data: Partial<League>): Promise<AxiosResponse<League>> => api.post('/leagues', data),
  updateLeague: (id: number | string, data: Partial<League>): Promise<AxiosResponse<League>> => api.put(`/leagues/${id}`, data),
  deleteLeague: (id: number | string): Promise<AxiosResponse<void>> => api.delete(`/leagues/${id}`),
}

export const teamAPI = {
  getTeams: (params?: Record<string, unknown>): Promise<AxiosResponse<Team[]>> => api.get('/teams', { params }),
  getTeamById: (id: number | string): Promise<AxiosResponse<Team>> => api.get(`/teams/${id}`),
  addTeam: (data: Partial<Team>): Promise<AxiosResponse<Team>> => api.post('/teams', data),
  updateTeam: (id: number | string, data: Partial<Team>): Promise<AxiosResponse<Team>> => api.put(`/teams/${id}`, data),
  deleteTeam: (id: number | string): Promise<AxiosResponse<void>> => api.delete(`/teams/${id}`),
}

export const playerAPI = {
  getPlayers: (params?: Record<string, unknown>): Promise<AxiosResponse<Player[]>> => api.get('/players', { params }),
  getPlayerById: (id: number | string): Promise<AxiosResponse<Player>> => api.get(`/players/${id}`),
  addPlayer: (data: Partial<Player>): Promise<AxiosResponse<Player>> => api.post('/players', data),
  updatePlayer: (id: number | string, data: Partial<Player>): Promise<AxiosResponse<Player>> => api.put(`/players/${id}`, data),
  deletePlayer: (id: number | string): Promise<AxiosResponse<void>> => api.delete(`/players/${id}`),
}
