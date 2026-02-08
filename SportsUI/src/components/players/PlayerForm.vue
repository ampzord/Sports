<template>
  <div class="max-w-xl mx-auto py-8">
    <div class="mb-6">
      <router-link to="/players" class="text-blue-600 hover:text-blue-800 font-semibold"> ← Back to Players </router-link>
    </div>

    <div class="bg-white rounded-lg shadow p-8 border border-gray-200">
      <h2 class="text-2xl font-bold text-blue-600 mb-6">
        {{ isEdit ? 'Edit Player' : 'Create Player' }}
      </h2>

      <form @submit.prevent="handleSubmit" class="space-y-6">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Player Name</label>
          <input
            v-model="form.name"
            type="text"
            placeholder="Enter player name"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition"
          />
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Position</label>
          <select
            v-model="form.position"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition cursor-pointer"
          >
            <option value="">Select Position</option>
            <option v-for="pos in PlayerPosition" :key="pos.value" :value="pos.value">
              {{ pos.label }}
            </option>
          </select>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Team</label>
          <select
            v-model="form.teamId"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition cursor-pointer"
          >
            <option value="" disabled>Select Team</option>
            <option v-for="team in teams" :key="team.id" :value="team.id">
              {{ team.name }}
            </option>
          </select>
        </div>

        <div class="flex gap-3 pt-4 border-t border-gray-200">
          <button
            type="submit"
            :disabled="loading"
            class="flex-1 px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded transition disabled:opacity-50"
          >
            {{ isEdit ? 'Update' : 'Create' }}
          </button>
          <router-link
            to="/players"
            class="flex-1 px-4 py-2 bg-gray-100 hover:bg-gray-200 text-gray-700 font-semibold rounded transition text-center"
          >
            Cancel
          </router-link>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { playerAPI, teamAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'
import { PlayerPosition } from '../../enums/PlayerPosition'

const toast = useToast()
const route = useRoute()
const router = useRouter()

const isEdit = computed(() => !!route.params.id)
const loading = ref(false)
const teams = ref([])
const form = ref({ name: '', position: '', teamId: '' })

onMounted(async () => {
  try {
    const teamsResponse = await teamAPI.getTeams()
    teams.value = teamsResponse.data
  } catch (error) {
    console.error('Failed to load teams:', error)
  }

  if (isEdit.value) {
    try {
      const response = await playerAPI.getPlayerById(route.params.id)
      form.value = {
        name: response.data.name,
        position: response.data.position || '',
        teamId: response.data.teamId || null,
      }
    } catch (error) {
      console.error('Failed to load player:', error)
    }
  } else if (route.query.teamId) {
    form.value.teamId = Number(route.query.teamId)
  }
})

const handleSubmit = async () => {
  loading.value = true
  try {
    if (isEdit.value) {
      await playerAPI.updatePlayer(route.params.id, form.value)
      toast.success('Player updated')
      router.push(`/players/${route.params.id}`)
    } else {
      await playerAPI.addPlayer({
        name: form.value.name,
        position: form.value.position,
        teamId: form.value.teamId,
      })
      toast.success('Player created')
      router.push('/players')
    }
  } catch (error) {
    console.error('Failed to save player:', error)
    const msg = error.response?.data?.detail || error.response?.data?.title || 'Failed to save player'
    toast.error(msg)
  } finally {
    loading.value = false
  }
}
</script>
