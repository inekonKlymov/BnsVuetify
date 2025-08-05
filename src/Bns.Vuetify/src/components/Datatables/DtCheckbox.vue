<template>

  <div
    class="v-input__control"
    :class="{ 'v-input--disabled': disabled }"
  >
    <div class="v-selection-control v-selection-control--density-default v-checkbox-btn" :class="{'v-selection-control--disabled':disabled, 'v-selection-control--dirty': modelValue}">
      <div class="v-selection-control__wrapper">
        <div class="v-selection-control__input">
          <i
            aria-hidden="true"
            class="mdi v-icon notranslate v-icon--size-default"
            :class="[
              modelValue ? 'mdi-checkbox-marked' : 'mdi-checkbox-blank-outline',
              themeDark ? 'v-theme--dark' : 'v-theme--light'
            ]"
          />
          <input
            :id="id"
            :aria-describedby="id + '-messages'"
            :aria-disabled="disabled ? 'true' : 'false'"
            :checked="modelValue"
            class="v-checkbox-input"
            :disabled="disabled"
            type="checkbox"
            :value="modelValue ? 'true' : 'false'"
            @change="onChange"
          >
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { computed, defineEmits, defineProps } from 'vue'

  // const themeClass = computed(() => theme.current.value.dark ? 'v-theme--dark' : 'v-theme--light')
  const props = defineProps<{
    themeDark: boolean
    modelValue: boolean
    disabled?: boolean
    id?: string
  }>()
  const emit = defineEmits(['update:modelValue'])

  // let themeClass = computed(() => '')
  // if (useTheme) {
  //   try {
  //   } catch {
  //     themeClass = computed(() => '')
  //   }
  // }
  const id = computed(() => props.id || `dt-checkbox-${Math.random().toString(36).slice(2, 10)}`)

  function onChange (event: Event) {
    debugger
    const target = event.target as HTMLInputElement | null
    if (target) {
      emit('update:modelValue', target.checked)
    }
  }
</script>

<style scoped>
@import 'vuetify/lib/components/VCheckbox/VCheckbox';
@import 'vuetify/lib/components/VSelectionControl/VSelectionControl';
</style>
