<script setup lang="ts">
import { ref, onMounted } from "vue";
import { UserClient, UserDto, FamilyClient } from "@/api/api";
import { useRouter } from "vue-router";
import { SubmitEventPromise } from "vuetify";

const router = useRouter();

interface User {
  firstName: string;
  lastName: string;
  email: string;
  passwordHash: string;
  familyId: string;
}

interface Family {
  id: string;
  name: string;
}

const client = new UserClient();
const familyClient = new FamilyClient();
const families = ref<Family[]>([]);
// const selectedFamilies = ref<string | null>(null);

const user = ref<User>({
  firstName: "",
  lastName: "",
  email: "",
  passwordHash: "",
  familyId: "",
});

onMounted(async () => {
  families.value = await getFamilies();
});

async function getFamilies() {
  const familyDtos = await familyClient.getAllFamilies();
  return familyDtos.map((family) => ({
    id: family.id ?? "",
    name: family.familyName,
  }));
}

async function submit(event: SubmitEventPromise) {
  const { valid } = await event;

  if (valid) {
    const model = new UserDto({
      firstName: user.value.firstName,
      lastName: user.value.lastName,
      email: user.value.email,
      passwordHash: user.value.passwordHash,
      familyId: user.value.familyId || "",
    });

    await client.createUser(model);
    await router.push("/manage-users");
  }
}

function required(fieldName: string): (v: string) => true | string {
  return (v) => !!v || `${fieldName} is required`;
}
</script>

<template>
  <VCard title="Create Users" class="vcard">
    <VForm validate-on="blur" @submit.prevent="submit">
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
          :rules="[required('Email')]"
          class="mb-2"
        />
        <VTextField
          v-model="user.passwordHash"
          label="Password"
          type="password"
          :rules="[required('Password')]"
          class="mb-2"
        />
        <VSelect
          v-model="user.familyId"
          :items="families"
           item-title="name"
          item-value="id"
          label="Family"
          :rules="[required('Family')]"
          class="mb-2"
        />
      </VCardText>
      <VCardActions>
        <VBtn class="me-4" type="submit"> submit </VBtn>
      </VCardActions>
    </VForm>
  </VCard>
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
