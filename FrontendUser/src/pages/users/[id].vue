<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { AuthClient } from "@/api/api";

const authClient = new AuthClient();
const user = ref();
const loading = ref(true);

onMounted(async () => {
  try {
    const token = sessionStorage.getItem("JWT");
    if (token) {
      const currentUser = await authClient.getCurrentUser(token);
      user.value = currentUser;

      console.log("User data:", user.value);
    } else {
      throw new Error("JWT token is missing");
    }
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
