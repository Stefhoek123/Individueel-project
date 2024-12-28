<script setup lang="ts">
import { ref, onMounted, watch } from "vue";
import { AuthClient } from "@/api/api";
import router from "@/router";

// Simulating authentication check (replace with actual logic)
const isLoggedIn = ref(false);
const userEmail = ref("");
const authClient = new AuthClient();

function isAuthenticated() {
  return document.cookie.includes('.AspNetCore.Cookies'); // Adjust cookie name if needed
}

console.log("Is authenticated:", isAuthenticated());
console.log("Is logged in:", isLoggedIn.value);

onMounted(() => {
  isLoggedIn.value = isAuthenticated();
});

async function logout() {
  try {
    await authClient.logout();
    isLoggedIn.value = false;
    userEmail.value = "";
    router.push("/");
    console.log("Logged out successfully");
  } catch (error: any) {
    console.error("Logout error:", error);
    alert(error.message || "Logout failed. Please try again.");
  }
}
</script>

<template>
  <v-navigation-drawer>
    <template v-slot:prepend>
      <v-list-item
      v-if="isLoggedIn"
        lines="two"
        prepend-avatar="../assets/resto-logo.png"
        subtitle="Logged in"
        :title="userEmail"
      ></v-list-item>
    </template>

    <v-divider></v-divider>
    
    <!-- Show links based on authentication status -->
    <v-list-item link to="/home" prepend-icon="mdi-home-city" title="Home" value="home" ></v-list-item>
    <v-list-item link to="/posts/create" prepend-icon="mdi-plus" title="Add" value="add" ></v-list-item>
    <v-list-item to="/family" title="Family" prepend-icon="mdi-forum" ></v-list-item>
    <v-list-item to="/account" prepend-icon="mdi-account" title="My Account" value="account" ></v-list-item>

    <!-- Show Logout when logged in -->
    <v-list-item prepend-icon="mdi-logout" title="Logout" value="logout"  @click="logout"></v-list-item>
  </v-navigation-drawer>
</template>

<style scoped>
/* Add custom styles if necessary */
</style>