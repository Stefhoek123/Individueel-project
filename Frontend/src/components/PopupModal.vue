<script setup lang="ts">
import { ref, defineExpose } from 'vue'
const isVisible = ref<boolean>(false)

function open() {
  isVisible.value = true
}

function close() {
  isVisible.value = false
}

defineExpose({ open, close })
</script>

<template>
  <Transition name="fade">
    <div
      v-if="isVisible"
      class="popup-modal"
    >
      <div class="window">
        <slot />
      </div>
    </div>
  </Transition>
</template>

<style scoped>
/* css class for the transition */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s;
}

.fade-enter,
.fade-leave-to {
  opacity: 0;
}

.popup-modal {
  position: fixed;
  z-index: 1;
  display: flex;
  align-items: center;
  padding: 0.5rem;
  background-color: rgba(0, 0, 0, 50%);
  inset: 0;
}

.window {
  padding: 1rem;
  border-radius: 5px;
  background: rgb(var(--v-theme-background));
  box-shadow: 2px 4px 8px rgba(0, 0, 0, 20%);
  margin-inline: auto;
  max-inline-size: 480px;
}
</style>
