// export const useAuthStore = defineStore('auth', {
//  state: () => ({
//    tokenType: null as string | null,
//    token: localStorage.getItem('jwt') || null,
//    refreshToken: localStorage.getItem('jwtRefresh') || null,
//    refreshTokenTimeout: undefined as number | undefined,
//  }),
//  actions: {
//    saveTokenData (data: { tokenType: string, accessToken: string, refreshToken: string, expiresIn: number }) {
//      this.tokenType = data.tokenType
//      this.token = data.accessToken
//      localStorage.setItem('jwt', data.accessToken)
//      this.refreshToken = data.refreshToken
//      localStorage.setItem('jwtRefresh', data.refreshToken)
//    },
//    removeTokenData () {
//      this.tokenType = null
//      this.token = null
//      localStorage.removeItem('jwt')
//      localStorage.removeItem('jwtRefresh')
//      this.refreshToken = null
//    },
//    async login (
//      credentials: { email: string, password: string },
//      onSuccess: () => void,
//      onError: (error: any) => void) {
//      axios
//        .post(`${import.meta.env.VITE_API_URL}/api/auth/login`, credentials)
//        .then(response => {
//          this.saveTokenData(response.data)
//          this.startRefreshTokenTimer(response.data.expiresIn)
//          onSuccess()
//        })
//        .catch(error => {
//          debugger
//          onError(error)
//        })
//    },
//    // setToken (token: string) {
//    //   this.token = token
//    //   localStorage.setItem('jwt', token)
//    // },
//    // clearToken () {
//    //   this.token = null
//    //   localStorage.removeItem('jwt')
//    // },
//    // setUser (user: string) {
//    //   this.user = user
//    // },
//    async refreshTokenAsync () {
//      if (!this.refreshToken) {
//        return
//      }
//      axios.post(`${import.meta.env.VITE_API_URL}/api/auth/refresh`, { refreshToken: this.refreshToken }, { withCredentials: true })
//        .then(response => {
//          this.saveTokenData(response.data)
//          this.startRefreshTokenTimer(response.data.expiresIn)
//        })
//        .catch(error => {
//          // console.error('Failed to refresh token:', error)
//          this.logout()
//        })
//    },
//    logout () {
//      this.stopRefreshTokenTimer()
//      this.removeTokenData()
//      router.push('/login')
//      // axios
//      //   .post(`${import.meta.env.VITE_API_URL}/revoke-token`, {}, { withCredentials: true })
//      //   .then(() => {
//      //   })
//    },
//    startRefreshTokenTimer (expiresIn = 3600) {
//      this.refreshTokenTimeout = setTimeout(this.refreshTokenAsync, expiresIn * 1000)
//    },
//    stopRefreshTokenTimer () {
//      if (this.refreshTokenTimeout !== undefined) {
//        clearTimeout(this.refreshTokenTimeout)
//        this.refreshTokenTimeout = undefined
//      }
//    },
//  },
//  getters: {
//    isAuthenticated: state => !!state.token,
//  },
// })
import Keycloak from 'keycloak-js'
import { defineStore } from 'pinia'

const keycloak = new Keycloak({
  url: 'http://localhost:8082/',
  realm: 'Bns',
  clientId: 'bnsVue',
})

// import keycloak from './keyCloak'

export const useAuthStore = defineStore('keycloak', {
  state: () => ({
    keycloak,
    authenticated: false,
    token: null as string | null,
    username: '',
  }),
  actions: {
    async init () {
      await this.keycloak.init({ onLoad: 'login-required' })
      this.authenticated = this.keycloak.authenticated!
      this.token = this.keycloak.token!
      this.username = this.keycloak.tokenParsed?.name || ''
      debugger
      if (this.token) {
        // Установить токен в axios по умолчанию
        import('axios').then(({ default: axios }) => {
          axios.defaults.headers.common.Authorization = `Bearer ${this.token}`
        })
      }
    },
    login () {
      this.keycloak.login()
    },
    logout () {
      // Удалить токен из axios по умолчанию
      import('axios').then(({ default: axios }) => {
        delete axios.defaults.headers.common.Authorization
      })
      this.keycloak.logout()
    },
    updateToken (minValidity = 5) {
      return this.keycloak.updateToken(minValidity).then(refreshed => {
        if (refreshed) {
          this.token = this.keycloak.token!
          // Обновить токен в axios
          import('axios').then(({ default: axios }) => {
            axios.defaults.headers.common.Authorization = `Bearer ${this.token}`
          })
        }
      })
    },
  },
  getters: {
    isAuthenticated: state => state.authenticated,
    getToken: state => state.token,
  },
})
