<template>
  <div class="p-8">
    <!-- Loading state -->
    <div v-if="loading" role="status" aria-label="Loading player" class="space-y-4">
      <div class="h-8 w-48 bg-gray-200 rounded animate-pulse"></div>
      <div class="bg-white rounded-lg shadow p-8 border border-gray-200 space-y-4">
        <div class="h-6 w-40 bg-gray-200 rounded animate-pulse"></div>
        <div class="grid grid-cols-2 gap-6">
          <div class="h-20 bg-gray-200 rounded animate-pulse"></div>
          <div class="h-20 bg-gray-200 rounded animate-pulse"></div>
        </div>
      </div>
    </div>

    <!-- Breadcrumb -->
    <div v-if="!loading" class="mb-6 text-sm">
      <router-link to="/players" class="text-blue-600 hover:text-blue-800 font-semibold">Players</router-link>
      <span class="mx-2 text-gray-400">/</span>
      <span class="text-gray-600">{{ player?.name }}</span>
    </div>

    <div v-if="!loading" class="bg-white rounded-lg shadow p-8 border border-gray-200">
      <div class="flex justify-between items-center mb-6">
        <h2 class="text-2xl font-bold text-blue-600">{{ player?.name }}</h2>
        <div class="flex gap-3">
          <router-link
            :to="`/players/${playerId}/edit`"
            class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-4 py-2 rounded font-semibold transition cursor-pointer"
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

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { playerAPI, teamAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'
import { getApiErrorMessage } from '../../utils/errors'
import { useRouteId } from '../../composables/useRouteId'
import type { Player } from '../../types'

const toast = useToast()
const router = useRouter()
const playerId = useRouteId()
const player = ref<Player | null>(null)
const teamName = ref('')
const loading = ref(true)

onMounted(async () => {
  await loadPlayer()
  loading.value = false
})

const loadPlayer = async () => {
  try {
    const response = await playerAPI.getPlayerById(playerId.value)
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
      await playerAPI.deletePlayer(playerId.value)
      toast.success('Player deleted')
      router.push('/players')
    } catch (err: unknown) {
      console.error('Failed to delete player:', err)
      toast.error(getApiErrorMessage(err, 'Failed to delete player'))
    }
  }
}
</script>
