<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { AuthClient } from "@/api/api";
import HeaderComponent from "@/components/HeaderComponent.vue";
import NavigationSide from "@/components/Navigation-side.vue";

const authClient = new AuthClient();
const user = ref();
const loading = ref(true);

onMounted(async () => {
  try {
    const token = sessionStorage.getItem("JWT");
    if (token) {
      const currentUser = await authClient.getCurrentUser(token);
      console.log("Current user:", currentUser);
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
  <div class="clas">
    <HeaderComponent />
    <div class="account-view">
        <NavigationSide />
      <h1>Mijn Account</h1>
      <div v-if="loading">Gegevens laden...</div>
      <div v-else-if="user" v-for="item in user" :key="item.id">
        <p><strong>User ID:</strong> {{ item.id }}</p>
        <p><strong>Firstname:</strong> {{ item.firstName }}</p>
        <p><strong>Lastname:</strong> {{ item.lastName }}</p>
        <p><strong>Email:</strong> {{ item.email }}</p>
        <p><strong>Family ID:</strong> {{ item.familyId }}</p>
      </div>
      <div v-else>
        <p>Geen gegevens gevonden.</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.account-view {
  max-width: 600px;
  margin: 0 auto;
}

.clas{
 margin-top: 65px;
} 
</style>
