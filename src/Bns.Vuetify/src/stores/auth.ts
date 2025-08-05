import axios from 'axios'
import { defineStore } from 'pinia'
import router from '@/router'
export const useAuthStore = defineStore('auth', {
  state: () => ({
    tokenType: null as string | null,
    token: localStorage.getItem('jwt') || null,
    refreshToken: localStorage.getItem('jwtRefresh') || null,
    refreshTokenTimeout: undefined as number | undefined,
  }),
  actions: {
    saveTokenData (data: { tokenType: string, accessToken: string, refreshToken: string, expiresIn: number }) {
      this.tokenType = data.tokenType
      this.token = data.accessToken
      localStorage.setItem('jwt', data.accessToken)
      this.refreshToken = data.refreshToken
      localStorage.setItem('jwtRefresh', data.refreshToken)
    },
    removeTokenData () {
      this.tokenType = null
      this.token = null
      localStorage.removeItem('jwt')
      localStorage.removeItem('jwtRefresh')
      this.refreshToken = null
    },
    async login (
      credentials: { email: string, password: string },
      onSuccess: () => void,
      onError: (error: any) => void) {
      axios
        .post(`${import.meta.env.VITE_API_URL}/api/auth/login`, credentials)
        .then(response => {
          this.saveTokenData(response.data)
          this.startRefreshTokenTimer(response.data.expiresIn)
          onSuccess()
        })
        .catch(error => {
          debugger
          onError(error)
        })
    },
    // setToken (token: string) {
    //   this.token = token
    //   localStorage.setItem('jwt', token)
    // },
    // clearToken () {
    //   this.token = null
    //   localStorage.removeItem('jwt')
    // },
    // setUser (user: string) {
    //   this.user = user
    // },
    async refreshTokenAsync () {
      if (!this.refreshToken) {
        return
      }
      axios.post(`${import.meta.env.VITE_API_URL}/api/auth/refresh`, { refreshToken: this.refreshToken }, { withCredentials: true })
        .then(response => {
          this.saveTokenData(response.data)
          this.startRefreshTokenTimer(response.data.expiresIn)
        })
        .catch(error => {
          // console.error('Failed to refresh token:', error)
          this.logout()
        })
    },
    logout () {
      this.stopRefreshTokenTimer()
      this.removeTokenData()
      router.push('/login')
      // axios
      //   .post(`${import.meta.env.VITE_API_URL}/revoke-token`, {}, { withCredentials: true })
      //   .then(() => {
      //   })
    },
    startRefreshTokenTimer (expiresIn = 3600) {
      this.refreshTokenTimeout = setTimeout(this.refreshTokenAsync, expiresIn * 1000)
    },
    stopRefreshTokenTimer () {
      if (this.refreshTokenTimeout !== undefined) {
        clearTimeout(this.refreshTokenTimeout)
        this.refreshTokenTimeout = undefined
      }
    },
  },
  getters: {
    isAuthenticated: state => !!state.token,
  },
})
