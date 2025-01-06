<script setup lang="ts">
import { ref } from "vue";
import { UserClient, AuthClient, LoginRequest, LoginRequestDto, UserDto } from "@/api/api";
import { useRouter } from "vue-router";

interface User {
  firstName: string;
  lastName: string;
  email: string;
  passwordHash: string;
}

const userClient = new UserClient();
const authClient = new AuthClient();
const router = useRouter();

const user = ref<User>({
  firstName: "",
  lastName: "",
  email: "",
  passwordHash: "",
});

async function submit() {
  const model = new LoginRequestDto({
    email: user.value.email,
    password: user.value.passwordHash,
  });

  const modelDto = new UserDto({
    firstName: user.value.firstName,
    lastName: user.value.lastName,
    email: user.value.email,
    passwordHash: user.value.passwordHash,
    familyId: "10000000-0000-0000-0000-000000000000",
    isActive: 2,
  });

  const userExists = await authClient.login(model);

console.log("User exists:", userExists);

  if (!userExists) {
    await userClient.createUser(modelDto);
    await authClient.login(model);
    await router.push("/home");
  } else {
    await router.push("/home");
  }
}

function login() {
  router.push("/");
}
</script>

<template>
  <div class="signup">
    <HeaderComponent />
    <NavigationSide />
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
