<template>
  <div class="max-w-xl mx-auto py-8">
    <div class="mb-6">
      <router-link to="/leagues" class="text-blue-600 hover:text-blue-800 font-semibold"> ← Back to Leagues </router-link>
    </div>

    <div class="bg-white rounded-lg shadow p-8 border border-gray-200">
      <h2 class="text-2xl font-bold text-blue-600 mb-6">
        {{ isEdit ? 'Edit League' : 'Create League' }}
      </h2>

      <form @submit.prevent="handleSubmit" class="space-y-6">
        <div>
          <label for="league-name" class="block text-sm font-semibold text-gray-700 mb-2">League Name</label>
          <input
            id="league-name"
            v-model="form.name"
            type="text"
            placeholder="Enter league name"
            required
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
            to="/leagues"
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
import { useRouter } from 'vue-router'
import { leagueAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'
import { getApiErrorMessage } from '../../utils/errors'
import { useRouteId } from '../../composables/useRouteId'

const toast = useToast()
const router = useRouter()
const routeId = useRouteId()

const isEdit = computed(() => !!routeId.value)
const loading = ref(false)
const form = ref({ name: '' })

onMounted(async () => {
  if (isEdit.value) {
    try {
      const response = await leagueAPI.getLeagueById(routeId.value)
      form.value = { name: response.data.name }
    } catch (error) {
      console.error('Failed to load league:', error)
    }
  }
})

const handleSubmit = async () => {
  loading.value = true
  try {
    if (isEdit.value) {
      await leagueAPI.updateLeague(routeId.value, form.value)
      toast.success('League updated')
      router.push(`/leagues/${routeId.value}`)
    } else {
      const response = await leagueAPI.addLeague(form.value)
      toast.success('League created')
      router.push(`/leagues/${response.data.id}`)
    }
  } catch (err: unknown) {
    console.error('Failed to save league:', err)
    toast.error(getApiErrorMessage(err, 'Failed to save league'))
  } finally {
    loading.value = false
  }
}
</script>
