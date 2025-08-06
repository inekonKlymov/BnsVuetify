import axios from 'axios'
import DateTime from 'datatables.net-datetime'
import DataTablesCore, { type ObjectColumnData } from 'datatables.net-dt'
import DataTable from 'datatables.net-vue3'

import moment from 'moment'
import { useAuthStore } from '@/stores'

DateTime.use(moment)
DataTablesCore.use(DateTime)
DataTable.use(DataTablesCore)

const Render = {
  DateTime: {
    _: (data: string) => {
      if (!data) {
        return ''
      }
      return moment(data).format('DD.MM.YYYY HH:mm:ss')
    },
    type: (data: string) => {
      if (!data) {
        return ''
      }
      return moment(data).unix()
    },
    sort: (data: string) => {
      if (!data) {
        return ''
      }
      return moment(data).unix()
    },
    // filter: (data: string) => {
    //   if (!data) return ''
    //   return moment(data).format('DD.MM.YYYY HH:mm:ss')
    // },
    // display: (data: string) => {
    //   if (!data) return ''
    //   return moment(data).format('DD.MM.YYYY HH:mm:ss')
    // },
  } as ObjectColumnData,
}
const Ajax = {
  Request (url: string) {
    return function (data: object, callback: ((data: any) => void), settings: any) {
      const auth = useAuthStore()
      // debugger
      axios.post(url, data, {
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded',
          // 'Authorization': `Bearer ${auth.token}`,
        },
        withCredentials: true,
      }).then(response => {
        callback(response.data)
      }).catch(() => {
        callback({ data: [] })
      })
    }
  },
}
// export function datatablesAjax (url: string) {
//   return function (data: object, callback: ((data: any) => void), settings: any) {
//     const auth = useAuthStore()
//     axios.post(url, data, {
//       headers: {
//         'Content-Type': 'application/x-www-form-urlencoded',
//         'Authorization': `Bearer ${auth.token}`,
//       },
//       withCredentials: true,
//     }).then(response => {
//       callback(response.data)
//     }).catch(() => {
//       callback({ data: [] })
//     })
//   }
// }
const DatatablesUtils = {
  Ajax,
  Render,
}
export default DatatablesUtils
