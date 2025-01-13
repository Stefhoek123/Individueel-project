<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { SubmitEventPromise } from "vuetify";
import { FamilyDto } from "@/api/api";
import { FamilyClient } from "@/api/api";

const route = useRoute();
const routeId = (route.params as { id: string }).id;
const router = useRouter();

const familyClient = new FamilyClient();
const familyDto = ref<FamilyDto>();

const errors = ref({
  familyName: "",
});

onMounted(async () => {
  await getFamilyById();
});

async function getFamilyById() {
    familyDto.value = await familyClient.getFamilyById(routeId);
}

async function submit(event: SubmitEventPromise) {
  if (!validateFields()) {
    return;
  }

  const { valid } = await event;

  if (valid) {
    const model = new FamilyDto({
      id: familyDto.value?.id,
      familyName: familyDto.value?.familyName || "",
    });

    await familyClient.updateFamily(model);
    await router.push("/manage-families");
  }
}

function validateFields() {
  errors.value.familyName = familyDto.value?.familyName
    ? ""
    : "Family name is required.";

  return !Object.values(errors.value).some((error) => error !== "");
}

</script>

<template>
  <VCard v-if="familyDto" title="Edit user">
    <VForm validate-on="blur" @submit.prevent="submit">
      <VCardText>
        <VCardTitle class="title-achievement">
          Family ID: {{ familyDto.id }}
        </VCardTitle>
        <VTextField
          v-model="familyDto.familyName"
          label="FamilyName"
          class="mb-2"
          id="familyName"
          clearable
        />
        <p v-if="errors.familyName" class="error">{{ errors.familyName }}</p>
        <VCardActions>
          <VBtn class="card" type="submit" > Save </VBtn>
        </VCardActions>
      </VCardText>
    </VForm>
  </VCard>
</template>

<style scoped>
.number-picker-container {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  margin-block-start: 10px; 
}

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
