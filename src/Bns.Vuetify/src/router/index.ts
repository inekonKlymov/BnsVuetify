/**
 * router/index.ts
 *
 * Automatic routes for `./src/pages/*.vue`
 */

import { setupLayouts } from 'virtual:generated-layouts'
// Composables
// eslint-disable-next-line import/no-duplicates
import { createRouter, createWebHistory } from 'vue-router/auto'
// eslint-disable-next-line import/no-duplicates
import { routes } from 'vue-router/auto-routes'

import { useAuthStore } from '@/stores/auth'

for (const route of routes) {
  if (route.name !== '/login') {
    route.meta = { ...route.meta, requiresAuth: true }
  }
}

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: setupLayouts(routes),
})

// Workaround for https://github.com/vitejs/vite/issues/11804
router.onError((err, to) => {
  if (err?.message?.includes?.('Failed to fetch dynamically imported module')) {
    if (localStorage.getItem('vuetify:dynamic-reload')) {
      console.error('Dynamic import error, reloading page did not fix it', err)
    } else {
      console.log('Reloading page to fix dynamic import error')
      localStorage.setItem('vuetify:dynamic-reload', 'true')
      location.assign(to.fullPath)
    }
  } else {
    console.error(err)
  }
})
router.beforeEach((to, from, next) => {
  const auth = useAuthStore()
  if ((to.meta.requiresAuth && (!auth.isAuthenticated))) {
    next('/login')
  } else {
    next()
  }
})
router.isReady().then(() => {
  localStorage.removeItem('vuetify:dynamic-reload')
})

export default router
