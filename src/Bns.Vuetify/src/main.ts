/**
 * main.ts
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

// Composables
import { createApp } from 'vue'

// Plugins
import { registerPlugins } from '@/plugins'

// Components
import App from './App.vue'
import { useAuthStore } from './stores'

// Styles
import 'unfonts.css'

const app = createApp(App)

registerPlugins(app)
// attempt to auto refresh token before startup
try {
  const authStore = useAuthStore()
  await authStore.refreshTokenAsync()
} catch {
  // catch error to start app on success or failure
}
app.mount('#app')
