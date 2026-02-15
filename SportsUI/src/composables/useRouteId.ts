import { computed } from 'vue'
import { useRoute } from 'vue-router'

/**
 * Returns a computed `string` id from route params.
 * Avoids the repeated `route.params.id as string` casts, since
 * vue-router types `params[key]` as `string | string[]`.
 */
export function useRouteId(param = 'id') {
  const route = useRoute()
  return computed(() => {
    const value = route.params[param]
    return Array.isArray(value) ? value[0] : value
  })
}
