<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";

// Define a user interface
interface User {
  email: string;
  passwordHash: string;
}

// Create a reactive user object
const user = ref<User>({
  email: "",
  passwordHash: "",
});

const router = useRouter();

// Function to handle login
async function submit() {
  try {
    const response = await fetch("https://localhost:5190/auth/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        username: user.value.email,
        password: user.value.passwordHash,
      }),
      credentials: "include",
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || "Login failed");
    }

    const data = await response.json();
    alert(data.message);
    console.log("Login successful:", data);

    await router.push("/"); // Redirect to homepage after successful login
  } catch (error: any) {
    console.error("Login error:", error);
    alert(error.message || "Login failed. Please try again.");
  }
}

// Function to get the profile
async function getProfile() {
  try {
    const response = await fetch("https://localhost:5190/auth/profile", {
      method: "GET",
      credentials: "include",
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || "Not authenticated");
    }

    const data = await response.json();
    alert(`Logged in as: ${data.username}`);
    console.log("Profile retrieved:", data);
  } catch (error: any) {
    console.error("Profile error:", error);
    alert(error.message || "Not authenticated");
  }
}

// Function to handle logout
async function logout() {
  try {
    const response = await fetch("https://localhost:5190/auth/logout", {
      method: "POST",
      credentials: "include",
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || "Logout failed");
    }

    const data = await response.json();
    alert(data.message);
    console.log("Logged out successfully");
  } catch (error: any) {
    console.error("Logout error:", error);
    alert(error.message || "Logout failed. Please try again.");
  }
}
</script>

<template>
  <div class="login">
    <img class="logo" src="../assets/resto-logo.png" width="35px" />
    <h1>Login</h1>
    <VCard title="Login into your account here">
      <VForm validate-on="blur" @submit.prevent="submit">
        <VCardText>
          <VTextField
            v-model="user.email"
            label="User email"
            class="mb-2"
          />
          <VTextField
            v-model="user.passwordHash"
            label="User password"
            type="password"
            class="mb-2"
          />
        </VCardText>
        <VCardActions>
          <VBtn class="me-4" type="submit">Submit</VBtn>
          <VBtn @click="getProfile" class="me-4">Get Profile</VBtn>
          <VBtn @click="logout" color="error">Logout</VBtn>
        </VCardActions>
      </VForm>
    </VCard>
  </div>
</template>

<style scoped>
.login {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding-top: 70px;
}
</style>
