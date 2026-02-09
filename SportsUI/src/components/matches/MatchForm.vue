<template>
  <div class="max-w-xl mx-auto py-8">
    <div class="mb-6">
      <router-link to="/matches" class="text-blue-600 hover:text-blue-800 font-semibold"> ← Back to Matches </router-link>
    </div>

    <div class="bg-white rounded-lg shadow p-8 border border-gray-200">
      <h2 class="text-2xl font-bold text-blue-600 mb-6">
        {{ isEdit ? 'Edit Match' : 'Create Match' }}
      </h2>

      <form @submit.prevent="handleSubmit" class="space-y-6">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">League (filter teams)</label>
          <select
            v-model="selectedLeagueId"
            @change="onLeagueChange"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition cursor-pointer"
          >
            <option value="">Select League</option>
            <option v-for="league in leagues" :key="league.id" :value="league.id">
              {{ league.name }}
            </option>
          </select>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Home Team</label>
          <select
            v-model="form.homeTeamId"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition cursor-pointer"
          >
            <option value="">Select Home Team</option>
            <option v-for="team in homeTeamOptions" :key="team.id" :value="team.id">
              {{ team.name }}
            </option>
          </select>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Away Team</label>
          <select
            v-model="form.awayTeamId"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition cursor-pointer"
          >
            <option value="">Select Away Team</option>
            <option v-for="team in awayTeamOptions" :key="team.id" :value="team.id">
              {{ team.name }}
            </option>
          </select>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Total Passes (optional)</label>
          <input
            v-model.number="form.totalPasses"
            type="number"
            min="0"
            placeholder="Enter total passes"
            class="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition"
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
            to="/matches"
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
import { ref, computed, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useMatch, useTeams, useLeagues, useCreateMatch, useUpdateMatch } from '../../composables/useMatchQueries'
import { useToast } from '../../composables/useToast'

const toast = useToast()
const route = useRoute()
const router = useRouter()

const isEdit = computed(() => !!route.params.id)
const selectedLeagueId = ref('')
const form = ref({ homeTeamId: '', awayTeamId: '', totalPasses: null })

const { data: teams } = useTeams()
const { data: leagues } = useLeagues()
const { data: existingMatch } = useMatch(computed(() => (isEdit.value ? route.params.id : null)))

const createMutation = useCreateMatch()
const updateMutation = useUpdateMatch()
const loading = computed(() => createMutation.isPending.value || updateMutation.isPending.value)

// When existing match loads, populate form
watch(
  existingMatch,
  (match) => {
    if (match) {
      form.value = {
        homeTeamId: match.homeTeamId,
        awayTeamId: match.awayTeamId,
        totalPasses: match.totalPasses,
      }
      const homeTeam = (teams.value || []).find((t) => t.id === match.homeTeamId || String(t.id) === String(match.homeTeamId))
      if (homeTeam) {
        selectedLeagueId.value = homeTeam.leagueId
      }
    }
  },
  { immediate: true },
)

const leagueTeams = computed(() => {
  if (!selectedLeagueId.value) return []
  return (teams.value || []).filter((t) => String(t.leagueId) === String(selectedLeagueId.value))
})

const homeTeamOptions = computed(() => {
  return leagueTeams.value.filter((t) => String(t.id) !== String(form.value.awayTeamId))
})

const awayTeamOptions = computed(() => {
  return leagueTeams.value.filter((t) => String(t.id) !== String(form.value.homeTeamId))
})

const onLeagueChange = () => {
  form.value.homeTeamId = ''
  form.value.awayTeamId = ''
}

const handleSubmit = async () => {
  try {
    const payload = {
      homeTeamId: form.value.homeTeamId,
      awayTeamId: form.value.awayTeamId,
      totalPasses: form.value.totalPasses || undefined,
    }
    if (isEdit.value) {
      await updateMutation.mutateAsync({ id: route.params.id, data: payload })
      toast.success('Match updated')
      router.push(`/matches/${route.params.id}`)
    } else {
      await createMutation.mutateAsync(payload)
      toast.success('Match created')
      router.push('/matches')
    }
  } catch (error) {
    console.error('Failed to save match:', error)
    toast.error(error.message || 'Failed to save match')
  }
}
</script>
