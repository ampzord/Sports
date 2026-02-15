<template>
  <div class="p-8">
    <!-- Breadcrumb -->
    <div class="mb-6 text-sm text-gray-500">
      <router-link to="/matches" class="text-blue-600 hover:text-blue-800">Matches</router-link>
      <span class="mx-2">/</span>
      <span class="text-gray-700">{{ matchTitle }}</span>
    </div>

    <div class="bg-white p-8 rounded-lg shadow border border-gray-200">
      <div class="flex justify-between items-start mb-6">
        <h2 class="text-2xl font-bold text-blue-600">{{ matchTitle }}</h2>
        <div class="flex gap-2">
          <router-link
            :to="`/matches/${matchId}/edit`"
            aria-label="Edit match"
            class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-4 py-2 rounded font-semibold transition cursor-pointer"
          >
            Edit
          </router-link>
          <button
            @click="deleteMatchConfirm"
            aria-label="Delete match"
            class="bg-red-100 hover:bg-red-200 text-red-700 px-4 py-2 rounded font-semibold transition cursor-pointer"
          >
            Delete
          </button>
        </div>
      </div>

      <!-- Match Info -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div class="bg-blue-50 p-4 rounded">
          <label class="block text-sm font-semibold text-blue-600 mb-1">Home Team</label>
          <p class="text-gray-800 font-semibold">{{ homeTeamName }}</p>
        </div>
        <div class="bg-blue-50 p-4 rounded">
          <label class="block text-sm font-semibold text-blue-600 mb-1">Away Team</label>
          <p class="text-gray-800 font-semibold">{{ awayTeamName }}</p>
        </div>
        <div class="bg-blue-50 p-4 rounded">
          <label class="block text-sm font-semibold text-blue-600 mb-1">League</label>
          <p class="text-gray-800">{{ leagueName }}</p>
        </div>
        <div class="bg-blue-50 p-4 rounded">
          <label class="block text-sm font-semibold text-blue-600 mb-1">Total Passes</label>
          <p class="text-gray-800">{{ match?.totalPasses != null ? match.totalPasses : 'Not simulated' }}</p>
        </div>
        <div v-if="match?.date" class="bg-blue-50 p-4 rounded">
          <label class="block text-sm font-semibold text-blue-600 mb-1">Date</label>
          <p class="text-gray-800">{{ match?.date }}</p>
        </div>
        <div v-if="match?.status" class="bg-blue-50 p-4 rounded">
          <label class="block text-sm font-semibold text-blue-600 mb-1">Status</label>
          <p class="text-gray-800">{{ match?.status }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useMatch, useTeams, useLeagues, useDeleteMatch } from '../../composables/useMatchQueries'
import { useToast } from '../../composables/useToast'
import { getApiErrorMessage } from '../../utils/errors'
import { useRouteId } from '../../composables/useRouteId'

const toast = useToast()
const router = useRouter()
const matchId = useRouteId()

const { data: match } = useMatch(matchId)
const { data: teams } = useTeams()
const { data: leagues } = useLeagues()
const deleteMutation = useDeleteMatch()

const homeTeamName = computed(() => {
  if (!match.value?.homeTeamId || !teams.value?.length) return 'TBD'
  const team = teams.value.find((t) => t.id === match.value.homeTeamId || String(t.id) === String(match.value.homeTeamId))
  return team?.name || 'Unknown'
})

const awayTeamName = computed(() => {
  if (!match.value?.awayTeamId || !teams.value?.length) return 'TBD'
  const team = teams.value.find((t) => t.id === match.value.awayTeamId || String(t.id) === String(match.value.awayTeamId))
  return team?.name || 'Unknown'
})

const leagueName = computed(() => {
  if (!match.value?.homeTeamId || !teams.value?.length || !leagues.value?.length) return ''
  const homeTeam = teams.value.find((t) => t.id === match.value.homeTeamId || String(t.id) === String(match.value.homeTeamId))
  if (!homeTeam?.leagueId) return 'Unknown'
  const league = leagues.value.find((l) => l.id === homeTeam.leagueId || String(l.id) === String(homeTeam.leagueId))
  return league?.name || 'Unknown'
})

const matchTitle = computed(() => {
  return `${homeTeamName.value} vs ${awayTeamName.value}`
})

const deleteMatchConfirm = async () => {
  if (confirm('Are you sure you want to delete this match?')) {
    try {
      await deleteMutation.mutateAsync(matchId.value)
      toast.success('Match deleted')
      router.push({ name: 'Matches' })
    } catch (err: unknown) {
      console.error('Failed to delete match:', err)
      toast.error(getApiErrorMessage(err, 'Failed to delete match'))
    }
  }
}
</script>
