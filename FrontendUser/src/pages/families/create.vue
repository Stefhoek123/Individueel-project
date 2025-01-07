<script setup lang="ts">
import { FamilyClient, FamilyDto } from "@/api/api";
import { useRouter } from "vue-router";
import { ref } from "vue";

const router = useRouter();
const client = new FamilyClient();

interface Family {
  familyName: string;
}

const family = ref<Family>({
  familyName: "",
});

async function submit() {
  const model = new FamilyDto({
    familyName: family.value.familyName,
  });

  await client.createFamily(model);
  await router.push("/manage-families");
}

function required(fieldName: string): (v: string) => true | string {
  return (v) => !!v || `${fieldName} is required`;
}
</script>

<template>
  <VCard title="Create Families" class="vcard">
    <VForm validate-on="blur" @submit.prevent="submit">
      <VCardText>
        <VTextField
          v-model="family.familyName"
          label="Familyname"
          :rules="[required('Familyname')]"
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
