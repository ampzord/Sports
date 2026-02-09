<template>
  <div class="p-8">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold text-blue-600">Leagues</h1>
      <router-link
        to="/leagues/create"
        class="bg-blue-600 hover:bg-blue-700 text-white px-5 py-2 rounded font-semibold transition"
      >
        + Create League
      </router-link>
    </div>

    <!-- Loading skeleton -->
    <div v-if="loading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
      <table class="w-full">
        <thead class="bg-blue-50">
          <tr>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">League Name</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="n in 6" :key="n" class="border-t border-gray-200">
            <td class="px-6 py-4"><div class="h-4 w-40 bg-gray-200 rounded animate-pulse"></div></td>
            <td class="px-6 py-4"><div class="h-4 w-28 bg-gray-200 rounded animate-pulse"></div></td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Virtualized Leagues List -->
    <div v-if="!loading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
      <!-- Sticky header -->
      <div class="bg-blue-50 grid grid-cols-[1fr_1fr]">
        <div class="px-6 py-3 text-left text-blue-600 font-bold">League Name</div>
        <div class="px-6 py-3 text-left text-blue-600 font-bold">Actions</div>
      </div>

      <!-- Scrollable virtual container -->
      <div ref="scrollEl" class="overflow-auto" :style="{ maxHeight: '70vh' }">
        <div :style="{ height: `${virtualizer.getTotalSize()}px`, position: 'relative' }">
          <div
            v-for="row in virtualizer.getVirtualItems()"
            :key="leagueRows[row.index].id"
            :style="{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: `${row.size}px`,
              transform: `translateY(${row.start}px)`,
            }"
            class="grid grid-cols-[1fr_1fr] items-center border-t border-gray-200 hover:bg-blue-50 hover:shadow-[inset_3px_0_0_0_#3b82f6] transition-all duration-200"
          >
            <div class="px-6 py-4">
              <router-link :to="`/leagues/${leagueRows[row.index].id}`" class="text-blue-600 hover:text-blue-800 font-semibold">
                {{ leagueRows[row.index].name }}
              </router-link>
            </div>
            <div class="px-6 py-4 flex gap-2">
              <router-link
                :to="`/leagues/${leagueRows[row.index].id}/edit`"
                class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-3 py-1 rounded font-semibold transition cursor-pointer"
              >
                Edit
              </router-link>
              <button
                @click="deleteLeague(leagueRows[row.index].id)"
                :disabled="deletingId === leagueRows[row.index].id"
                class="bg-red-100 hover:bg-red-200 disabled:opacity-50 text-red-700 px-3 py-1 rounded font-semibold transition cursor-pointer inline-flex items-center gap-1.5"
              >
                <span
                  v-if="deletingId === leagueRows[row.index].id"
                  class="inline-block w-3 h-3 border-2 border-red-700 border-t-transparent rounded-full animate-spin"
                ></span>
                Delete
              </button>
            </div>
          </div>
        </div>
      </div>

      <p v-if="leagueRows.length === 0" class="p-6 text-gray-400 text-center">No leagues found</p>
    </div>

    <!-- Error state -->
    <div v-if="error" class="bg-red-50 border border-red-200 rounded-lg p-6 text-center">
      <p class="text-red-600 font-semibold">{{ error }}</p>
      <button
        @click="fetchLeagues"
        class="mt-3 px-4 py-2 bg-red-100 hover:bg-red-200 text-red-700 rounded font-semibold transition cursor-pointer"
      >
        Retry
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useVirtualizer } from '@tanstack/vue-virtual'
import { leagueAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'

const ROW_HEIGHT = 52

const toast = useToast()
const leagues = ref([])
const loading = ref(true)
const error = ref(null)
const deletingId = ref(null)
const scrollEl = ref(null)

const leagueRows = computed(() => leagues.value)

const virtualizer = useVirtualizer({
  get count() {
    return leagueRows.value.length
  },
  getScrollElement: () => scrollEl.value,
  estimateSize: () => ROW_HEIGHT,
  overscan: 20,
})

const fetchLeagues = async () => {
  try {
    error.value = null
    const response = await leagueAPI.getLeagues()
    leagues.value = response.data
  } catch (err) {
    console.error('Failed to fetch leagues:', err)
    error.value = 'Failed to load leagues. Please try again.'
  } finally {
    loading.value = false
  }
}

const deleteLeague = async (id) => {
  if (confirm('Are you sure you want to delete this league?')) {
    deletingId.value = id
    try {
      await leagueAPI.deleteLeague(id)
      toast.success('League deleted')
      await fetchLeagues()
    } catch (error) {
      console.error('Failed to delete league:', error)
      const msg = error.response?.data?.detail || error.response?.data?.title || 'Failed to delete league'
      toast.error(msg)
    } finally {
      deletingId.value = null
    }
  }
}

onMounted(fetchLeagues)
</script>
