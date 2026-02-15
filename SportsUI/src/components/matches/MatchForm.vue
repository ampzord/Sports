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
          <label for="match-league" class="block text-sm font-semibold text-gray-700 mb-2">League (filter teams)</label>
          <CustomSelect
            id="match-league"
            v-model="selectedLeagueId"
            :options="leagueOptions"
            placeholder="Select League"
            :disabled="!leagues?.length"
            required
            @change="onLeagueChange"
          />
        </div>

        <div>
          <label for="match-home-team" class="block text-sm font-semibold text-gray-700 mb-2">Home Team</label>
          <CustomSelect
            id="match-home-team"
            v-model="form.homeTeamId"
            :options="homeTeamSelectOptions"
            placeholder="Select Home Team"
            :disabled="leagueTeams.length === 0"
            required
          />
        </div>

        <div>
          <label for="match-away-team" class="block text-sm font-semibold text-gray-700 mb-2">Away Team</label>
          <CustomSelect
            id="match-away-team"
            v-model="form.awayTeamId"
            :options="awayTeamSelectOptions"
            placeholder="Select Away Team"
            :disabled="leagueTeams.length === 0"
            required
          />
        </div>

        <div>
          <label for="match-total-passes" class="block text-sm font-semibold text-gray-700 mb-2">Total Passes (optional)</label>
          <input
            id="match-total-passes"
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
import { ref, computed, watch } from 'vue'
import { useRouter } from 'vue-router'
import { useMatch, useTeams, useLeagues, useCreateMatch, useUpdateMatch } from '../../composables/useMatchQueries'
import { useToast } from '../../composables/useToast'
import { getApiErrorMessage } from '../../utils/errors'
import { useRouteId } from '../../composables/useRouteId'
import CustomSelect from '../ui/CustomSelect.vue'
import type { MatchFormData } from '../../types'

const toast = useToast()
const router = useRouter()
const routeId = useRouteId()

const isEdit = computed(() => !!routeId.value)
const selectedLeagueId = ref<string | number>('')
const form = ref<MatchFormData>({ homeTeamId: '', awayTeamId: '', totalPasses: null })

const { data: teams } = useTeams()
const { data: leagues } = useLeagues()
const { data: existingMatch } = useMatch(computed(() => (isEdit.value ? routeId.value : null)))

const createMutation = useCreateMatch()
const updateMutation = useUpdateMatch()
const loading = computed(() => createMutation.isPending.value || updateMutation.isPending.value)

// When existing match loads (and teams are ready), populate form
watch(
  [existingMatch, teams],
  ([match, teamList]) => {
    if (match && teamList?.length) {
      form.value = {
        homeTeamId: match.homeTeamId,
        awayTeamId: match.awayTeamId,
        totalPasses: match.totalPasses,
      }
      const homeTeam = teamList.find((t) => t.id === match.homeTeamId || String(t.id) === String(match.homeTeamId))
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

const leagueOptions = computed(() => (leagues.value || []).map((l) => ({ value: l.id, label: l.name })))

const homeTeamSelectOptions = computed(() => homeTeamOptions.value.map((t) => ({ value: t.id, label: t.name })))

const awayTeamSelectOptions = computed(() => awayTeamOptions.value.map((t) => ({ value: t.id, label: t.name })))

const onLeagueChange = () => {
  form.value.homeTeamId = ''
  form.value.awayTeamId = ''
}

const handleSubmit = async () => {
  try {
    const payload = {
      leagueId: selectedLeagueId.value,
      homeTeamId: Number(form.value.homeTeamId),
      awayTeamId: Number(form.value.awayTeamId),
      totalPasses: form.value.totalPasses != null ? form.value.totalPasses : undefined,
    }
    if (isEdit.value) {
      await updateMutation.mutateAsync({ id: routeId.value, data: payload })
      toast.success('Match updated')
      router.push(`/matches/${routeId.value}`)
    } else {
      const created = await createMutation.mutateAsync(payload)
      toast.success('Match created')
      router.push(`/matches/${created.id}`)
    }
  } catch (err: unknown) {
    console.error('Failed to save match:', err)
    toast.error(getApiErrorMessage(err, 'Failed to save match'))
  }
}
</script>
