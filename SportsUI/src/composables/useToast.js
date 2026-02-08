import { useToast as useToastification } from 'vue-toastification'

export function useToast() {
  const toast = useToastification()

  return {
    success: (message) => toast.success(message),
    error: (message) => toast.error(message),
  }
}
