<script setup lang="ts">
import { ref } from "vue";
import { UserClient } from "@/api/api";
import { UserDto } from "@/api/api";
import { useRouter } from "vue-router";

interface User {
  firstName: string;
  lastName: string;
  email: string;
  passwordHash: string;
}

const client = new UserClient();
const router = useRouter();

const user = ref<User>({
  firstName: "",
  lastName: "",
  email: "",
  passwordHash: "",
});

async function submit() {
  try {
    const model = new UserDto({
      firstName: "",
      lastName: "",
      email: user.value.email,
      passwordHash: user.value.passwordHash,
      familyId: " ",
    });

    // Controleer of account bestaat
    const accountCheck = await client.checkAccount(model);
    if (accountCheck == null) {
      // Geen account gevonden -> ga naar sign-up
      await router.push("/sign-up");
      return;
    }

    // Probeer in te loggen
    const loginResponse = await client.login(model);
    if (loginResponse == null) {
      // Login niet succesvol -> ga naar homepage
      await router.push("/login");
      return;
    }

    // Login succesvol -> navigeer naar dashboard
    await router.push("/");
  } catch (error) {
    console.error("Er is een fout opgetreden:", error);
    alert("Er is iets misgegaan. Probeer het later opnieuw.");
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
          <VBtn class="me-4" type="submit"> Submit </VBtn>
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
