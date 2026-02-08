import { ref, onUnmounted } from 'vue'
import { matchAPI, teamAPI, leagueAPI } from '../services/api'

/**
 * Composable for match data with on-demand polling.
 *
 * - Loads matches, teams, leagues on `fetchAll()`
 * - `startSimulation()` triggers the API then polls every `interval` ms
 * - Polling auto-stops when every match has a `totalPasses` value
 * - Cleans up on component unmount
 */
export function useMatchPolling(interval = 2000) {
  const matches = ref([])
  const teams = ref([])
  const leagues = ref([])
  const loading = ref(true)
  const simulating = ref(false)

  let pollTimer = null

  const fetchMatches = async () => {
    const res = await matchAPI.getMatches()
    matches.value = res.data
  }

  const fetchAll = async () => {
    loading.value = true
    try {
      const [matchRes, teamRes, leagueRes] = await Promise.all([
        matchAPI.getMatches(),
        teamAPI.getTeams(),
        leagueAPI.getLeagues(),
      ])
      matches.value = matchRes.data
      teams.value = teamRes.data
      leagues.value = leagueRes.data
    } catch (error) {
      console.error('Failed to fetch data:', error)
    } finally {
      loading.value = false
    }
  }

  const allSimulated = () => matches.value.length > 0 && matches.value.every((m) => m.totalPasses != null)

  const stopPolling = () => {
    clearInterval(pollTimer)
    pollTimer = null
    simulating.value = false
  }

  const startPolling = () => {
    if (pollTimer) return
    simulating.value = true
    pollTimer = setInterval(async () => {
      try {
        await fetchMatches()
        if (allSimulated()) stopPolling()
      } catch (error) {
        console.error('Polling error:', error)
      }
    }, interval)
  }

  const startSimulation = async () => {
    await matchAPI.simulateMatches()
    startPolling()
  }

  onUnmounted(stopPolling)

  return {
    matches,
    teams,
    leagues,
    loading,
    simulating,
    fetchAll,
    fetchMatches,
    startSimulation,
    stopPolling,
  }
}
