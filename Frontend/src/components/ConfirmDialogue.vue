<script setup lang="ts">
import { ref } from 'vue'
import PopupModal from './PopupModal.vue'

const popupRef = ref<InstanceType<typeof PopupModal> | null>(null)

const title = ref<string | undefined>(undefined)
const message = ref<string | undefined>(undefined)
const okButton = ref<string | undefined>(undefined)
const cancelButton = ref<string>('Go Back')

let resolvePromise: ((value: boolean) => void) | undefined
let rejectPromise: ((reason?: unknown) => void) | undefined

function show(opts: { title: string; message: string; okButton: string; cancelButton: string | undefined }) {
  title.value = opts.title
  message.value = opts.message
  okButton.value = opts.okButton
  if (opts.cancelButton)
    cancelButton.value = opts.cancelButton

  popupRef.value?.open()

  return new Promise((resolve, reject) => {
    resolvePromise = resolve
    rejectPromise = reject
  })
}

function confirm() {
  popupRef.value?.close()
  resolvePromise?.(true)
}

function cancel() {
  popupRef.value?.close()
  rejectPromise?.('user cancelled')
}

defineExpose({ show })
</script>

<template>
  <PopupModal ref="popupRef">
    <h2 style="margin-block-start: 0;">
      {{ title }}
    </h2>
    <p>{{ message }}</p>
    <div class="btns">
      <VBtn @click="cancel">
        {{ cancelButton }}
      </VBtn>
      <VBtn @click="confirm">
        {{ okButton }}
      </VBtn>
    </div>
  </PopupModal>
</template>

<style scoped>
.btns {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}
</style>
