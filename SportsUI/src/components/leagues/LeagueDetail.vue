<template>
  <div class="p-8">
    <!-- Loading state -->
    <div v-if="loading" role="status" aria-label="Loading league" class="space-y-4">
      <div class="h-8 w-48 bg-gray-200 rounded animate-pulse"></div>
      <div class="bg-white rounded-lg shadow p-6 border border-gray-200 space-y-3">
        <div class="h-5 w-32 bg-gray-200 rounded animate-pulse"></div>
        <div class="h-4 w-full bg-gray-200 rounded animate-pulse"></div>
        <div class="h-4 w-3/4 bg-gray-200 rounded animate-pulse"></div>
      </div>
    </div>

    <!-- Breadcrumb -->
    <div v-if="!loading" class="mb-6 text-sm">
      <router-link to="/leagues" class="text-blue-600 hover:text-blue-800 font-semibold">Leagues</router-link>
      <span class="mx-2 text-gray-400">/</span>
      <span class="text-gray-600">{{ league?.name }}</span>
    </div>

    <div v-if="!loading" class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold text-blue-600">{{ league?.name }}</h1>
      <div class="flex gap-3">
        <router-link
          :to="`/leagues/${leagueId}/edit`"
          aria-label="Edit league"
          class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-4 py-2 rounded font-semibold transition cursor-pointer"
        >
          Edit League
        </router-link>
        <button
          @click="handleDelete"
          aria-label="Delete league"
          class="bg-red-100 hover:bg-red-200 text-red-700 px-4 py-2 rounded font-semibold transition cursor-pointer"
        >
          Delete League
        </button>
      </div>
    </div>

    <!-- Teams in League -->
    <div v-if="!loading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
      <div class="flex justify-between items-center p-6 border-b border-gray-200">
        <h2 class="text-lg font-bold text-blue-600">Teams in {{ league?.name }}</h2>
        <router-link
          :to="`/teams/create?leagueId=${leagueId}`"
          class="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded font-semibold transition cursor-pointer"
        >
          + Add Team
        </router-link>
      </div>
      <table class="w-full">
        <thead class="bg-blue-50">
          <tr>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Team Name</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="team in teams" :key="team.id" class="border-t border-gray-200 hover:bg-blue-50 transition">
            <td class="px-6 py-4">
              <router-link :to="`/teams/${team.id}`" class="text-blue-600 hover:text-blue-800 font-semibold">
                {{ team.name }}
              </router-link>
            </td>
            <td class="px-6 py-4 flex gap-2">
              <router-link
                :to="`/teams/${team.id}/edit`"
                :aria-label="`Edit ${team.name}`"
                class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-3 py-1 rounded font-semibold transition cursor-pointer"
              >
                Edit
              </router-link>
              <button
                @click="deleteTeam(team.id)"
                :aria-label="`Delete ${team.name}`"
                class="bg-red-100 hover:bg-red-200 text-red-700 px-3 py-1 rounded font-semibold transition cursor-pointer"
              >
                Delete
              </button>
            </td>
          </tr>
        </tbody>
      </table>
      <p v-if="teams.length === 0" class="p-6 text-gray-400 text-center">No teams in this league yet</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { leagueAPI, teamAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'

const toast = useToast()
const route = useRoute()
const router = useRouter()
const leagueId = route.params.id
const league = ref(null)
const teams = ref([])
const loading = ref(true)

onMounted(async () => {
  await Promise.all([loadLeague(), loadTeams()])
  loading.value = false
})

const loadLeague = async () => {
  try {
    const response = await leagueAPI.getLeagueById(leagueId)
    league.value = response.data
  } catch (error) {
    console.error('Failed to fetch league:', error)
  }
}

const loadTeams = async () => {
  try {
    const response = await teamAPI.getTeams({ leagueId })
    teams.value = response.data
  } catch (error) {
    console.error('Failed to fetch teams:', error)
  }
}

const handleDelete = async () => {
  if (confirm('Are you sure you want to delete this league?')) {
    try {
      await leagueAPI.deleteLeague(leagueId)
      toast.success('League deleted')
      router.push('/leagues')
    } catch (error) {
      console.error('Failed to delete league:', error)
      const msg = error.response?.data?.detail || error.response?.data?.title || 'Failed to delete league'
      toast.error(msg)
    }
  }
}

const deleteTeam = async (id) => {
  if (confirm('Are you sure you want to delete this team?')) {
    try {
      await teamAPI.deleteTeam(id)
      toast.success('Team deleted')
      await loadTeams()
    } catch (error) {
      console.error('Failed to delete team:', error)
      const msg = error.response?.data?.detail || error.response?.data?.title || 'Failed to delete team'
      toast.error(msg)
    }
  }
}
</script>
