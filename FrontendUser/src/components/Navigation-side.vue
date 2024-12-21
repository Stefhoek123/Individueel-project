<script setup lang="ts">
import { ref, onMounted } from "vue";
import { AuthClient } from "@/api/api";

// Simulating authentication check (replace with actual logic)
const isLoggedIn = ref(false); // Reactive variable for logged-in state
const authClient = new AuthClient();

// You can replace this with actual login logic, such as checking for a valid session or token
onMounted(() => {
  // Example: check if the user is logged in by checking a token or session state
  isLoggedIn.value = !!localStorage.getItem("auth_token"); // Assuming you store token in localStorage
});

async function logout() {
  try {
    // Call the logout endpoint
    await authClient.logout();
    alert("Logged out successfully");
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
        lines="two"
        prepend-avatar="../assets/resto-logo.png"
        subtitle="Logged in"
        title="Jane Smith"
        v-if="isLoggedIn"
      ></v-list-item>
    </template>

    <v-divider></v-divider>
    
    <v-list-item link to="/" prepend-icon="mdi-home-city" title="Home" value="home"></v-list-item>
    <v-list-item link to="/posts/create" prepend-icon="mdi-plus" title="Add" value="add" v-if="isLoggedIn"></v-list-item>
    <v-list-item to="/family" title="Family" prepend-icon="mdi-forum" v-if="isLoggedIn"></v-list-item>
    <v-list-item to="/account" prepend-icon="mdi-account" title="My Account" value="account" v-if="isLoggedIn"></v-list-item>

    <!-- Show Login/Sign Up when not logged in -->
    <v-list-item to="/login" prepend-icon="mdi-login" title="Login" value="login" v-if="!isLoggedIn"></v-list-item>
    <v-list-item to="/sign-up" prepend-icon="mdi-account-plus" title="Sign Up" value="sign-up" v-if="!isLoggedIn"></v-list-item>
    <v-list-item prepend-icon="mdi-logout" title="Logout" value="logout" v-if="isLoggedIn" @click="logout()"></v-list-item>
  </v-navigation-drawer>
</template>

<style scoped>
/* Add custom styles if necessary */
</style>
