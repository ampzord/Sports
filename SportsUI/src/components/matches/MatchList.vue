<template>
  <div class="p-8">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold text-blue-600">Matches</h1>
      <div class="flex items-center gap-3">
        <button
          @click="simulateAll"
          :disabled="simulating"
          class="bg-green-600 hover:bg-green-700 disabled:bg-green-400 disabled:cursor-not-allowed text-white px-5 py-2 rounded font-semibold transition cursor-pointer inline-flex items-center gap-2"
        >
          <span
            v-if="simulating"
            class="inline-block w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin"
          ></span>
          {{ simulating ? 'Simulating...' : '▶ Simulate All Matches' }}
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
    <div v-if="loading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
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

    <!-- Matches List -->
    <div v-if="!loading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
      <table class="w-full">
        <thead class="bg-blue-50">
          <tr>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Match</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">League</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Total Passes</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Actions</th>
          </tr>
        </thead>
        <TransitionGroup tag="tbody" appear @before-enter="onBeforeEnter" @enter="onEnter">
          <tr
            v-for="(match, index) in matches ?? []"
            :key="match.id"
            :data-index="index"
            class="border-t border-gray-200 hover:bg-blue-50 hover:shadow-[inset_3px_0_0_0_#3b82f6] transition-all duration-200"
          >
            <td class="px-6 py-4">
              <router-link
                :to="{ name: 'MatchDetail', params: { id: match.id } }"
                class="text-blue-600 hover:text-blue-800 font-semibold"
              >
                {{ getTeamName(match.homeTeamId) }} vs {{ getTeamName(match.awayTeamId) }}
              </router-link>
            </td>
            <td class="px-6 py-4 text-gray-600">{{ getMatchLeagueName(match) }}</td>
            <td class="px-6 py-4">
              <Transition name="cell-swap" mode="out-in">
                <span
                  v-if="match.totalPasses != null"
                  :key="'passes-' + match.totalPasses"
                  class="bg-green-100 text-green-700 px-3 py-1 rounded-full text-sm font-semibold inline-block"
                >
                  {{ match.totalPasses }}
                </span>
                <span v-else key="not-simulated" class="text-gray-400 text-sm inline-block">Not simulated</span>
              </Transition>
            </td>
            <td class="px-6 py-4 flex gap-2">
              <button
                @click="editMatch(match)"
                class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-3 py-1 rounded font-semibold transition"
              >
                Edit
              </button>
              <button
                @click="deleteMatch(match.id)"
                :disabled="deletingId === match.id"
                class="bg-red-100 hover:bg-red-200 disabled:opacity-50 text-red-700 px-3 py-1 rounded font-semibold transition cursor-pointer inline-flex items-center gap-1.5"
              >
                <span
                  v-if="deletingId === match.id"
                  class="inline-block w-3 h-3 border-2 border-red-700 border-t-transparent rounded-full animate-spin"
                ></span>
                Delete
              </button>
            </td>
          </tr>
        </TransitionGroup>
      </table>
      <p v-if="(matches ?? []).length === 0" class="p-6 text-gray-400 text-center">No matches found</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useToast } from '../../composables/useToast'
import { useStaggerAnimation } from '../../composables/useStaggerAnimation'
import { useMatchPolling } from '../../composables/useMatchPolling'
import { matchAPI } from '../../services/api'

const toast = useToast()
const router = useRouter()
const { onBeforeEnter, onEnter } = useStaggerAnimation()
const deletingId = ref(null)

const { matches, teams, leagues, loading, simulating, fetchAll, fetchMatches, startSimulation } = useMatchPolling()

onMounted(fetchAll)

// --- Actions ---
const deleteMatch = async (id) => {
  if (confirm('Are you sure?')) {
    deletingId.value = id
    try {
      await matchAPI.deleteMatch(id)
      toast.success('Match deleted')
      await fetchMatches()
    } catch (error) {
      console.error('Failed to delete match:', error)
      const msg = error.response?.data?.detail || error.response?.data?.title || 'Failed to delete match'
      toast.error(msg)
    } finally {
      deletingId.value = null
    }
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

const simulateAll = async () => {
  try {
    await startSimulation()
    toast.success('Simulation started!')
  } catch (error) {
    console.error('Failed to simulate:', error)
    const msg = error.response?.data?.detail || error.response?.data?.title || 'Failed to simulate matches'
    toast.error(msg)
  }
}
</script>

<style scoped>
.cell-swap-enter-active,
.cell-swap-leave-active {
  transition: all 0.35s ease;
}
.cell-swap-enter-from {
  opacity: 0;
  transform: translateY(6px) scale(0.95);
}
.cell-swap-leave-to {
  opacity: 0;
  transform: translateY(-6px) scale(0.95);
}
</style>
