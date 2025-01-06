<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { LoginRequest, UserClient, UserDto, AuthClient } from "@/api/api";

// Define a user interface
interface User {
  firstName: string;
  lastName: string;
  email: string;
  passwordHash: string;
  familyId: string;
  isActive: number;
}

// Create a reactive user object
const user = ref<User>({
  firstName: "",
  lastName: "",
  email: "",
  passwordHash: "",
  familyId: "",
  isActive: 0,
});

const authClient = new AuthClient();
const router = useRouter();
const userId = ref("");

async function submit() {
  const model = new LoginRequest({
    email: user.value.email,
    password: user.value.passwordHash,
  });

  const accountCheckResponse = await authClient.checkAccount(model);

  const responseBody = await accountCheckResponse.data.text();
  const accountData = JSON.parse(responseBody);

  if (accountData.message === "Account not found. Please register.") {
    await router.push("/sign-up");
    return;
  }
  console.log("Account exists. Proceed with login.");
  // Step 2: Proceed with login
  const loginResponse = await authClient.login(model);

  const loginResponseBody = await loginResponse.data.text();
  const loginData = JSON.parse(loginResponseBody);

  userId.value = loginData.userIdSession;

  const authResponse = await authClient.checkAuthStatus();
  const authResponseBody = await authResponse.data.text();
  const authData = JSON.parse(authResponseBody);
  console.log("Auth status:", authData);

  await router.push("/home");
}

async function signup() {
  await router.push("/sign-up");
}
</script>

<template>
  <div class="login">
    <HeaderComponent />
    <NavigationSide />
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
          <VBtn class="me-4" type="submit">Login</VBtn> or
          <VBtn class="me-4" @click="signup">Sign up</VBtn>
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
