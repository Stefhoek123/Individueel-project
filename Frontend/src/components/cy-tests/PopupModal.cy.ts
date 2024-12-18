import PopupModal from '../PopupModal.vue'
import { mount } from 'cypress/vue'

describe('<PopupModal />', () => {
  it('renders', () => {
    // see: https://on.cypress.io/mounting-vue
    mount(PopupModal)
  })
})