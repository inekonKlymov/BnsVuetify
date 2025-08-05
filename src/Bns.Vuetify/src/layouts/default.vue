<template>
  <v-app-bar color="primary">
    <v-app-bar-nav-icon variant="text" @click.stop="rail = !rail" />

    <v-toolbar-title>My files</v-toolbar-title>

    <template v-if="$vuetify.display.mdAndUp">
      <v-btn icon="mdi-magnify" variant="text" />
      <v-btn icon="mdi-filter" variant="text" />
    </template>
    <div>{{ username }}</div>
    <v-btn icon="mdi-dots-vertical" variant="text" />
    <v-btn icon="mdi-theme-light-dark" @click="theme.toggle()" />
    <v-btn icon="mdi-logout" variant="text" @click="auth.logout()" />
  </v-app-bar>

  <v-navigation-drawer
    v-model="drawer"
    expand-on-hover
    permanent
    :rail="rail"
    @click.stop="rail = false"
  >
    <!-- :location="$vuetify.display.mobile ? 'bottom' : undefined" -->
    <v-list density="compact">
      <v-list-item
        v-for="item in items"
        :key="item.title"
        :prepend-icon="item.icon"
        :title="item.title"
        :to="item.to"
      />
    </v-list>
  </v-navigation-drawer>
  <v-main>
    <div class="pa-10">
      <router-view />
    </div>
  </v-main>

  <AppFooter />
</template>

<script lang="ts" setup>
  import { ref } from 'vue'
  import { useTheme } from 'vuetify'
  import { useAuthStore } from '@/stores'

  const auth = useAuthStore()
  console.log('Auth store:', auth)
  const username = auth.user
  const drawer = ref(true)
  const rail = ref(true)
  const items = ref([
    { title: 'Home', icon: 'mdi-home', to: '/' },
    { title: 'Datasources', icon: 'mdi-database', to: '/admin/datasources' },
    { title: 'Admin', icon: 'mdi-security', to: '/admin' },
  ])

  const theme = useTheme()
</script>
