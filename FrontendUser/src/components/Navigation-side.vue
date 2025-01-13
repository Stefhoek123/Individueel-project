<script setup lang="ts">
import { ref } from "vue";
import router from "@/router";

// Simulating authentication check (replace with actual logic)
const isLoggedIn = ref(false);
const userEmail = ref("");

async function logout() {

  await sessionStorage.clear();
  await router.push("/");
    console.log("Logged out successfully");
}
</script>

<template>
  <v-navigation-drawer app permanent>
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
    <v-list-item link to="/families/:id" title="My Family" prepend-icon="mdi-account-group-outline" ></v-list-item>
    <v-list-item link to="/users/:id" prepend-icon="mdi-account" title="My Account" value="account" ></v-list-item>

    <!-- Show Logout when logged in -->
    <v-list-item prepend-icon="mdi-logout" title="Logout" value="logout"  @click="logout"></v-list-item>
  </v-navigation-drawer>
</template>

<style scoped>
/* Add custom styles if necessary */
</style>