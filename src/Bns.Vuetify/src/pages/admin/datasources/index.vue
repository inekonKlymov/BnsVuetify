<template>
  <div>
    <!-- <div>datasources</div> -->
    <!-- <dt-datatables :columns="columns" :options="options">
      <template
        #column-name="{cellData,
                       rowData,
                       type}"
      >
        <a class="cursor-pointer text-secondary-hover" :href="rowData.Url">{{ cellData }}</a>
      </template>
    </dt-datatables> -->
    <!-- <Dt
      :columns="columns"
      :options="options"
      :theme="theme.current"
    >
      <template #checkbox-disabled="{cellData,rowData,type}">
        <DtCheckbox disabled :model-value="cellData" :theme-dark="theme.current.value.dark" />
      </template>
    </Dt> -->
    <DataTableRaw
      :columns="columns"
      :options="options"
      :url="`${baseUrl}/api/data-sources`"
    >
      <template #checkbox="{cellData,rowData,type}">
        <v-checkbox disabled :model-value="cellData" :theme-dark="theme.current.value.dark" />
      </template>
      <template #column-name="{cellData,rowData,type}">
        {{ cellData }}
      </template>
    </DataTableRaw>
    <!-- <DtDatatables
      :columns="columns"
      :options="options"
      :theme="theme.current"
    />  -->
    <!-- <v-data-table /> -->
    <!-- <v-table /> -->
    <!--
      <v-checkbox
    /> -->
    <!-- <ReactiveDataTable
      :columns="columns"
      :data="[]"
      :options="options"
    /> -->
    <!-- <DtDatatables
      :columns="columns"
      :options="options"
      :theme="theme.current"
    /> -->

  </div>
</template>
<route lang="yaml">
meta:
  layout: admin
</route>
<script lang="ts" setup>
  import type { Config, ConfigColumns, ObjectColumnData } from 'datatables.net-dt'
  import DatatableLib from 'datatables.net-dt'
  import DataTables from 'datatables.net-vue3/src/DataTable.vue'
  import { useTheme } from 'vuetify'
  import DatatablesUtils from '@/utils/datatablesUtils'
  DataTables.use(DatatableLib)
  const baseUrl = import.meta.env.VITE_API_URL
  const theme = useTheme()
  const options: Config = {
    ajax: DatatablesUtils.Ajax.Request(`${import.meta.env.VITE_API_URL}/api/data-sources`),
    serverSide: true,
    select: {
      selector: 'td:first-child',
      style: 'multi',

    },
    paging: true,
    info: true,
    orderMulti: true,
    order: [[1, 'desc']],
  }

  const columns: ConfigColumns[] = [
    {
      data: 'data_source.name',
      name: 'name',
      title: 'name',
    },
    {
      data: 'data_source.type',
      name: 'type',
      title: 'type',
    },
    {
      data: 'data_source.enabled',
      name: 'enabled',
      title: 'enabled',
      type: 'boolean',
      render: '#checkbox',
    },
    {
      data: 'data_source.modify_date',
      name: 'modify_date',
      title: 'modify_date',
      orderable: true,
      searchable: true,
      type: 'date',
      render: DatatablesUtils.Render.DateTime as ObjectColumnData,
    },
  ]

</script>
<style lang="scss">
@import 'datatables.net-dt';
@import 'vuetify/lib/components/VDataTable/VDataTable';
@import 'vuetify/lib/components/VTable/VTable';
</style>
