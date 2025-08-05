<template>

  <!-- <div class="v-theme-provider  rounded-b" :class="{ 'v-theme--dark': theme.current.value.dark, 'v-theme--light': !theme.current.value.dark }"> -->
  <div class="v-table  v-table--density-default" :class="{ 'v-theme--dark': theme.current.value.dark, 'v-theme--light': !theme.current.value.dark }">
    <div class="v-table__wrapper pa-2">
      <DataTable :columns :options="mergedOptions">
        <template v-for="(_, name) in $slots" #[name]="slotProps">
          <slot :name="name" v-bind="slotProps" />
        </template>
        <!-- <template #checkbox-disabled="{cellData,rowData,type}">
          <DtCheckbox disabled :model-value="cellData" :theme-dark="theme.current.value.dark" />
        </template> -->
        <template #checkbox="{cellData,rowData,type}">

          <v-checkbox
            :model-value="cellData"
            :theme-dark="theme.current.value.dark"
          />
          <!-- <DtCheckbox :model-value="cellData" :theme-dark="theme.current.value.dark" /> -->
        </template>
      </DataTable>
    </div>
  </div>
  <!-- </div> -->
</template>
<script lang="ts" setup>

  import type { Config, ConfigColumns } from 'datatables.net-dt'
  import DateTime from 'datatables.net-datetime'
  import DataTablesCore from 'datatables.net-dt'
  import DataTable from 'datatables.net-vue3/src/DataTable.vue'
  import jszip from 'jszip'
  import pdfmake from 'pdfmake'
  import { useTheme } from 'vuetify'
  // import Editor from '@datatables.net/editor'
  import 'datatables.net-autofill-dt'
  import 'datatables.net-buttons-dt'
  import 'datatables.net-buttons/js/buttons.colVis.mjs'
  import 'datatables.net-buttons/js/buttons.html5.mjs'
  import 'datatables.net-buttons/js/buttons.print.mjs'
  import 'datatables.net-colreorder-dt'
  import 'datatables.net-columncontrol-dt'
  import 'datatables.net-fixedcolumns-dt'
  import 'datatables.net-fixedheader-dt'
  import 'datatables.net-keytable-dt'
  import 'datatables.net-responsive-dt'
  import 'datatables.net-rowgroup-dt'
  import 'datatables.net-rowreorder-dt'
  import 'datatables.net-scroller-dt'
  import 'datatables.net-searchbuilder-dt'
  import 'datatables.net-searchpanes-dt'
  import 'datatables.net-select-dt'
  import 'datatables.net-staterestore-dt'
  import 'material-components-web'

  DataTablesCore.use(DateTime)
  DataTablesCore.Buttons.jszip(jszip)
  DataTablesCore.Buttons.pdfMake(pdfmake)
  DataTable.use(DataTablesCore)

  const props = defineProps<{
    columns: ConfigColumns[]
    options: Config
  }>()
  const defaultOptions: Config = {
    // ваши дефолтные опции, например:
    // paging: true,
    // searching: true,
    // ...
    processing: true,
    scrollX: true,
  }

  const mergedOptions = computed(() => ({
    ...defaultOptions,
    ...props.options,
  }))
  const theme = useTheme()
  // const editor: Editor = new Editor({
  //   ajax: {
  //     url: `${import.meta.env.VITE_API_URL}/api/data-sources`,
  //     type: 'POST',
  //   },
  //   table: '#dataTable',
  //   fields: [
  //     {
  //       label: 'Name',
  //       name: 'data_source.name',
  //     },
  //     {
  //       label: 'Type',
  //       name: 'data_source.type',
  //     },
  //     {
  //       label: 'Enabled',
  //       name: 'data_source.enabled',
  //       type: 'checkbox',
  //     },
  //     {
  //       label: 'Modify Date',
  //       name: 'data_source.modify_date',
  //       type: 'datetime',
  //     },
  //   ],
  // })
</script>
<style lang="scss" scoped>
@import 'vuetify/lib/components/VDataTable/VDataTable';
@import 'vuetify/lib/components/VTable/VTable';

</style>
<style lang="scss">
// @import 'material-components-web';
@import 'datatables.net-dt';
@import 'datatables.net-autofill-dt';
@import 'datatables.net-buttons-dt';
@import 'datatables.net-colreorder-dt';
@import 'datatables.net-columncontrol-dt';
@import 'datatables.net-fixedcolumns-dt';
@import 'datatables.net-fixedheader-dt';
@import 'datatables.net-keytable-dt';
@import 'datatables.net-responsive-dt';
@import 'datatables.net-rowgroup-dt';
@import 'datatables.net-rowreorder-dt';
@import 'datatables.net-scroller-dt';
@import 'datatables.net-searchbuilder-dt';
@import 'datatables.net-searchpanes-dt';
@import 'datatables.net-select-dt';
@import 'datatables.net-staterestore-dt';
.dt-container .dt-layout-row:last-child,
.dt-container .dt-layout-row:first-child,
.dt-container .dt-layout-row:nth-last-child(2):has(+ .dt-autosize) {
  padding: 8px 16px !important;
}

.dt-scroll-headInner table.dataTable > thead > tr > th,
.dt-scroll-headInner table.dataTable > thead > tr > td,
// table.dataTable > tbody > tr > th,
.dt-scroll-body table.dataTable > tbody > tr > td {
  padding: 8px 16px !important;
}
</style>
