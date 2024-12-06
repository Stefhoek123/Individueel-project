// Plugins
import { registerPlugins } from '@/plugins'
import vuetify from '@/plugins/vuetify'
import router from '@/router'
import axios from 'axios'
import { createPinia } from 'pinia'
import VueAxios from 'vue-axios'

// Components
import App from './App.vue'

// Composables
import { createApp } from 'vue'
import axiosIns from './plugins/axios'

const app = createApp(App)

// Set axios base URL
 axios.defaults.baseURL = 'https://localhost:5190'

// app.use(axios, {
//     baseUrl: 'https://localhost:5190',
// })

// Use plugins
app.use(vuetify)
app.use(createPinia())
app.use(router)
app.use(VueAxios, axios)
registerPlugins(app)

app.mount('#app')
