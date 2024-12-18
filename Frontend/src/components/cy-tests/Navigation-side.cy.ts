import NavigationSide from '../Navigation-side.vue'
import { mount } from 'cypress/vue'

describe('<NavigationSide />', () => {
  it('renders', () => {
    // see: https://on.cypress.io/mounting-vue
    mount(NavigationSide)
  })
})