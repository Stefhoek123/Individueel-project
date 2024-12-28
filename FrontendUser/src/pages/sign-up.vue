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
  const model = new UserDto({
    firstName: user.value.firstName,
    lastName: user.value.lastName,
    email: user.value.email,
    passwordHash: user.value.passwordHash,
    familyId: "10000000-0000-0000-0000-000000000000",
  });

  try {
    await client.checkAccount(model);
    console.log("Account already exists");
    await router.push("/login");
  } catch (error) {
    console.error("Error creating user:", error);
    const completeModel = await client.createUser(model);
    console.log("User created:", completeModel);
    await router.push({ name: "/" });
  }
}

async function login() {
  await router.push("/");
}

</script>

<template>
  <div class="signup">
    <img class="logo" src="../assets/resto-logo.png" width="35px" />
    <h1>Sign Up</h1>
    <VCard title="Register your account here" class="title">
      <VForm validate-on="blur" @submit.prevent="submit">
        <VCardText>
          <VTextField
            v-model="user.firstName"
            label="User firstname"
            class="mb-2"
          />

          <VTextField
            v-model="user.lastName"
            label="User lastname"
            class="mb-2"
          />

          <VTextField v-model="user.email" label="User email" class="mb-2" />

          <VTextField
            v-model="user.passwordHash"
            label="User password"
            type="password"
            class="mb-2"
          />
        </VCardText>

        <VCardActions>
          <VBtn class="me-4" type="submit">Sign up</VBtn>
          or 
          <VBtn class="me-4" @click="login">Login</VBtn>
        </VCardActions>
      </VForm>
    </VCard>
  </div>
</template>

<style scoped>
.signup {
  display: flex;
  flex-direction: column;
  align-items: center;
  /* justify-content: center; */
  padding-top: 70px;
}
</style>
