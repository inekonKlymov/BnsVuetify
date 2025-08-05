<template>
  <div class="v-table  v-table--density-default" :class="{ 'v-theme--dark': theme.current.value.dark, 'v-theme--light': !theme.current.value.dark }">
    <div class="v-table__wrapper pa-2">
      <div class="datatable">
        <v-table ref="table">
          <thead>
            <tr>
              <th v-for="col in columns" :key="col.name || col.data" @click="onSort(col)">
                {{ col.title }}
                <v-icon v-if="sort.col === (col.name || col.data)" size="x-small">
                  {{ sort.dir === 'asc' ? 'mdi-arrow-up' : 'mdi-arrow-down' }}
                </v-icon>
              </th>
            </tr>
            <!-- <tr>
          <th v-for="col in columns" :key="'search-' + (col.name || col.data)">
            <v-text-field
              v-model="search[col.name || col.data]"
              density="compact"
              hide-details
              placeholder="Поиск..."
              style="max-width:120px"
              variant="solo"
            />
          </th>
        </tr> -->
          </thead>
          <tbody>
            <tr v-for="row in visibleRows" :key="row.id || row._dtId || rowIndex(row)">
              <td v-for="(col, idx) in columns" :key="idx">
                <template v-if="$slots['column-' + (col.name || col.data)]">
                  <slot :cell-data="getNestedValue(row,col.data) " :name="'column-' + (col.name || col.data)" :row-data="row" />
                </template>
                <template v-else-if="typeof col.render === 'string' && col.render.charAt(0) === '#' && $slots[col.render.replace('#', '')]">
                  <slot :cell-data="getNestedValue(row,col.data)" :name="col.render.replace('#', '')" :row-data="row" />
                </template>
                <template v-else-if="typeof col.render === 'object'">
                  {{ console.log('ObjectColumnData', col.data) }}
                </template>
                <template v-else>
                  {{ console.log('default', typeof col.render) }}
                  {{ getNestedValue(row,col.data) }}
                </template>
              </td>
            </tr>
          </tbody>
        </v-table>
        <v-pagination
          v-model="pagination.page"
          class="mt-2"
          :length="pagination.totalPages"
          @input="onPageChange"
        />
        <v-progress-linear v-if="loading" color="primary" indeterminate />
        <v-alert v-if="error" type="error">{{ error }}</v-alert>
      </div>
    </div>
  </div>
</template>
<script lang="ts" setup>

  import type { Config, ConfigColumns } from 'datatables.net-dt'
  import axios from 'axios'
  import DataTablesCore from 'datatables.net-dt'
  import { computed, onMounted, ref } from 'vue'
  import { useTheme } from 'vuetify'

  import { useAuthStore } from '@/stores'
  // let DataTablesLib: any

  const props = defineProps<{
    columns: ConfigColumns[]
    url: string
    options: Config
  }>()

  defineEmits([
    'childRow', 'column-sizing', 'column-visibility', 'destroy', 'draw', 'error', 'init', 'length', 'order', 'page', 'preDraw', 'preInit', 'preXhr', 'processing', 'requestChild', 'search', 'stateLoadParams', 'stateLoaded', 'stateSaveParams', 'xhr', 'autoFill', 'preAutoFill', 'buttons-action', 'buttons-processing', 'column-reorder', 'key', 'key-blur', 'key-focus', 'key-refocus', 'key-return-submit', 'responsive-display', 'responsive-resize', 'rowgroup-datasrc', 'pre-row-reorder', 'row-reorder', 'row-reordered', 'dtsb-inserted', 'deselect', 'select', 'select-blur', 'selectItems', 'selectStyle', 'user-select', 'stateRestore-change',
  ])
  const table = ref(null)
  const dt = ref(null)
  debugger
  const theme = useTheme()
  const tableData = ref<any[]>([])
  const columns = computed(() => props.columns)
  const loading = ref(false)
  const error = ref<string | null>(null)
  // Pagination, sort, search state
  const pagination = ref({ page: 1, perPage: props.options?.pageLength || 10, total: 0, totalPages: 1 })
  const sort = ref<{ col: string, dir: 'asc' | 'desc' }>({ col: '', dir: 'asc' })
  // const search = ref<Record<string, string>>({})

  function fetchData () {
    const auth = useAuthStore()
    loading.value = true
    error.value = null
    axios.post(props.url, {}, {
      headers: { 'Content-Type': 'application/x-www-form-urlencoded',
                 'Authorization': `Bearer ${auth.token}` },
      withCredentials: true,
    }).then(resp => {
      if (Array.isArray(resp.data)) {
        tableData.value = resp.data
      } else if (resp.data && Array.isArray(resp.data.data)) {
        tableData.value = resp.data.data
      } else {
        tableData.value = []
      }
    }).catch(error_ => {
      error.value = error_.message || 'Ошибка загрузки данных'
      tableData.value = []
    }).finally(() => {
      loading.value = false
    })
  }
  dt.value = new DataTablesCore(table.value, {
    ...props.options,
    data: tableData.value,
    columns: props.columns,
  })
  onMounted(() => {
    fetchData()
  })

  // ...existing imports...
  // ...existing code...

  const visibleRows = computed(() => {
    const rows = [...tableData.value]
    // Search
    // for (const key in search.value) {
    //   if (search.value[key]) {
    //     rows = rows.filter(row => String(row[key]).toLowerCase().includes(search.value[key].toLowerCase()))
    //   }
    // }
    // Sort
    // if (sort.value.col) {
    //   rows.sort((a, b) => {
    //     const av = a[sort.value.col]
    //     const bv = b[sort.value.col]
    //     if (av === bv) return 0
    //     if (sort.value.dir === 'asc') return av > bv ? 1 : -1
    //     return av < bv ? 1 : -1
    //   })
    // }
    // Pagination
    pagination.value.total = rows.length
    pagination.value.totalPages = Math.max(1, Math.ceil(rows.length / pagination.value.perPage))
    const start = (pagination.value.page - 1) * pagination.value.perPage
    return rows.slice(start, start + pagination.value.perPage)
  })

  function rowIndex (row: any) {
    return tableData.value.indexOf(row)
  }

  function onPageChange (page: number) {
    pagination.value.page = page
  }

  function onSort (col: any) {
    const colKey = col.name || col.data
    if (sort.value.col === colKey) {
      sort.value.dir = sort.value.dir === 'asc' ? 'desc' : 'asc'
    } else {
      sort.value.col = colKey
      sort.value.dir = 'asc'
    }
  }

  // ...existing code...
  function getNestedValue (obj, path) {
    return path.split('.').reduce((acc, key) => acc && acc[key], obj)
  }
</script>

<style lang="css">
@import 'datatables.net-dt';
</style>
