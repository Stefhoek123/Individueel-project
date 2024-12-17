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

    console.log("Model:", model);

    try {
  // Probeer te controleren of het account bestaat
 // await client.checkAccount(model);
  console.log("Account already exists");

  try {
    // Probeer in te loggen als account bestaat
    await client.login(model);
    console.log("Login successful");
    await router.push("/"); // Ga naar de homepage na succesvolle login
  } catch (loginError) {
    console.error("Error logging in:", loginError);
    alert("Login failed. Please try again.");
    await router.push("/login"); // Ga naar de loginpagina bij fout
  }
} catch (checkAccountError) {
  // // Als account niet bestaat, check op specifieke fout
  // if (checkAccountError.response && checkAccountError.response.status === 404) {
  //   console.log("Account not found. Redirecting to sign-up...");
  //   await router.push("/sign-up"); // Stuur gebruiker naar de sign-up pagina
  // } else {
  //   // Onverwachte fouten afhandelen
  //   console.error("Unexpected error checking account:", checkAccountError);
  //   alert("Something went wrong. Please try again later.");
  // }
}
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
