/// <reference types="cypress" />

import ConfirmDialogue from '../ConfirmDialogue.vue'
import { mount } from 'cypress/vue'

describe('<ConfirmDialogue />', () => {
  it('renders', () => {
    // see: https://on.cypress.io/mounting-vue
    mount(ConfirmDialogue)
  })
})