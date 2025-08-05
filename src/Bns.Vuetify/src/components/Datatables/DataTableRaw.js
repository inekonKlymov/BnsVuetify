import { createElementVNode, defineComponent, getCurrentInstance, h, mergeProps, onBeforeUnmount, onMounted, ref, render, renderSlot, setBlockTracking, unref, watch } from 'vue'
const dtEvents = [
  'childRow',
  'column-sizing',
  'column-visibility',
  'destroy',
  'draw',
  'error',
  'init',
  'length',
  'order',
  'page',
  'preDraw',
  'preInit',
  'preXhr',
  'processing',
  'requestChild',
  'search',
  'stateLoadParams',
  'stateLoaded',
  'stateSaveParams',
  'xhr',
  'autoFill',
  'preAutoFill',
  'buttons-action',
  'buttons-processing',
  'column-reorder',
  'key',
  'key-blur',
  'key-focus',
  'key-refocus',
  'key-return-submit',
  'responsive-display',
  'responsive-resize',
  'rowgroup-datasrc',
  'pre-row-reorder',
  'row-reorder',
  'row-reordered',
  'dtsb-inserted',
  'deselect',
  'select',
  'select-blur',
  'selectItems',
  'selectStyle',
  'user-select',
  'stateRestore-change',
]
let DataTablesLib
const DataTable = defineComponent({
  name: 'Datatables.netVue',
  inheritAttrs: false,
  props: {
    ajax: null,
    columns: null,
    data: null,
    options: null,
  },
  emits: dtEvents,
  setup (props, { expose, slots, attrs, emit }) {
    const table = ref(null)
    const elements = {}
    const dt = ref()
    const oldData = ref([])
    watch(
      () => props.data,
      newVal => {
        const api = dt.value
        if (!api) {
          return
        }
        deleteElements(api)
        api.clear()
        api.rows.add(newVal).draw(false)
      },
      { deep: true },
    )
    onMounted(() => {
      const inst = getCurrentInstance()
      const options = Object.assign({}, props.options) || {}
      if (props.data) {
        options.data = props.data
        saveOld(options.data)
      }
      if (props.columns) {
        options.columns = props.columns
      }
      if (options.columns) {
        applyRenderers(options.columns, inst)
      }
      if (props.ajax) {
        options.ajax = props.ajax
      }
      if (!options.columnDefs) {
        options.columnDefs = []
      }
      if (inst) {
        const slotNames = Object.keys(inst.slots)
        for (const name of slotNames) {
          if (/^column\-/.test(name)) {
            const part = name.replace('column-', '')
            options.columnDefs.push({
              target: /^\d+$/.test(part) ? Number.parseInt(part) : part + ':name',
              render: '#' + name,
            })
          }
        }
        applyRenderers(options.columnDefs, inst)
      }
      if (!DataTablesLib) {
        throw new Error(
          'DataTables library not set. See https://datatables.net/tn/19 for details.',
        )
      }
      dt.value = new DataTablesLib(unref(table), options)
      dt.value?.on('preXhr', function () {
        deleteElements(dt.value)
      })
      for (const eName of dtEvents) {
        if (dt.value && inst) {
          dt.value.on(eName, function () {
            const args = Array.from(arguments)
            const event = args.shift()
            args.unshift({ event, dt })
            args.unshift(eName)
            inst.emit.apply(inst, args)
          })
        }
      }
    })
    onBeforeUnmount(() => {
      deleteElements(dt.value)
      dt.value?.destroy(true)
    })
    function saveOld (d) {
      oldData.value = d.value ? d.value.slice() : d.slice()
    }
    function createRenderer (slot) {
      return function (data, type, row, meta) {
        const key = meta.settings.sTableId + ',' + meta.row + ',' + meta.col
        if (!elements[key]) {
          const content = h('div', slot({
            cellData: data,
            colIndex: meta.col,
            rowData: row,
            rowIndex: meta.row,
            type,
          }))
          elements[key] = document.createElement('div')
          render(content, elements[key])
        }
        return elements[key]
      }
    }
    function applyRenderers (columns, inst) {
      if (!inst) {
        return
      }
      for (const col of columns) {
        if (typeof col.render === 'string' && col.render.charAt(0) === '#') {
          const name = col.render.replace('#', '')
          if (inst.slots[name]) {
            col.render = createRenderer(inst.slots[name])
          }
        } else if (
          typeof col.render === 'object'
          && typeof col.render.display === 'string'
          && col.render.display.charAt(0) === '#'
        ) {
          const name = col.render.display.replace('#', '')
          if (inst.slots[name]) {
            col.render.display = createRenderer(inst.slots[name])
          }
        }
      }
    }
    function deleteElements (dt) {
      const keys = Object.keys(elements)
      const id = dt.table().node().id
      for (const key of keys) {
        if (key.indexOf(id + ',') === 0) {
          delete elements[key]
        }
      }
    }
    expose({ dt })
    return (t, e) => e[0] || (setBlockTracking(-1), e[0] = createElementVNode('div', { class: 'datatable' }, [
      createElementVNode('table', mergeProps({
        ref: 'table',
      }, t.$attrs, { style: { width: '100%' } }), [
        renderSlot(t.$slots, 'default'),
      ], 16),
    ]), setBlockTracking(1), e[0])
  },
})
DataTable.use = function (lib) {
  DataTablesLib = lib
}
export { DataTable }
export default DataTable
