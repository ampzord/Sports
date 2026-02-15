export interface League {
  id: number
  name: string
}

export interface Team {
  id: number
  name: string
  leagueId: number
}

export interface Player {
  id: number
  name: string
  position: string | null
  teamId: number | null
}

export interface Match {
  id: number
  homeTeamId: number
  awayTeamId: number
  totalPasses: number | null
  date?: string
  status?: string
}

export interface SelectOption {
  value: string | number
  label: string
}

export interface PositionOption {
  value: string
  label: string
}

// Form data types for create/edit forms
export interface TeamFormData {
  name: string
  leagueId: number | ''
}

export interface PlayerFormData {
  name: string
  position: string
  teamId: number | null
}

export interface MatchFormData {
  homeTeamId: number | ''
  awayTeamId: number | ''
  totalPasses: number | null
}
