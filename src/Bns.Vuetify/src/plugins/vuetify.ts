/**
 * plugins/vuetify.ts
 *
 * Framework documentation: https://vuetifyjs.com`
 */

// Composables
import { createVuetify } from 'vuetify'
import colors from 'vuetify/util/colors'

// Styles
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

// https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides
export default createVuetify({
  theme: {
    defaultTheme: 'system',
    themes: {
      light: {
        dark: false,
        colors: {
          primary: '#21304D', // colors.indigo.darken4
          secondary: colors.lightBlue.darken1, // '#10a4e4',
          error: colors.red.darken2, // '#D32F2F' Material Red 700
          info: colors.blue.darken2, // '#1976D2' Material Blue 700
          success: colors.green.darken2, // '#388E3C' Material Green 700
          warning: colors.amber.darken2, // '#FFA000' Material Amber 700
          background: colors.grey.lighten4, // '#F5F5F5' Material Grey 100
          surface: colors.shades.white, // '#FFFFFF' Material White
        },
      },
      dark: {
        dark: true,
        colors: {
          primary: '#21304D', // colors.indigo.darken4 // Indigo 900 (ближайший к вашему #21304D)
          secondary: colors.lightBlue.darken1, // '#039BE5', // Light Blue 600 (ближайший к вашему #10A4E4)
          error: '#CF6679', // Material Red A200 (рекомендуется для dark)
          info: colors.blue.lighten3, // '#90CAF9', // Material Blue 200
          success: colors.green.lighten1, // '#66BB6A', // Material Green 400
          warning: colors.amber.darken1, // '#FFB300', // Material Amber 600
          background: '#121212', // Material Dark background
          surface: '#1E1E1E', // Material Dark surface
        },
      },
    },
  },
})
