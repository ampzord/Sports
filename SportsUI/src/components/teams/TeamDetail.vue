<template>
  <div class="p-8">
    <!-- Loading state -->
    <div v-if="loading" role="status" aria-label="Loading team" class="space-y-4">
      <div class="h-8 w-48 bg-gray-200 rounded animate-pulse"></div>
      <div class="bg-white rounded-lg shadow p-6 border border-gray-200 space-y-3">
        <div class="h-5 w-32 bg-gray-200 rounded animate-pulse"></div>
        <div class="h-4 w-full bg-gray-200 rounded animate-pulse"></div>
        <div class="h-4 w-3/4 bg-gray-200 rounded animate-pulse"></div>
      </div>
    </div>

    <!-- Breadcrumb -->
    <div v-if="!loading" class="mb-6 text-sm">
      <router-link to="/teams" class="text-blue-600 hover:text-blue-800 font-semibold">Teams</router-link>
      <span class="mx-2 text-gray-400">/</span>
      <span class="text-gray-600">{{ team?.name }}</span>
    </div>

    <div v-if="!loading" class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold text-blue-600">{{ team?.name }}</h1>
      <div class="flex gap-3">
        <router-link
          :to="`/teams/${teamId}/edit`"
          class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-4 py-2 rounded font-semibold transition cursor-pointer"
        >
          Edit Team
        </router-link>
        <button
          @click="handleDelete"
          class="bg-red-100 hover:bg-red-200 text-red-700 px-4 py-2 rounded font-semibold transition cursor-pointer"
        >
          Delete Team
        </button>
      </div>
    </div>

    <!-- League Info -->
    <div v-if="!loading && team?.leagueId" class="mb-6">
      <p class="text-gray-600">
        League:
        <router-link :to="`/leagues/${team.leagueId}`" class="text-blue-600 hover:text-blue-800 font-semibold">
          {{ leagueName }}
        </router-link>
      </p>
    </div>

    <!-- Players in Team -->
    <div v-if="!loading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
      <div class="flex justify-between items-center p-6 border-b border-gray-200">
        <h2 class="text-lg font-bold text-blue-600">Players in {{ team?.name }}</h2>
        <router-link
          :to="`/players/create?teamId=${teamId}`"
          class="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded font-semibold transition cursor-pointer"
        >
          + Add Player
        </router-link>
      </div>
      <table class="w-full">
        <thead class="bg-blue-50">
          <tr>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Player Name</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Position</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="player in players" :key="player.id" class="border-t border-gray-200 hover:bg-blue-50 transition">
            <td class="px-6 py-4">
              <router-link :to="`/players/${player.id}`" class="text-blue-600 hover:text-blue-800 font-semibold">
                {{ player.name }}
              </router-link>
            </td>
            <td class="px-6 py-4 text-gray-600">{{ player.position || '-' }}</td>
            <td class="px-6 py-4 flex gap-2">
              <router-link
                :to="`/players/${player.id}/edit`"
                :aria-label="`Edit ${player.name}`"
                class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-3 py-1 rounded font-semibold transition cursor-pointer"
              >
                Edit
              </router-link>
              <button
                @click="deletePlayer(player.id)"
                :aria-label="`Delete ${player.name}`"
                class="bg-red-100 hover:bg-red-200 text-red-700 px-3 py-1 rounded font-semibold transition cursor-pointer"
              >
                Delete
              </button>
            </td>
          </tr>
        </tbody>
      </table>
      <p v-if="players.length === 0" class="p-6 text-gray-400 text-center">No players in this team yet</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { teamAPI, playerAPI, leagueAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'

const toast = useToast()
const route = useRoute()
const router = useRouter()
const teamId = route.params.id
const team = ref(null)
const leagueName = ref('')
const players = ref([])
const loading = ref(true)

onMounted(async () => {
  await Promise.all([loadTeam(), loadPlayers()])
  loading.value = false
})

const loadTeam = async () => {
  try {
    const response = await teamAPI.getTeamById(teamId)
    team.value = response.data
    if (team.value.leagueId) {
      const leagueRes = await leagueAPI.getLeagueById(team.value.leagueId)
      leagueName.value = leagueRes.data.name
    }
  } catch (error) {
    console.error('Failed to fetch team:', error)
  }
}

const loadPlayers = async () => {
  try {
    const response = await playerAPI.getPlayers({ teamId })
    players.value = response.data
  } catch (error) {
    console.error('Failed to fetch players:', error)
  }
}

const handleDelete = async () => {
  if (confirm('Are you sure you want to delete this team?')) {
    try {
      await teamAPI.deleteTeam(teamId)
      toast.success('Team deleted')
      router.push('/teams')
    } catch (error) {
      console.error('Failed to delete team:', error)
      const msg = error.response?.data?.detail || error.response?.data?.title || 'Failed to delete team'
      toast.error(msg)
    }
  }
}

const deletePlayer = async (id) => {
  if (confirm('Are you sure you want to delete this player?')) {
    try {
      await playerAPI.deletePlayer(id)
      toast.success('Player deleted')
      await loadPlayers()
    } catch (error) {
      console.error('Failed to delete player:', error)
      const msg = error.response?.data?.detail || error.response?.data?.title || 'Failed to delete player'
      toast.error(msg)
    }
  }
}
</script>
