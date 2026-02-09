<template>
  <div class="p-8">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold text-blue-600">Teams</h1>
      <router-link to="/teams/create" class="bg-blue-600 hover:bg-blue-700 text-white px-5 py-2 rounded font-semibold transition">
        + Create Team
      </router-link>
    </div>

    <!-- Loading skeleton -->
    <div v-if="loading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
      <table class="w-full">
        <thead class="bg-blue-50">
          <tr>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Team Name</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">League</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="n in 8" :key="n" class="border-t border-gray-200">
            <td class="px-6 py-4"><div class="h-4 w-36 bg-gray-200 rounded animate-pulse"></div></td>
            <td class="px-6 py-4"><div class="h-4 w-24 bg-gray-200 rounded animate-pulse"></div></td>
            <td class="px-6 py-4"><div class="h-4 w-28 bg-gray-200 rounded animate-pulse"></div></td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Virtualized Teams List -->
    <div v-if="!loading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
      <!-- Sticky header -->
      <div class="bg-blue-50 grid grid-cols-[1fr_1fr_1fr]">
        <div class="px-6 py-3 text-left text-blue-600 font-bold">Team Name</div>
        <div class="px-6 py-3 text-left text-blue-600 font-bold">League</div>
        <div class="px-6 py-3 text-left text-blue-600 font-bold">Actions</div>
      </div>

      <!-- Scrollable virtual container -->
      <div ref="scrollEl" class="overflow-auto" :style="{ maxHeight: '70vh' }">
        <div :style="{ height: `${virtualizer.getTotalSize()}px`, position: 'relative' }">
          <div
            v-for="row in virtualizer.getVirtualItems()"
            :key="teamRows[row.index].id"
            :style="{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: `${row.size}px`,
              transform: `translateY(${row.start}px)`,
            }"
            class="grid grid-cols-[1fr_1fr_1fr] items-center border-t border-gray-200 hover:bg-blue-50 hover:shadow-[inset_3px_0_0_0_#3b82f6] transition-all duration-200"
          >
            <div class="px-6 py-4">
              <router-link :to="`/teams/${teamRows[row.index].id}`" class="text-blue-600 hover:text-blue-800 font-semibold">
                {{ teamRows[row.index].name }}
              </router-link>
            </div>
            <div class="px-6 py-4 text-gray-600">{{ getLeagueName(teamRows[row.index].leagueId) }}</div>
            <div class="px-6 py-4 flex gap-2">
              <router-link
                :to="`/teams/${teamRows[row.index].id}/edit`"
                class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-3 py-1 rounded font-semibold transition cursor-pointer"
              >
                Edit
              </router-link>
              <button
                @click="deleteTeam(teamRows[row.index].id)"
                :disabled="deletingId === teamRows[row.index].id"
                class="bg-red-100 hover:bg-red-200 disabled:opacity-50 text-red-700 px-3 py-1 rounded font-semibold transition cursor-pointer inline-flex items-center gap-1.5"
              >
                <span
                  v-if="deletingId === teamRows[row.index].id"
                  class="inline-block w-3 h-3 border-2 border-red-700 border-t-transparent rounded-full animate-spin"
                ></span>
                Delete
              </button>
            </div>
          </div>
        </div>
      </div>

      <p v-if="teamRows.length === 0" class="p-6 text-gray-400 text-center">No teams found</p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useVirtualizer } from '@tanstack/vue-virtual'
import { teamAPI, leagueAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'

const ROW_HEIGHT = 52

const toast = useToast()
const teams = ref([])
const leagues = ref([])
const loading = ref(true)
const deletingId = ref(null)
const scrollEl = ref(null)

const teamRows = computed(() => teams.value)

const virtualizer = useVirtualizer({
  get count() {
    return teamRows.value.length
  },
  getScrollElement: () => scrollEl.value,
  estimateSize: () => ROW_HEIGHT,
  overscan: 20,
})

onMounted(async () => {
  try {
    const [teamsRes, leaguesRes] = await Promise.all([teamAPI.getTeams(), leagueAPI.getLeagues()])
    teams.value = teamsRes.data
    leagues.value = leaguesRes.data
  } catch (error) {
    console.error('Failed to fetch data:', error)
  } finally {
    loading.value = false
  }
})

const getLeagueName = (leagueId) => {
  const league = leagues.value.find((l) => l.id === leagueId)
  return league?.name || 'Unknown'
}

const deleteTeam = async (id) => {
  if (confirm('Are you sure you want to delete this team?')) {
    deletingId.value = id
    try {
      await teamAPI.deleteTeam(id)
      toast.success('Team deleted')
      const response = await teamAPI.getTeams()
      teams.value = response.data
    } catch (error) {
      console.error('Failed to delete team:', error)
      const msg = error.response?.data?.detail || error.response?.data?.title || 'Failed to delete team'
      toast.error(msg)
    } finally {
      deletingId.value = null
    }
  }
}
</script>
