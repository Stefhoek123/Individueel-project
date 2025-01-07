<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { AuthClient } from "@/api/api";
import { el } from "vuetify/locale";

const authClient = new AuthClient();
const user = ref();
const loading = ref(true);

// Data ophalen bij component-mount
onMounted(async () => {
  try {
    sessionStorage.getItem("JWT");
    user.value = await authClient.getCurrentUser();

    if (!user) {
      console.error("No user data found.");
      console.log("User data loaded:", user);
    } else {
      console.error("User found:", user);
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
