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
  const modelLogin = new LoginRequest({
    email: user.value.email,
    password: user.value.passwordHash,
  });

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

  const accountCheckResponse = await authClient.checkAccount(modelLogin);

  const responseBody = await accountCheckResponse.data.text();
  const accountData = JSON.parse(responseBody);

  if (accountData.message === "Account not found. Please register.") {
    await userClient.createUser(modelDto);
    await authClient.login(model);
  } else {
    await router.push("/");
    return;
  }

  await router.push("/home");
}

function login() {
  router.push("/");
}
</script>

<template>
  <div class="signup">
    <HeaderComponent />
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
  padding-top: 70px;
}
</style>
