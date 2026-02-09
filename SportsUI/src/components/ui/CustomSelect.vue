<template>
  <div class="relative" ref="wrapper">
    <button
      type="button"
      :id="id"
      :disabled="disabled"
      @click="toggle"
      @keydown.down.prevent="openAndFocusFirst"
      @keydown.up.prevent="openAndFocusLast"
      @keydown.enter.prevent="toggle"
      @keydown.space.prevent="toggle"
      @keydown.escape="close"
      :aria-expanded="isOpen"
      aria-haspopup="listbox"
      :class="[
        'w-full px-4 py-2 border rounded transition text-left flex items-center justify-between',
        disabled
          ? 'bg-gray-100 border-gray-200 text-gray-400 cursor-not-allowed'
          : isOpen
            ? 'border-blue-500 ring-1 ring-blue-500 cursor-pointer'
            : 'border-gray-300 hover:border-gray-400 cursor-pointer',
      ]"
    >
      <span :class="selectedLabel ? 'text-gray-800' : 'text-gray-400'">
        {{ selectedLabel || placeholder }}
      </span>
      <svg
        class="w-4 h-4 text-gray-400 transition-transform"
        :class="{ 'rotate-180': isOpen }"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
      </svg>
    </button>

    <Transition
      enter-active-class="transition duration-150 ease-out"
      enter-from-class="opacity-0 -translate-y-1"
      enter-to-class="opacity-100 translate-y-0"
      leave-active-class="transition duration-100 ease-in"
      leave-from-class="opacity-100 translate-y-0"
      leave-to-class="opacity-0 -translate-y-1"
    >
      <ul
        v-if="isOpen"
        role="listbox"
        ref="listbox"
        class="absolute z-50 mt-1 w-full bg-white border border-gray-200 rounded-lg shadow-lg max-h-60 overflow-auto"
        @keydown.escape="close"
      >
        <li
          v-for="(option, index) in options"
          :key="option.value"
          role="option"
          :aria-selected="option.value === modelValue"
          :tabindex="0"
          @click="select(option.value)"
          @keydown.enter.prevent="select(option.value)"
          @keydown.space.prevent="select(option.value)"
          @keydown.down.prevent="focusNext(index)"
          @keydown.up.prevent="focusPrev(index)"
          :class="[
            'px-4 py-2.5 cursor-pointer transition-colors duration-100 outline-none',
            option.value === modelValue ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-gray-700 hover:bg-gray-50',
            index !== options.length - 1 ? 'border-b border-gray-100' : '',
          ]"
        >
          {{ option.label }}
        </li>
        <li v-if="options.length === 0" class="px-4 py-2.5 text-gray-400 text-sm">No options available</li>
      </ul>
    </Transition>

    <!-- Hidden input for form validation -->
    <input v-if="required" :value="modelValue" required tabindex="-1" class="absolute opacity-0 w-0 h-0 pointer-events-none" />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount, nextTick } from 'vue'

const props = defineProps({
  modelValue: { type: [String, Number, null], default: '' },
  options: { type: Array, required: true },
  placeholder: { type: String, default: 'Select...' },
  disabled: { type: Boolean, default: false },
  required: { type: Boolean, default: false },
  id: { type: String, default: undefined },
})

const emit = defineEmits(['update:modelValue', 'change'])

const isOpen = ref(false)
const wrapper = ref(null)
const listbox = ref(null)

const selectedLabel = computed(() => {
  const opt = props.options.find((o) => o.value === props.modelValue || String(o.value) === String(props.modelValue))
  return opt?.label || ''
})

const toggle = () => {
  if (props.disabled) return
  isOpen.value = !isOpen.value
}

const close = () => {
  isOpen.value = false
}

const select = (value) => {
  emit('update:modelValue', value)
  emit('change', value)
  close()
}

const openAndFocusFirst = () => {
  if (!isOpen.value) isOpen.value = true
  nextTick(() => {
    const items = listbox.value?.querySelectorAll('[role="option"]')
    items?.[0]?.focus()
  })
}

const openAndFocusLast = () => {
  if (!isOpen.value) isOpen.value = true
  nextTick(() => {
    const items = listbox.value?.querySelectorAll('[role="option"]')
    items?.[items.length - 1]?.focus()
  })
}

const focusNext = (index) => {
  const items = listbox.value?.querySelectorAll('[role="option"]')
  const next = items?.[index + 1] || items?.[0]
  next?.focus()
}

const focusPrev = (index) => {
  const items = listbox.value?.querySelectorAll('[role="option"]')
  const prev = items?.[index - 1] || items?.[items.length - 1]
  prev?.focus()
}

const onClickOutside = (e) => {
  if (wrapper.value && !wrapper.value.contains(e.target)) {
    close()
  }
}

onMounted(() => document.addEventListener('click', onClickOutside))
onBeforeUnmount(() => document.removeEventListener('click', onClickOutside))
</script>
