<script setup lang="ts">
import { UserClient, UserDto } from '@/api/api'
import { Console } from 'console';

const router = useRouter()
const isPasswordVisible = ref(false)

interface User {
  firstName: string
  lastName: string
  email: string
  password: string
}

const client = new UserClient()

const user = ref<User>({
  firstName: '',
  lastName: '',
  email: '',
  password: '',
})

async function submit() {

    const model = new UserDto({
      firstName: user.value.firstName,
      lastName: user.value.lastName,
      email: user.value.email,
      password: user.value.password,
    })

    await client.createUser(model)
    Console.log("User created:", model)

    await router.push('/manage-users')
  }


function required(fieldName: string): (v: string) => true | string {
  return v => !!v || `${fieldName} is required`
}
</script>

<template>
  <HeaderComponent />
  <VCard title="Create Users" class="vcard">
    <VForm
      validate-on="blur"
      @submit.prevent="submit"
    >
      <VCardText>
        <VTextField
          v-model="user.firstName"
          label="Firstname"
          :rules="[required('Firstname')]"
          class="mb-2"
        />
        <VTextField
          v-model="user.lastName"
          label="Lastname"
          :rules="[required('Lastname')]"
          class="mb-2"
        />
        <VTextField
          v-model="user.email"
          label="Email"
          class="mb-2"
        />
        <VTextField
                  v-model="user.password"
                  label="Password"
                  :rules="[required('Password')]"
                  :type="isPasswordVisible ? 'text' : 'password'"
                  :append-inner-icon="isPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'"
                  @click:append-inner="isPasswordVisible = !isPasswordVisible"
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
  <FooterComponent />
</template>

<style scoped>
.vcard {
  margin-top: 170px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 70px;
  width: 70%;
}
</style>