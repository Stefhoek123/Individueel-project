<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { UserClient } from '@/api/api'
import { UserDto } from '@/api/api'
import type { SubmitEventPromise } from 'vuetify/lib/framework.mjs'
import { useRouter } from 'vue-router';

interface User {
  firstName: string
  lastName: string
  email: string
  password: string
}

const client = new UserClient()
const router = useRouter()

const user = ref<User>({
  firstName: '',
  lastName: 'test.png',
  email: '',
  password: "",
})


async function submit(event: SubmitEventPromise) {
  const { valid } = await event

  if (valid) {
    const model = new UserDto({
      firstName: user.value.firstName,
      lastName: user.value.lastName,
      email: user.value.email,
      password: user.value.password,
    })

    const completeModel = await client.createUser(model)

    await   router.push({ name: "/" })
  }
}

// Lifecycle hook
onMounted(() => {
  console.log("Signup Page Loaded");
})
</script>

<template>
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
          v-model="user.email"
          :rules="[required('User email')]"
          label="User email"
          class="mb-2"
        />

        <VTextField
          v-model="user.password"
          :rules="[required('User password')]"
          label="User password"
          class="mb-2"
        />

      </VCardText>

      <VCardActions>
        <VBtn
          class="me-4"
          type="submit"
        >
          submit
        </VBtn>
      </VCardActions>
    </VForm>
  </VCard>

  <img class="logo" src="../assets/resto-logo.png" />
  <h1>Sign Up</h1>
  <div class="register">
    <!-- Use v-model for two-way data binding -->
    <!-- <input v-model="firstName" type="text" placeholder="Enter Firstname" />
    <input v-model="lastName" type="text" placeholder="Enter Lastname" />
    <input v-model="email" type="text" placeholder="Enter Email" />
    <input v-model="password" type="password" placeholder="Enter Password" />
    <button @click="signUp">Sign Up</button> -->
    <p>
        <router-link to="/login">Login</router-link>
    </p>
  </div>
</template>

<style scoped>
.logo{
  padding-top: 70px;
}
</style>
