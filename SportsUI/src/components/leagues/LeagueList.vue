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

    <div v-if="!loading" class="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
      <table class="w-full">
        <thead class="bg-blue-50">
          <tr>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">League Name</th>
            <th class="px-6 py-3 text-left text-blue-600 font-bold">Actions</th>
          </tr>
        </thead>
        <TransitionGroup tag="tbody" appear @before-enter="onBeforeEnter" @enter="onEnter">
          <tr
            v-for="(league, index) in leagues"
            :key="league.id"
            :data-index="index"
            class="border-t border-gray-200 hover:bg-blue-50 hover:shadow-[inset_3px_0_0_0_#3b82f6] transition-all duration-200"
          >
            <td class="px-6 py-4">
              <router-link :to="`/leagues/${league.id}`" class="text-blue-600 hover:text-blue-800 font-semibold">
                {{ league.name }}
              </router-link>
            </td>
            <td class="px-6 py-4 flex gap-2">
              <router-link
                :to="`/leagues/${league.id}/edit`"
                class="bg-blue-100 hover:bg-blue-200 text-blue-700 px-3 py-1 rounded font-semibold transition"
              >
                Edit
              </router-link>
              <button
                @click="deleteLeague(league.id)"
                :disabled="deletingId === league.id"
                class="bg-red-100 hover:bg-red-200 disabled:opacity-50 text-red-700 px-3 py-1 rounded font-semibold transition cursor-pointer inline-flex items-center gap-1.5"
              >
                <span
                  v-if="deletingId === league.id"
                  class="inline-block w-3 h-3 border-2 border-red-700 border-t-transparent rounded-full animate-spin"
                ></span>
                Delete
              </button>
            </td>
          </tr>
        </TransitionGroup>
      </table>
      <p v-if="leagues.length === 0" class="p-6 text-gray-400 text-center">No leagues found</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { leagueAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'
import { useStaggerAnimation } from '../../composables/useStaggerAnimation'

const toast = useToast()
const { onBeforeEnter, onEnter } = useStaggerAnimation()
const leagues = ref([])
const loading = ref(true)
const deletingId = ref(null)

const fetchLeagues = async () => {
  try {
    const response = await leagueAPI.getLeagues()
    leagues.value = response.data
  } catch (error) {
    console.error('Failed to fetch leagues:', error)
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

<style scoped></style>
