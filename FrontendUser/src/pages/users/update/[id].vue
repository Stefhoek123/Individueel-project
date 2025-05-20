<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { SubmitEventPromise } from "vuetify";
import { UserDto } from "@/api/api";
import { UserClient, FamilyClient } from "@/api/api";
import NavigationSide from "@/components/Navigation-side.vue";

const route = useRoute();
const routeId = (route.params as { id: string }).id;
const router = useRouter();
const userClient = new UserClient();
const familyClient = new FamilyClient();
const userDto = ref<UserDto>();
const families = ref<Family[]>([]);

interface User {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  familyId: string;
  isActive: number;
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
  isActive: 1,
});

const errors = ref({
  firstName: "",
  lastName: "",
  email: "",
  familyId: "",
});

onMounted(async () => {
  await getUserbById();
});

async function getUserbById() {
  userDto.value = await userClient.getUserById(routeId);
  const fmaily = await familyClient.getAllFamilies();

    families.value = fmaily.map((family) => ({
        id: family.id || "",
        name: family.familyName,
    }));
}

async function submit(event: SubmitEventPromise) {
  if (!validateFields()) {
    return;
  }

  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (userDto.value && !emailPattern.test(userDto.value.email)) {
    errors.value.email = "Invalid email format.";
    return;
  }

  const { valid } = await event;

  if (valid) {
    const model = new UserDto({
      id: userDto.value?.id,
      firstName: userDto.value?.firstName || "",
      lastName: userDto.value?.lastName || "",
      email: userDto.value?.email || "",
      passwordHash: userDto.value?.passwordHash || "",
      familyId: userDto.value?.familyId,
      isActive: user.value.isActive,
    });

    await userClient.updateUser(model);
    await router.push("/users/:id");
  }
}

function validateFields() {
  errors.value.firstName = userDto.value?.firstName
    ? ""
    : "First name is required.";
  errors.value.lastName = userDto.value?.lastName ? "" : "Last name is required.";
  errors.value.email = userDto.value?.email ? "" : "Email is required.";
  errors.value.familyId = userDto.value?.familyId ? "" : "Family is required.";

  return !Object.values(errors.value).some((error) => error !== "");
}
</script>

<template>
  <div>
      <NavigationSide />
  <VCard v-if="userDto" title="Edit user">
    <VForm validate-on="blur" @submit.prevent="submit">
      <VCardText>
        <VCardTitle class="title-achievement">
         User ID: {{ userDto.id }}
        </VCardTitle>
        <VTextField
          v-model="userDto.firstName"
          label="Firstname"
          class="mb-2"
          clearable
        />
        <p v-if="errors.firstName" class="error">{{ errors.firstName }}</p>
        <VTextField
          v-model="userDto.lastName"
          auto-grow
          label="Lastname"
          class="mb-2"
          clearable
        />
        <p v-if="errors.lastName" class="error">{{ errors.lastName }}</p>
        <VTextField
          v-model="userDto.email"
          auto-grow
          label="Email"
          class="mb-2"
          clearable
        />
        <p v-if="errors.email" class="error">{{ errors.email }}</p>
        <VCardTitle class="title-achievement">
         Family ID: {{ userDto.familyId }}
        </VCardTitle>
        <!-- <VSelect
          v-model="userDto.familyId"
          :items="families"
          item-title="name"
          item-value="id"
          label="Family"
          class="mb-2"
        />
        <p v-if="errors.familyId" class="error">{{ errors.familyId }}</p> -->
        <VCardActions>
          <VBtn class="card" type="submit"> Save </VBtn>
        </VCardActions>
      </VCardText>
    </VForm>
  </VCard>
</div>
</template>

<style scoped>
.error {
  color: red;
  font-size: 0.9em;
  margin-top: -10px;
  margin-bottom: 10px;
}

.card {
  background-color: #1F7087;
  color: white;
  border-radius: 10px;
  box-shadow: 0 4px 6px 0 rgba(0, 0, 0, 0.1);
  transition: 0.3s;
}
</style>