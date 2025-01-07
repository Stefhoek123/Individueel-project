<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { AuthClient } from "@/api/api";

const authClient = new AuthClient();
const user = ref(null);
const loading = ref(true);

// Data ophalen bij component-mount
onMounted(async () => {
  try {
    const response = await authClient.getCurrentUser();
    user.value = response.data;
  } catch (error) {
    console.error("Error loading user data:", error);
  } finally {
    loading.value = false;
  }
});
</script>

<template>
    <div class="account-view">
      <h1>Mijn Account</h1>
      <div v-if="loading">Gegevens laden...</div>
      <div v-else-if="user">
        <p><strong>Naam:</strong> {{ user.firstName }}</p>
        <p><strong>Email:</strong> {{ user.email }}</p>
      </div>
      <div v-else>
        <p>Geen gegevens gevonden.</p>
      </div>
    </div>
  </template>
  
  <style scoped>
  .account-view {
    max-width: 600px;
    margin: 0 auto;
  }
  </style>
  