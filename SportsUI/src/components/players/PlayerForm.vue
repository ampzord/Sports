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
          <label for="player-name" class="block text-sm font-semibold text-gray-700 mb-2">Player Name</label>
          <input
            id="player-name"
            v-model="form.name"
            type="text"
            placeholder="Enter player name"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition"
          />
        </div>

        <div>
          <label for="player-position" class="block text-sm font-semibold text-gray-700 mb-2">Position</label>
          <CustomSelect
            id="player-position"
            v-model="form.position"
            :options="positionOptions"
            placeholder="Select Position"
            required
          />
        </div>

        <div>
          <label for="player-team" class="block text-sm font-semibold text-gray-700 mb-2">Team</label>
          <CustomSelect
            id="player-team"
            v-model="form.teamId"
            :options="teamOptions"
            placeholder="Select Team"
            :disabled="teams.length === 0"
            required
          />
        </div>

        <div class="flex gap-3 pt-4 border-t border-gray-200">
          <button
            type="submit"
            :disabled="loading"
            class="flex-1 px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded transition disabled:opacity-50 cursor-pointer"
          >
            {{ isEdit ? 'Update' : 'Create' }}
          </button>
          <router-link
            to="/players"
            class="flex-1 px-4 py-2 bg-gray-100 hover:bg-gray-200 text-gray-700 font-semibold rounded transition text-center cursor-pointer"
          >
            Cancel
          </router-link>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { playerAPI, teamAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'
import { getApiErrorMessage } from '../../utils/errors'
import { useRouteId } from '../../composables/useRouteId'
import { PlayerPosition } from '../../enums/PlayerPosition'
import CustomSelect from '../ui/CustomSelect.vue'
import type { Team, PlayerFormData } from '../../types'

const toast = useToast()
const route = useRoute()
const router = useRouter()
const routeId = useRouteId()

const isEdit = computed(() => !!routeId.value)
const loading = ref(false)
const teams = ref<Team[]>([])
const form = ref<PlayerFormData>({ name: '', position: '', teamId: null })

const positionOptions = computed(() => PlayerPosition.map((p) => ({ value: p.value, label: p.label })))

const teamOptions = computed(() => teams.value.map((t) => ({ value: t.id, label: t.name })))

onMounted(async () => {
  try {
    const teamsResponse = await teamAPI.getTeams()
    teams.value = teamsResponse.data
  } catch (error) {
    console.error('Failed to load teams:', error)
  }

  if (isEdit.value) {
    try {
      const response = await playerAPI.getPlayerById(routeId.value)
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
      await playerAPI.updatePlayer(routeId.value, {
        name: form.value.name,
        position: form.value.position,
        teamId: form.value.teamId ? Number(form.value.teamId) : null,
      })
      toast.success('Player updated')
      router.push(`/players/${routeId.value}`)
    } else {
      const response = await playerAPI.addPlayer({
        name: form.value.name,
        position: form.value.position,
        teamId: form.value.teamId ? Number(form.value.teamId) : null,
      })
      toast.success('Player created')
      router.push(`/players/${response.data.id}`)
    }
  } catch (err: unknown) {
    console.error('Failed to save player:', err)
    toast.error(getApiErrorMessage(err, 'Failed to save player'))
  } finally {
    loading.value = false
  }
}
</script>
