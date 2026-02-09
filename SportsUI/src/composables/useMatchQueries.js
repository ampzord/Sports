import { useQuery, useMutation, useQueryClient } from '@tanstack/vue-query'
import { computed, ref, onUnmounted } from 'vue'
import { matchService } from '../services/matchService'
import { teamAPI, leagueAPI } from '../services/api'

// ── Query keys ──────────────────────────────────────────────
export const matchKeys = {
  all: ['matches'],
  detail: (id) => ['matches', id],
  teams: ['teams'],
  leagues: ['leagues'],
}

// ── Queries ─────────────────────────────────────────────────

export function useMatches() {
  return useQuery({
    queryKey: matchKeys.all,
    queryFn: () => matchService.getMatches(),
  })
}

export function useMatch(id) {
  const resolvedId = computed(() => (typeof id === 'function' ? id() : (id?.value ?? id)))
  return useQuery({
    queryKey: computed(() => matchKeys.detail(resolvedId.value)),
    queryFn: () => matchService.getMatchById(resolvedId.value),
    enabled: computed(() => !!resolvedId.value),
  })
}

export function useTeams() {
  return useQuery({
    queryKey: matchKeys.teams,
    queryFn: async () => {
      const res = await teamAPI.getTeams()
      return res.data
    },
    staleTime: 60_000,
  })
}

export function useLeagues() {
  return useQuery({
    queryKey: matchKeys.leagues,
    queryFn: async () => {
      const res = await leagueAPI.getLeagues()
      return res.data
    },
    staleTime: 60_000,
  })
}

// ── Mutations ───────────────────────────────────────────────

export function useCreateMatch() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (data) => matchService.addMatch(data),
    onSuccess: () => qc.invalidateQueries({ queryKey: matchKeys.all }),
  })
}

export function useUpdateMatch() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, data }) => matchService.updateMatch(id, data),
    onSuccess: (_res, { id }) => {
      qc.invalidateQueries({ queryKey: matchKeys.all })
      qc.invalidateQueries({ queryKey: matchKeys.detail(id) })
    },
  })
}

export function useDeleteMatch() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id) => matchService.deleteMatch(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: matchKeys.all }),
  })
}

/**
 * Simulate mutation + smart polling.
 *
 * After each simulate click, polls every `interval` ms.
 * Each poll fetches the full list, diffs against the cache,
 * and only patches matches whose totalPasses changed.
 * Stops when a poll brings zero new changes.
 *
 * Spammable — clicking again just re-triggers the API
 * and resets the "no-change stop" condition.
 */
export function useSimulateWithPolling(interval = 2000, maxPolls = 30) {
  const qc = useQueryClient()
  const polling = ref(false)
  let timer = null
  let pollCount = 0

  const stopPolling = () => {
    clearInterval(timer)
    timer = null
    polling.value = false
    pollCount = 0
  }

  const poll = async () => {
    pollCount++
    if (pollCount >= maxPolls) {
      stopPolling()
      return
    }

    try {
      const fresh = await matchService.getMatches()
      const cached = qc.getQueryData(matchKeys.all) ?? []

      // Build a lookup of current totalPasses by id
      const cacheMap = new Map(cached.map((m) => [m.id, m.totalPasses]))

      let changed = 0
      for (const m of fresh) {
        if (m.totalPasses !== cacheMap.get(m.id)) changed++
      }

      // Always update the cache with the fresh data
      qc.setQueryData(matchKeys.all, fresh)

      // If nothing changed this cycle, simulation is done
      if (changed === 0) {
        stopPolling()
      }
    } catch (err) {
      console.error('Polling error:', err)
    }
  }

  const startPolling = () => {
    if (timer) return
    polling.value = true
    pollCount = 0
    timer = setInterval(poll, interval)
  }

  const simulate = useMutation({
    mutationFn: () => matchService.simulateMatches(),
    onSuccess: () => {
      startPolling()
    },
  })

  onUnmounted(stopPolling)

  return {
    simulate,
    polling,
    stopPolling,
  }
}
