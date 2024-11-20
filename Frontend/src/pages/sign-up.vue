<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { UserClient } from '@/api/api';
import { UserDto } from '@/api/api';
import { useRouter } from 'vue-router';
import HeaderComponent from '@/components/HeaderComponentMobile.vue';
import FooterComponent from '@/components/FooterComponentMobile.vue';

interface User {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

const client = new UserClient();
const router = useRouter();

const user = ref<User>({
  firstName: '',
  lastName: '',
  email: '',
  password: '',
});

async function submit() {

    const model = new UserDto({
      firstName: user.value.firstName,
      lastName: user.value.lastName,
      email: user.value.email,
      password: user.value.password,
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
  <HeaderComponent />
  <img class="logo" src="../assets/resto-logo.png" />
  <h1>Sign Up</h1>
  <VCard title="Create user">
    <VForm
      validate-on="blur"
      @submit.prevent="submit"
    >
      <VCardText>
        <VTextField
          v-model="user.firstName"
          :rules="[required('User firstname')]"
          label="User firstname"
          class="mb-2"
        />

        <VTextField
          v-model="user.lastName"
          :rules="[required('User lastname')]"
          label="User lastname"
          class="mb-2"
        />

        <VTextField
          v-model="user.email"
          :rules="[required('User email')]"
          label="User email"
          class="mb-2"
        />

        <VTextField
          v-model="user.password"
          :rules="[required('User password')]"
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
  <FooterComponent />
</template>

<style scoped>
.logo {
  padding-top: 70px;
}
</style>