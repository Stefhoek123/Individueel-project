<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { LoginRequestDto, AuthClient } from "@/api/api";

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

async function submit() {

  const modelDto = new LoginRequestDto({
    email: user.value.email,
    password: user.value.passwordHash,
  });

 const login = await authClient.login(modelDto);

 console.log(login);

 if (login.accessToken) {
   sessionStorage.setItem("JWT", login.accessToken);
 }

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
