<template>
  <div class="max-w-xl mx-auto py-8">
    <div class="mb-6">
      <router-link to="/teams" class="text-blue-600 hover:text-blue-800 font-semibold"> ← Back to Teams </router-link>
    </div>

    <div class="bg-white rounded-lg shadow p-8 border border-gray-200">
      <h2 class="text-2xl font-bold text-blue-600 mb-6">
        {{ isEdit ? 'Edit Team' : 'Create Team' }}
      </h2>

      <form @submit.prevent="handleSubmit" class="space-y-6">
        <div>
          <label for="team-name" class="block text-sm font-semibold text-gray-700 mb-2">Team Name</label>
          <input
            id="team-name"
            v-model="form.name"
            type="text"
            placeholder="Enter team name"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition"
          />
        </div>

        <div>
          <label for="team-league" class="block text-sm font-semibold text-gray-700 mb-2">League</label>
          <CustomSelect
            id="team-league"
            v-model="form.leagueId"
            :options="leagueOptions"
            placeholder="Select League"
            :disabled="leagues.length === 0"
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
            to="/teams"
            class="flex-1 px-4 py-2 bg-gray-100 hover:bg-gray-200 text-gray-700 font-semibold rounded transition text-center cursor-pointer"
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
import { teamAPI, leagueAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'
import CustomSelect from '../ui/CustomSelect.vue'

const toast = useToast()
const route = useRoute()
const router = useRouter()

const isEdit = computed(() => !!route.params.id)
const loading = ref(false)
const leagues = ref([])
const form = ref({ name: '', leagueId: '' })

const leagueOptions = computed(() => leagues.value.map((l) => ({ value: l.id, label: l.name })))

onMounted(async () => {
  try {
    const leaguesRes = await leagueAPI.getLeagues()
    leagues.value = leaguesRes.data
  } catch (error) {
    console.error('Failed to load leagues:', error)
  }

  if (isEdit.value) {
    try {
      const response = await teamAPI.getTeamById(route.params.id)
      form.value = { name: response.data.name, leagueId: response.data.leagueId }
    } catch (error) {
      console.error('Failed to load team:', error)
    }
  } else if (route.query.leagueId) {
    form.value.leagueId = Number(route.query.leagueId)
  }
})

const handleSubmit = async () => {
  loading.value = true
  try {
    if (isEdit.value) {
      await teamAPI.updateTeam(route.params.id, form.value)
      toast.success('Team updated')
      router.push(`/teams/${route.params.id}`)
    } else {
      const response = await teamAPI.addTeam({ name: form.value.name, leagueId: form.value.leagueId })
      toast.success('Team created')
      router.push(`/teams/${response.data.id}`)
    }
  } catch (error) {
    console.error('Failed to save team:', error)
    const msg = error.response?.data?.detail || error.response?.data?.title || 'Failed to save team'
    toast.error(msg)
  } finally {
    loading.value = false
  }
}
</script>
