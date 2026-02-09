<template>
  <div class="p-8">
    <!-- Breadcrumb -->
    <div class="mb-6 text-sm">
      <router-link to="/players" class="text-blue-600 hover:text-blue-800 font-semibold">Players</router-link>
      <span class="mx-2 text-gray-400">/</span>
      <span class="text-gray-600">{{ player?.name }}</span>
    </div>

    <div class="bg-white rounded-lg shadow p-8 border border-gray-200">
      <div class="flex justify-between items-center mb-6">
        <h2 class="text-2xl font-bold text-blue-600">{{ player?.name }}</h2>
        <div class="flex gap-3">
          <router-link
            :to="`/players/${playerId}/edit`"
            class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-4 py-2 rounded font-semibold transition"
          >
            Edit Player
          </router-link>
          <button
            @click="handleDelete"
            class="bg-red-100 hover:bg-red-200 text-red-700 px-4 py-2 rounded font-semibold transition cursor-pointer"
          >
            Delete Player
          </button>
        </div>
      </div>

      <div class="grid grid-cols-2 gap-6">
        <div class="bg-blue-50 p-4 rounded border-l-4 border-blue-500">
          <p class="text-xs text-gray-500 uppercase font-bold">Position</p>
          <p class="text-lg font-bold text-blue-600 mt-1">
            {{ player?.position || 'Not specified' }}
          </p>
        </div>
        <div class="bg-blue-50 p-4 rounded border-l-4 border-blue-500">
          <p class="text-xs text-gray-500 uppercase font-bold">Team</p>
          <p class="text-lg font-bold mt-1">
            <router-link v-if="player?.teamId" :to="`/teams/${player.teamId}`" class="text-blue-600 hover:text-blue-800">
              {{ teamName || `Team #${player.teamId}` }}
            </router-link>
            <span v-else class="text-gray-500">Free Agent</span>
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { playerAPI, teamAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'

const toast = useToast()
const route = useRoute()
const router = useRouter()
const playerId = route.params.id
const player = ref(null)
const teamName = ref('')

onMounted(async () => {
  await loadPlayer()
})

const loadPlayer = async () => {
  try {
    const response = await playerAPI.getPlayerById(playerId)
    player.value = response.data
    if (player.value.teamId) {
      try {
        const teamResponse = await teamAPI.getTeamById(player.value.teamId)
        teamName.value = teamResponse.data.name
      } catch (e) {
        console.error('Failed to fetch team:', e)
      }
    }
  } catch (error) {
    console.error('Failed to fetch player:', error)
  }
}

const handleDelete = async () => {
  if (confirm('Are you sure you want to delete this player?')) {
    try {
      await playerAPI.deletePlayer(playerId)
      toast.success('Player deleted')
      router.push('/players')
    } catch (error) {
      console.error('Failed to delete player:', error)
      const msg = error.response?.data?.detail || error.response?.data?.title || 'Failed to delete player'
      toast.error(msg)
    }
  }
}
</script>

<style scoped></style>
