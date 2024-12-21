<script setup lang="ts">
import { ref, onMounted } from "vue";
import { AuthClient } from "@/api/api";

// Simulating authentication check (replace with actual logic)
const isLoggedIn = ref(false);
const userEmail = ref("");
const authClient = new AuthClient();

// API check for authentication status
const checkAuth = async () => {
  try {
    const response = await fetch('http://localhost:5190/auth/auth/check', {
      method: 'GET', // Using GET method
      credentials: 'include', // Ensure cookies are sent with the request
    });
    console.log("Authentication check response:", response);

    if (response.ok) {
      const data = await response.json();
      console.log("Authentication check data:", data);
      isLoggedIn.value = data.isAuthenticated;
      userEmail.value = data.email || ''; // Ensure there's no undefined value
    } else {
      console.log('Authentication failed');
      isLoggedIn.value = false;}
    } catch (error) {
    isLoggedIn.value = false;
    console.error("Authentication check failed:", error);
  }
};

onMounted(() => {
  checkAuth(); // Check authentication status on component mount
});

async function logout() {
  try {
    await authClient.logout();
    isLoggedIn.value = false;
    userEmail.value = "";
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
    <!-- Navbar content, showing user info if logged in -->
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
    <v-list-item link to="/" prepend-icon="mdi-home-city" title="Home" value="home" v-if="isLoggedIn"></v-list-item>
    <v-list-item link to="/posts/create" prepend-icon="mdi-plus" title="Add" value="add" v-if="isLoggedIn"></v-list-item>
    <v-list-item to="/family" title="Family" prepend-icon="mdi-forum" v-if="isLoggedIn"></v-list-item>
    <v-list-item to="/account" prepend-icon="mdi-account" title="My Account" value="account" v-if="isLoggedIn"></v-list-item>

    <!-- Show Login/Sign Up when not logged in -->
    <v-list-item to="/login" prepend-icon="mdi-login" title="Login" value="login" v-if="!isLoggedIn"></v-list-item>
    <v-list-item to="/sign-up" prepend-icon="mdi-account-plus" title="Sign Up" value="sign-up" v-if="!isLoggedIn"></v-list-item>

    <!-- Show Logout when logged in -->
    <v-list-item prepend-icon="mdi-logout" title="Logout" value="logout" v-if="!isLoggedIn" @click="logout"></v-list-item>
  </v-navigation-drawer>
</template>

<style scoped>
/* Add custom styles if necessary */
</style>
