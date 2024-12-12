<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { UserClient } from '@/api/api';
import { UserDto } from '@/api/api';
import { useRouter } from 'vue-router';

interface User {
  firstName: string;
  lastName: string;
  email: string;
  passwordHash: string;
}

const client = new UserClient();
const router = useRouter();

const user = ref<User>({
  firstName: '',
  lastName: '',
  email: '',
  passwordHash: '',
});

async function submit() {

    const model = new UserDto({
      firstName: user.value.firstName,
      lastName: user.value.lastName,
      email: user.value.email,
      passwordHash: user.value.passwordHash,
      familyId: '6E051EB1-A3CC-4B9D-84DD-8C11B2704190'
    });

    try {
      const completeModel = await client.createUser(model);
      console.log("User created:", completeModel);
      await router.push({ name: "/" }); 
    } catch (error) {
      console.error("Error creating user:", error);
    }
  }

// Lifecycle hook
onMounted(() => {
  console.log("Signup Page Loaded");
});
</script>

<template>
  <div class="signup">
  <img class="logo" src="../assets/resto-logo.png" width="35px" />
  <h1>Sign Up</h1>
  <VCard title="Create user">
    <VForm
      validate-on="blur"
      @submit.prevent="submit"
    >
      <VCardText>
        <VTextField
          v-model="user.firstName"
          :rules="[('User firstname')]"
          label="User firstname"
          class="mb-2"
        />

        <VTextField
          v-model="user.lastName"
          :rules="[('User lastname')]"
          label="User lastname"
          class="mb-2"
        />

        <VTextField
          v-model="user.email"
          :rules="[('User email')]"
          label="User email"
          class="mb-2"
        />

        <VTextField
          v-model="user.password"
          :rules="[('User password')]"
          label="User password"
          type="password"
          class="mb-2"
        />
      </VCardText>

      <VCardActions>
        <VBtn
          class="me-4"
          type="submit"
        >
          Submit
        </VBtn>
      </VCardActions>
    </VForm>
  </VCard>
</div>
</template>

<style scoped>
.signup{
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding-top: 70px;
}
</style>