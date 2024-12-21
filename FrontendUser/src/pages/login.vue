<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { AuthClient, LoginRequest } from "@/api/api";

// Define a user interface
interface User {
  email: string;
  passwordHash: string; // Updated to match API requirements
}

// Create a reactive user object
const user = ref<User>({
  email: "",
  passwordHash: "",
});

const authClient = new AuthClient();
const router = useRouter();

// Function to handle login
async function submit() {
  try {
    const model = new LoginRequest({
      email: user.value.email,
      password: user.value.passwordHash,
    });

    // Step 1: Check if the account exists
    try {
      const accountCheckResponse = await authClient.checkAccount(model);

      // Read the JSON from the response body
      const responseBody = await accountCheckResponse.data.text();
      const accountData = JSON.parse(responseBody);

      if (accountData.message === "Account not found. Please register.") {
        alert("Account does not exist. Please register first.");
        return; // Stop further execution if the account doesn't exist
      }
    } catch (error: any) {
      console.error("Account check error:", error);
      alert(error.message || "Failed to check account.");
      return;
    }
    console.log("Account exists. Proceed with login.");

    // Step 2: Proceed with login
    const loginResponse = await authClient.login(model);
    
    console.log("Login response:", loginResponse);

    // Read the JSON from the login response
    const loginResponseBody = await loginResponse.data.text();
    const loginData = JSON.parse(loginResponseBody);

    alert(loginData.message || "Login successful");
    console.log("Login successful:", loginData);

    // Redirect to homepage after successful login
    await router.push("/");
  } catch (error: any) {
    console.error("Error:", error);
    alert(error.message || "An error occurred. Please try again.");
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
            v-model="user.passwordHash"
            label="User password"
            type="password"
            class="mb-2"
          />
        </VCardText>
        <VCardActions>
          <VBtn class="me-4" type="submit">Login</VBtn>
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
