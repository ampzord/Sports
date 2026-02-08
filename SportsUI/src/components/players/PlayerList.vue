<template>
  <div class="p-8">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold text-blue-600">Players</h1>
      <router-link
        to="/players/create"
        class="bg-blue-600 hover:bg-blue-700 text-white px-5 py-2 rounded font-semibold transition"
      >
        + Create Player
      </router-link>
    </div>

    <!-- Loading skeleton -->
    <div v-if="loading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
      <table class="w-full">
        <thead class="bg-blue-50">
          <tr>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Player Name</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Position</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Team</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="n in 8" :key="n" class="border-t border-gray-200">
            <td class="px-6 py-4"><div class="h-4 w-32 bg-gray-200 rounded animate-pulse"></div></td>
            <td class="px-6 py-4"><div class="h-4 w-20 bg-gray-200 rounded animate-pulse"></div></td>
            <td class="px-6 py-4"><div class="h-4 w-24 bg-gray-200 rounded animate-pulse"></div></td>
            <td class="px-6 py-4"><div class="h-4 w-28 bg-gray-200 rounded animate-pulse"></div></td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-if="!loading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
      <table class="w-full">
        <thead class="bg-blue-50">
          <tr>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Player Name</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Position</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Team</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Actions</th>
          </tr>
        </thead>
        <TransitionGroup tag="tbody" appear @before-enter="onBeforeEnter" @enter="onEnter">
          <tr
            v-for="(player, index) in players"
            :key="player.id"
            :data-index="index"
            class="border-t border-gray-200 hover:bg-blue-50 hover:shadow-[inset_3px_0_0_0_#3b82f6] transition-all duration-200"
          >
            <td class="px-6 py-4">
              <router-link :to="`/players/${player.id}`" class="text-blue-600 hover:text-blue-800 font-semibold">
                {{ player.name }}
              </router-link>
            </td>
            <td class="px-6 py-4">
              <span class="bg-blue-100 text-blue-700 px-3 py-1 rounded-full text-sm font-semibold">
                {{ player.position || '-' }}
              </span>
            </td>
            <td class="px-6 py-4 text-gray-600">
              {{ getTeamName(player.teamId) }}
            </td>
            <td class="px-6 py-4 flex gap-2">
              <router-link
                :to="`/players/${player.id}/edit`"
                class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-3 py-1 rounded font-semibold transition"
              >
                Edit
              </router-link>
              <button
                @click="deletePlayer(player.id)"
                :disabled="deletingId === player.id"
                class="bg-red-100 hover:bg-red-200 disabled:opacity-50 text-red-700 px-3 py-1 rounded font-semibold transition cursor-pointer inline-flex items-center gap-1.5"
              >
                <span
                  v-if="deletingId === player.id"
                  class="inline-block w-3 h-3 border-2 border-red-700 border-t-transparent rounded-full animate-spin"
                ></span>
                Delete
              </button>
            </td>
          </tr>
        </TransitionGroup>
      </table>
      <p v-if="players.length === 0" class="p-6 text-gray-400 text-center">No players found</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { playerAPI, teamAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'
import { useStaggerAnimation } from '../../composables/useStaggerAnimation'

const toast = useToast()
const { onBeforeEnter, onEnter } = useStaggerAnimation()
const players = ref([])
const teams = ref([])
const loading = ref(true)
const deletingId = ref(null)

const fetchPlayers = async () => {
  try {
    const [playersRes, teamsRes] = await Promise.all([playerAPI.getPlayers(), teamAPI.getTeams()])
    players.value = playersRes.data
    teams.value = teamsRes.data
  } catch (error) {
    console.error('Failed to fetch players:', error)
  } finally {
    loading.value = false
  }
}

const getTeamName = (teamId) => {
  if (!teamId) return 'Free Agent'
  const team = teams.value.find((t) => t.id === teamId)
  return team?.name || 'Unknown'
}

const deletePlayer = async (id) => {
  if (confirm('Are you sure you want to delete this player?')) {
    deletingId.value = id
    try {
      await playerAPI.deletePlayer(id)
      toast.success('Player deleted')
      await fetchPlayers()
    } catch (error) {
      console.error('Failed to delete player:', error)
      const msg = error.response?.data?.detail || error.response?.data?.title || 'Failed to delete player'
      toast.error(msg)
    } finally {
      deletingId.value = null
    }
  }
}

onMounted(fetchPlayers)
</script>

<style scoped></style>
