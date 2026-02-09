<template>
  <div class="p-8">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold text-blue-600">Matches</h1>
      <div class="flex items-center gap-3">
        <button
          @click="simulateAll"
          class="bg-green-600 hover:bg-green-700 active:scale-95 text-white px-5 py-2 rounded font-semibold transition cursor-pointer inline-flex items-center gap-2"
        >
          ▶ Simulate All Matches
        </button>
        <router-link
          to="/matches/create"
          class="bg-blue-600 hover:bg-blue-700 text-white px-5 py-2 rounded font-semibold transition"
        >
          + Create Match
        </router-link>
      </div>
    </div>

    <!-- Loading skeleton -->
    <div v-if="isLoading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
      <table class="w-full">
        <thead class="bg-blue-50">
          <tr>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Match</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">League</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Total Passes</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="n in 8" :key="n" class="border-t border-gray-200">
            <td class="px-6 py-4"><div class="h-4 w-40 bg-gray-200 rounded animate-pulse"></div></td>
            <td class="px-6 py-4"><div class="h-4 w-24 bg-gray-200 rounded animate-pulse"></div></td>
            <td class="px-6 py-4"><div class="h-4 w-16 bg-gray-200 rounded animate-pulse"></div></td>
            <td class="px-6 py-4"><div class="h-4 w-28 bg-gray-200 rounded animate-pulse"></div></td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Virtualized Matches List -->
    <div v-if="!isLoading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
      <!-- Sticky header -->
      <div class="bg-blue-50 grid grid-cols-[1fr_1fr_1fr_1fr]">
        <div class="px-6 py-3 text-left text-blue-600 font-bold">Match</div>
        <div class="px-6 py-3 text-left text-blue-600 font-bold">League</div>
        <div class="px-6 py-3 text-left text-blue-600 font-bold">Total Passes</div>
        <div class="px-6 py-3 text-left text-blue-600 font-bold">Actions</div>
      </div>

      <!-- Scrollable virtual container -->
      <div ref="scrollEl" class="overflow-auto" :style="{ maxHeight: '70vh' }">
        <!-- Spacer that represents total height of all rows -->
        <div :style="{ height: `${virtualizer.getTotalSize()}px`, position: 'relative' }">
          <div
            v-for="row in virtualizer.getVirtualItems()"
            :key="matchRows[row.index].id"
            :style="{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: `${row.size}px`,
              transform: `translateY(${row.start}px)`,
            }"
            class="grid grid-cols-[1fr_1fr_1fr_1fr] items-center border-t border-gray-200 hover:bg-blue-50 hover:shadow-[inset_3px_0_0_0_#3b82f6] transition-all duration-200"
          >
            <div class="px-6 py-4">
              <router-link
                :to="{ name: 'MatchDetail', params: { id: matchRows[row.index].id } }"
                class="text-blue-600 hover:text-blue-800 font-semibold"
              >
                {{ getTeamName(matchRows[row.index].homeTeamId) }} vs {{ getTeamName(matchRows[row.index].awayTeamId) }}
              </router-link>
            </div>
            <div class="px-6 py-4 text-gray-600">{{ getMatchLeagueName(matchRows[row.index]) }}</div>
            <div class="px-6 py-4">
              <span
                v-if="matchRows[row.index].totalPasses != null"
                class="bg-green-100 text-green-700 px-3 py-1 rounded-full text-sm font-semibold inline-block"
              >
                {{ matchRows[row.index].totalPasses }}
              </span>
              <span v-else class="text-gray-400 text-sm inline-block">Not simulated</span>
            </div>
            <div class="px-6 py-4 flex gap-2">
              <button
                @click="editMatch(matchRows[row.index])"
                class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-3 py-1 rounded font-semibold transition cursor-pointer"
              >
                Edit
              </button>
              <button
                @click="handleDelete(matchRows[row.index].id)"
                :disabled="deletingId === matchRows[row.index].id"
                class="bg-red-100 hover:bg-red-200 disabled:opacity-50 text-red-700 px-3 py-1 rounded font-semibold transition cursor-pointer inline-flex items-center gap-1.5"
              >
                <span
                  v-if="deletingId === matchRows[row.index].id"
                  class="inline-block w-3 h-3 border-2 border-red-700 border-t-transparent rounded-full animate-spin"
                ></span>
                Delete
              </button>
            </div>
          </div>
        </div>
      </div>

      <p v-if="matchRows.length === 0" class="p-6 text-gray-400 text-center">No matches found</p>
    </div>

    <!-- Error state -->
    <div v-if="isError" class="bg-red-50 border border-red-200 rounded-lg p-6 text-center">
      <p class="text-red-600 font-semibold">{{ matchError?.message || 'Failed to load matches. Please try again.' }}</p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useVirtualizer } from '@tanstack/vue-virtual'
import { useToast } from '../../composables/useToast'
import { useMatches, useTeams, useLeagues, useDeleteMatch, useSimulateWithPolling } from '../../composables/useMatchQueries'

const ROW_HEIGHT = 52

const toast = useToast()
const router = useRouter()
const deletingId = ref(null)
const scrollEl = ref(null)

const { data: matches, isLoading, isError, error: matchError } = useMatches()
const { data: teams } = useTeams()
const { data: leagues } = useLeagues()

const deleteMutation = useDeleteMatch()
const { simulate: simulateMutation } = useSimulateWithPolling()

const matchRows = computed(() => matches.value ?? [])

const virtualizer = useVirtualizer({
  get count() {
    return matchRows.value.length
  },
  getScrollElement: () => scrollEl.value,
  estimateSize: () => ROW_HEIGHT,
  overscan: 20,
})

// --- Actions ---
const handleDelete = async (id) => {
  if (!confirm('Are you sure?')) return
  deletingId.value = id
  try {
    await deleteMutation.mutateAsync(id)
    toast.success('Match deleted')
  } catch (error) {
    console.error('Failed to delete match:', error)
    toast.error(error.message || 'Failed to delete match')
  } finally {
    deletingId.value = null
  }
}

const editMatch = (match) => {
  router.push(`/matches/${match.id}/edit`)
}

const getLeagueName = (leagueId) => {
  if (!leagueId && leagueId !== 0) return 'Unknown'
  const league = (leagues.value || []).find((l) => l.id === leagueId || String(l.id) === String(leagueId))
  return league?.name || 'Unknown'
}

const getMatchLeagueName = (match) => {
  const team = (teams.value || []).find((t) => t.id === match.homeTeamId || String(t.id) === String(match.homeTeamId))
  if (!team) return 'Unknown'
  return getLeagueName(team.leagueId)
}

const getTeamName = (teamId) => {
  if (!teamId && teamId !== 0) return 'TBD'
  const team = (teams.value || []).find((t) => t.id === teamId || String(t.id) === String(teamId))
  return team?.name || 'Unknown'
}

const simulateAll = () => {
  simulateMutation.mutate(undefined, {
    onSuccess: () => toast.success('Triggered Matches Simulation'),
    onError: (error) => {
      console.error('Failed to simulate:', error)
      toast.error(error.message || 'Failed to simulate matches')
    },
  })
}
</script>
