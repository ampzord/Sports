<template>
  <div class="max-w-xl mx-auto py-8">
    <div class="mb-6">
      <router-link to="/leagues" class="text-blue-600 hover:text-blue-800 font-semibold">
        ← Back to Leagues
      </router-link>
    </div>

    <div class="bg-white rounded-lg shadow p-8 border border-gray-200">
      <h2 class="text-2xl font-bold text-blue-600 mb-6">
        {{ isEdit ? 'Edit League' : 'Create League' }}
      </h2>

      <form @submit.prevent="handleSubmit" class="space-y-6">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">League Name</label>
          <input
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
            class="flex-1 px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded transition disabled:opacity-50"
          >
            {{ isEdit ? 'Update' : 'Create' }}
          </button>
          <router-link
            to="/leagues"
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
import { leagueAPI } from '../../services/api'
import { useToast } from '../../composables/useToast'

const toast = useToast()
const route = useRoute()
const router = useRouter()

const isEdit = computed(() => !!route.params.id)
const loading = ref(false)
const form = ref({ name: '' })

onMounted(async () => {
  if (isEdit.value) {
    try {
      const response = await leagueAPI.getLeagueById(route.params.id)
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
      await leagueAPI.updateLeague(route.params.id, form.value)
      toast.success('League updated')
    } else {
      await leagueAPI.addLeague(form.value)
      toast.success('League created')
    }
    router.push('/leagues')
  } catch (error) {
    console.error('Failed to save league:', error)
    const msg =
      error.response?.data?.detail || error.response?.data?.title || 'Failed to save league'
    toast.error(msg)
  } finally {
    loading.value = false
  }
}
</script>
