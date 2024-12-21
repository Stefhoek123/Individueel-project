<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { AuthClient, LoginRequest } from "@/api/api";

// Define a user interface
interface User {
  email: string;
  password: string; // Updated to match API requirements
}

// Create a reactive user object
const user = ref<User>({
  email: "",
  password: "",
});

const authClient = new AuthClient();
const router = useRouter();

// Function to handle login
// Function to handle login
async function submit() {
  try {
    const model = new LoginRequest({
      email: user.value.email,
      password: user.value.password,
    });

    // Step 1: Check if the account exists
    const accountCheckResponse = await authClient.checkAccount(model);
    if (accountCheckResponse.message === "Account not found. Please register.") {
      alert("Account does not exist. Please register first.");
      return; // Stop further execution if the account doesn't exist
    }

    // Step 2: Proceed with login
    const loginResponse = await authClient.login(model);

    alert("Login successful");
    console.log("Login successful:", loginResponse);

    // Redirect to homepage after successful login
    await router.push("/");
  } catch (error: any) {
    console.error("Error:", error);
    alert(error.message || "An error occurred. Please try again.");
  }
}


// Function to get the profile
async function getProfile() {
  try {
    // Call the profile endpoint
    const response = await authClient.profile();
    console.log("Profile retrieved:", response);
    alert(`User profile: ${JSON.stringify(response)}`);
  } catch (error: any) {
    console.error("Profile error:", error);
    alert(error.message || "Not authenticated");
  }
}

// Function to handle logout
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
  <div class="login">
    <img class="logo" src="../assets/resto-logo.png" width="35px" />
    <h1>Login</h1>
    <VCard title="Login into your account here">
      <VForm validate-on="blur" @submit.prevent="submit">
        <VCardText>
          <VTextField v-model="user.email" label="User email" class="mb-2" />
          <VTextField
            v-model="user.password"
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
