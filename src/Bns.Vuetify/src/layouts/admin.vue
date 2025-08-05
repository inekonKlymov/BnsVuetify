<template>
  <v-app-bar color="dark">
    <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer" />

    <v-toolbar-title>My files</v-toolbar-title>

    <template v-if="$vuetify.display.mdAndUp">
      <v-btn icon="mdi-magnify" variant="text" />

      <v-btn icon="mdi-filter" variant="text" />
    </template>

    <v-btn icon="mdi-dots-vertical" variant="text" />
    <v-btn icon="mdi-theme-light-dark" @click="theme.toggle()" />
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
</template>

<script lang="ts" setup>
  import { ref } from 'vue'
  import { useTheme } from 'vuetify'
  const theme = useTheme()
  const drawer = ref(true)
  const rail = ref(true)
  const items = ref([
    { title: 'Home', icon: 'mdi-home', to: '/' },
    { title: 'Index', icon: 'mdi-cog', to: '/admin' },
    { title: 'Datasources', icon: 'mdi-database', to: '/admin/datasources' },
  ])
// defineOptions({ name: 'AdminLayout' })
</script>
