<script setup lang="ts">
import { ref, onMounted } from "vue";
import { UserClient, UserDto, FamilyClient } from "@/api/api";
import { useRouter } from "vue-router";
import { SubmitEventPromise } from "vuetify";
import { LoginRequest } from "@/api/api";

const router = useRouter();
const client = new UserClient();
const familyClient = new FamilyClient();
const families = ref<Family[]>([]);

interface User {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  familyId: string;
}

interface Family {
  id: string;
  name: string;
}

const user = ref<User>({
  firstName: "",
  lastName: "",
  email: "",
  password: "",
  familyId: "",
});

const errors = ref({
  firstName: "",
  lastName: "",
  email: "",
  password: "",
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
  if (!validateFields()) {
    return;
  }

  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailPattern.test(user.value.email)) {
    errors.value.email = "Invalid email format.";
    return;
  }

  const passwordPattern = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/; // Minimum 8 characters, at least one letter and one number
  if (!passwordPattern.test(user.value.password)) {
    errors.value.password = "Password must be at least 8 characters long and contain at least one letter and one number.";
    return;
  }

  const { valid } = await event;

  const modelRequest = new LoginRequest({
    email: user.value.email,
    password: user.value.password,
  });

  const email = await client.checkAccount(modelRequest);

  const responseBody = await email.data.text();
  const accountData = JSON.parse(responseBody);

  if (accountData.message === "Account exists") {
    errors.value.email = "Email already exists.";
    return;
  }

  if (valid) {
    const model = new UserDto({
      firstName: user.value.firstName,
      lastName: user.value.lastName,
      email: user.value.email,
      passwordHash: user.value.password,
      familyId: user.value.familyId || "",
    });

    await client.createUser(model);
    await router.push("/manage-users");
  }
}

function validateFields() {
  errors.value.firstName = user.value.firstName
    ? ""
    : "First name is required.";
  errors.value.lastName = user.value.lastName ? "" : "Last name is required.";
  errors.value.email = user.value.email ? "" : "Email is required.";
  errors.value.password = user.value.password ? "" : "Password is required.";
  errors.value.familyId = user.value.familyId ? "" : "Family is required.";

  return !Object.values(errors.value).some((error) => error !== "");
}
</script>

<template>
  <VCard title="Create a user" class="vcard">
    <VForm validate-on="blur" @submit.prevent="submit">
      <VCardText>
        <VTextField
          v-model="user.firstName"
          label="Firstname"
          class="mb-2"
          id="firstName"
        />
        <p v-if="errors.firstName" class="error">{{ errors.firstName }}</p>
        <VTextField
          v-model="user.lastName"
          label="Lastname"
          class="mb-2"
          id="lastName"
        />
        <p v-if="errors.lastName" class="error">{{ errors.lastName }}</p>
        <VTextField
          v-model="user.email"
          label="Email"
          class="mb-2"
          id="email"
        />
        <p v-if="errors.email" class="error">{{ errors.email }}</p>
        <VTextField
          v-model="user.password"
          label="Password"
          type="password"
          class="mb-2"
          id="password"
        />
        <p v-if="errors.password" class="error">{{ errors.password }}</p>
        <VSelect
          v-model="user.familyId"
          :items="families"
          item-title="name"
          item-value="id"
          label="Family"
          class="mb-2"
        />
        <p v-if="errors.familyId" class="error">{{ errors.familyId }}</p>
      </VCardText>
      <VCardActions>
        <VBtn class="card" type="submit"> submit </VBtn>
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

.error {
  color: red;
  font-size: 0.9em;
  margin-top: -10px;
  margin-bottom: 10px;
}

.card {
  background-color: #1f7087;
  color: white;
  border-radius: 10px;
  box-shadow: 0 4px 6px 0 rgba(0, 0, 0, 0.1);
  transition: 0.3s;
}
</style>
