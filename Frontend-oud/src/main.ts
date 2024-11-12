import '@/@iconify/icons-bundle'
import App from '@/App.vue'
import vuetify from '@/plugins/vuetify'
import router from '@/router'
import '@core/scss/template/index.scss'
import '@styles/styles.scss'
import axios from 'axios'
import { createApp } from 'vue'
import VueAxios from 'vue-axios'


// Create vue app
const app = createApp(App)

// Use plugins
app.use(vuetify)
app.use(router)
app.use(VueAxios, axios)

// Production: https://onboardifyapiservice.azurewebsites.net/
// Local: https://localhost:7226
app.axios.defaults.baseURL = 'https://localhost:5190'

// Mount vue app
app.mount('#app')
