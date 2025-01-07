<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import type { SubmitEventPromise } from "vuetify/lib/framework.mjs";
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

onMounted(async () => {
    console.log(routeId);
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


// Form submission
async function submit(event: SubmitEventPromise) {
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

// Validation helper
function required(fieldName: string): (v: string) => true | string {
  return (v) => !!v || `${fieldName} is required`;
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
          :rules="[required('Firstname')]"
          label="Firstname"
          class="mb-2"
          clearable
        />
        <VTextField
          v-model="userDto.lastName"
          auto-grow
          label="Lastname"
          :rules="[required('Lastname')]"
          class="mb-2"
        />
        <VTextField
          v-model="userDto.email"
          auto-grow
          label="Email"
          :rules="[required('Email')]"
          class="mb-2"
        />
        <VSelect
          v-model="userDto.familyId"
          :items="families"
          item-title="name"
          item-value="id"
          label="Family"
          :rules="[required('Family')]"
          class="mb-2"
        />

        <!-- Save Button -->
        <VCardActions>
          <VBtn class="me-4" type="submit"> Save </VBtn>
        </VCardActions>
      </VCardText>
    </VForm>
  </VCard>
</div>
</template>

<style scoped>
</style>
