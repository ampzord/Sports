import { useQuery, useMutation, useQueryClient } from '@tanstack/vue-query'
import { computed, ref, onUnmounted, type Ref, type ComputedRef } from 'vue'
import { matchService } from '../services/matchService'
import { teamAPI, leagueAPI } from '../services/api'
import type { Match, Team, League } from '../types'

// ── Query keys ──────────────────────────────────────────────
const matchKeys = {
  all: ['matches'] as const,
  detail: (id: number | string) => ['matches', id] as const,
  teams: ['teams'] as const,
  leagues: ['leagues'] as const,
}

// ── Queries ─────────────────────────────────────────────────

export function useMatches() {
  return useQuery<Match[]>({
    queryKey: matchKeys.all,
    queryFn: () => matchService.getMatches(),
    retry: 1,
  })
}

export function useMatch(
  id: Ref<string | number | null> | ComputedRef<string | number | null> | (() => string | number | null) | string | number,
) {
  const resolvedId = computed(() =>
    typeof id === 'function' ? id() : id && typeof id === 'object' && 'value' in id ? id.value : id,
  )
  return useQuery<Match>({
    queryKey: computed(() => matchKeys.detail(resolvedId.value!)),
    queryFn: () => matchService.getMatchById(resolvedId.value!),
    enabled: computed(() => !!resolvedId.value),
    staleTime: 30_000,
  })
}

export function useTeams() {
  return useQuery<Team[]>({
    queryKey: matchKeys.teams,
    queryFn: async () => {
      const res = await teamAPI.getTeams()
      return res.data
    },
    staleTime: 60_000,
  })
}

export function useLeagues() {
  return useQuery<League[]>({
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
    mutationFn: (data: Partial<Match>) => matchService.addMatch(data),
    onSuccess: (newMatch: Match) => {
      // Seed the detail cache so MatchDetail renders instantly
      qc.setQueryData(matchKeys.detail(newMatch.id), newMatch)
      qc.invalidateQueries({ queryKey: matchKeys.all })
    },
  })
}

export function useUpdateMatch() {
  const qc = useQueryClient()
  return useMutation<Match, Error, { id: number | string; data: Partial<Match> }>({
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
    mutationFn: (id: number | string) => matchService.deleteMatch(id),
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
  let timer: ReturnType<typeof setInterval> | null = null
  let pollCount = 0

  const stopPolling = () => {
    if (timer) {
      clearInterval(timer)
    }
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
      const cached: Match[] = qc.getQueryData(matchKeys.all) ?? []

      // Build a lookup of current totalPasses by id
      const cacheMap = new Map(cached.map((m) => [m.id, m.totalPasses]))

      let changed = 0
      for (const m of fresh) {
        if (m.totalPasses !== cacheMap.get(m.id)) {
          changed++
        }
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
    if (timer) {
      return
    }
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
