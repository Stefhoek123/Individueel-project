<script setup lang="ts">
import type { FamilyDto } from "@/api/api";
import { FamilyClient } from "@/api/api";
import ConfirmDialogue from "@/components/ConfirmDialogue.vue";
import { ref, onMounted } from "vue";

const families = ref<FamilyDto[]>([]);
const client = new FamilyClient();
const searchbar = ref<HTMLInputElement | null>(null);

const confirmDialogueRef = ref<InstanceType<typeof ConfirmDialogue> | null>(
  null
);

onMounted(() => getFamilyData());

async function getFamilies() {
  let SearchText = "";
  if (searchbar.value) SearchText = searchbar.value.value;

  try {
    families.value = await client.searchFamilyByName(SearchText);
  } catch (error) {
    console.error("Error fetching families:", error);
  }
}

async function getFamilyData() {
  try {
    const userdata = await client.searchFamilyByName("");
    families.value = userdata;
    const allFamilies = await client.getAllFamilies();
    console.log("All families:", allFamilies);
  } catch (error) {
    console.error("Error fetching family data:", error);
  }
}

async function confirmAndDelete(id: string) {
  const confirmed = await confirmDialogueRef.value?.show({
    title: "Delete family",
    message:
      "Are you sure you want to delete this family and its members? It cannot be undone.",
    okButton: "Delete Forever",
    cancelButton: "Cancel",
  });

  if (confirmed) await deleteFamilyById(id);
}

async function deleteFamilyById(id: string) {
  await client.deleteFamilyById(id);
  getFamilyData();
}
</script>

<template>
  <div>
    <ConfirmDialogue ref="confirmDialogueRef" />
    <VCard title="Manage Families" class="manage-families">
      <VCardText> Families </VCardText>
      <VCardText>
        <div class="wrapper">
          <VTextField
            id="searchbar"
            ref="searchbar"
            style="margin-right: 0.625rem"
            prepend-inner-icon="mdi-search"
            placeholder="Search"
            @input="getFamilies"
          />

          <VBtn to="/families/create" prepend-icon="mdi-plus">
            New Family
          </VBtn>
        </div>
        <VTable>
          <thead>
            <div v-if="families.length === 0">
              <p class="no-results">No results found for your search.</p>
            </div>
            <tr>
              <th class="text-left">Family name</th>
              <th class="text-right actions-column">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in families" :key="item.id">
              <td>
                <RouterLink :to="`/families/${item.id}`">
                  {{ item.familyName }}
                </RouterLink>
              </td>
              <td class="text-right">
                <VBtn
                  icon="mdi-delete"
                  variant="plain"
                  color="accent"
                  size="small"
                  @click="() => item.id && confirmAndDelete(item.id)"
                />
              </td>
            </tr>
          </tbody>
        </VTable>
      </VCardText>
    </VCard>
  </div>
</template>

<style>
.wrapper {
  display: flex;
  justify-content: flex-end;
  inline-size: 100%;
  padding-block: 16px;
}

.actions-column {
  inline-size: 200px;
}

.inline {
  inline-size: 200px;
  max-width: 200px;
}

.manage-families {
  margin-top: 65px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 70px;
  width: 100%;
}
</style>
