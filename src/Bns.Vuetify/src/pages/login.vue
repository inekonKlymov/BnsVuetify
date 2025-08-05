<template>
  <div class="login-container align-content-center">
    <v-card
      class="mx-auto my-auto pa-12 pb-8"
      elevation="8"
      max-width="448"
      rounded="lg"
    >
      <v-card-title class="text-center mb-5">Welcome </v-card-title>
      <v-form @submit.prevent="submit">
        <v-card-text>
          <v-text-field
            v-model="email.value.value"
            class="mb-2"
            density="compact"
            :error-messages="email.errorMessage.value"
            placeholder="Username"
            prepend-inner-icon="mdi-account-outline"
            required
            variant="outlined"
          />

          <v-text-field
            v-model="password.value.value"
            :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
            density="compact"
            :error-messages="password.errorMessage.value"
            placeholder="Password"
            prepend-inner-icon="mdi-lock-outline"
            required
            :type="visible ? 'text' : 'password'"
            variant="outlined"
            @click:append-inner="visible = !visible"
          />
        </v-card-text>

        <v-card-actions>
          <v-btn
            block
            class="mb-8"
            color="blue"
            size="large"
            type="submit"
            variant="tonal"
          >
            Log In
          </v-btn>
        </v-card-actions>
      </v-form>
      <div class="d-flex justify-center">
        <v-btn icon="mdi-theme-light-dark" @click="theme.toggle()" />
      </div>
    </v-card>
  </div>
</template>

<script lang="ts" setup>
  import type { AxiosError } from 'axios'
  import { useField, useForm } from 'vee-validate'
  import { ref } from 'vue'
  import { useRouter } from 'vue-router'
  import { useTheme } from 'vuetify'
  import { useAuthStore } from '@/stores/auth'
  const theme = useTheme()
  const auth = useAuthStore()
  const router = useRouter()

  const { handleSubmit, handleReset } = useForm({
    validationSchema: {
      email (value: string) {
        if (!value) return 'Email is required'
        if (value?.length <= 2) return 'Email must be at least 3 characters'
        if (value?.length >= 50) return 'Email must be less than 50 characters'
        return true
      },
      password (value: string) {
        if (!value) return 'Password is required'
        if (value?.length < 2) return 'Password must be at least 2 characters'
        if (value?.length > 50) return 'Password must be less than 50 characters'
        return true
      },
    },
  })
  const email = useField('email')
  const password = useField('password')
  const visible = ref(false)

  function onLoginSuccess () {
    router.push('/') // или нужный маршрут
  }
  function onLoginError (error: AxiosError) {
    password.setErrors(
      error.message || 'Login failed',
    )
  }

  // Для проверки авторизации
  const submit = handleSubmit((values: any) => {
    auth.login(values, onLoginSuccess, onLoginError)
  })
  if (auth.isAuthenticated) {
    onLoginSuccess()
  }
</script>

<style lang="scss">
.login-container {
  background: url("@/assets/logo.svg") no-repeat center center;
  background-size: cover;
  min-height: 100vh;
}
</style>
<route lang="yaml">
meta:
  layout: false
</route>
