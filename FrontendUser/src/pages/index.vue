<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { AuthClient, LoginRequest, UserClient, UserDto } from "@/api/api";

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
const userClient = new UserClient();
const router = useRouter();

async function submit() {
  const model = new LoginRequest({
    email: user.value.email,
    password: user.value.passwordHash,
  });

  // Step 1: Check if the account exists
  const accountCheckResponse = await userClient.checkAccount(model);

  const responseBody = await accountCheckResponse.data.text();
  const accountData = JSON.parse(responseBody);

  if (accountData.message === "Account not found. Please register.") {
    await router.push("/sign-up");
    return;
  }

  console.log("Account exists. Proceed with login.");

  // Step 2: Proceed with login
  const loginResponse = await userClient.login(model);

  const loginResponseBody = await loginResponse.data.text();
  const loginData = JSON.parse(loginResponseBody);

  console.log("Login successful:", loginData);

  if (loginData.message === "Login successful") {

    const model = new UserDto({
      firstName: user.value.firstName,
      lastName: user.value.lastName,
      email: user.value.email,
      passwordHash: user.value.passwordHash,
      familyId: user.value.familyId || "",
      isActive: 1,
    });

    await router.push("/sign-up");
    return;
  }

 

  // Redirect to homepage after successful login
  await router.push("/home");
  // window.location.reload(); // Reload the page if needed, depending on your session management
}

async function signup() {
  await router.push("/sign-up");
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
