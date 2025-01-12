<script setup lang="ts">
import { FamilyClient, FamilyDto } from "@/api/api";
import { useRouter } from "vue-router";
import { ref } from "vue";

const router = useRouter();

interface Family {
  familyName: string;
}

const client = new FamilyClient();

const family = ref<Family>({
  familyName: "",
});

const errors = ref({
  familyName: "",
});

async function submit() {
  if (!validateFields()) {
    return;
  }

  const model = new FamilyDto({
    familyName: family.value.familyName,
  });

  await client.createFamily(model);

  await router.push("/manage-families");
}

function validateFields() {
  errors.value.familyName = family.value.familyName
    ? ""
    : "Family name is required.";

  return !Object.values(errors.value).some((error) => error !== "");
}
</script>

<template>
  <VCard title="Create a family" class="vcard">
    <VForm validate-on="blur" @submit.prevent="submit">
      <VCardText>
        <VTextField
          v-model="family.familyName"
          label="Family name"
          class="mb-2"
          id="familyName"
        />
        <p v-if="errors.familyName" class="error">{{ errors.familyName }}</p>
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

.error {
  color: red;
  font-size: 0.9em;
  margin-top: -10px;
  margin-bottom: 10px;
}
</style>
