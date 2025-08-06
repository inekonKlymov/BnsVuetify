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

// const app = createApp(App)

const app = createApp(App)
registerPlugins(app)
const authStore = useAuthStore()
authStore.init().then(_ => {
  if (authStore.isAuthenticated) {
    // app.config.globalProperties.$keycloak = keycloak
    app.mount('#app')
  } else {
    authStore.login()
  }
})
// attempt to auto refresh token before startup
// try {
//   await authStore.refreshTokenAsync()
// } catch {
//   // catch error to start app on success or failure
// }
// app.mount('#app')
