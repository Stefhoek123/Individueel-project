<script setup lang="ts">
import type { FamilyDto } from "@/api/api";
import { FamilyClient } from "@/api/api";

const families = ref<FamilyDto[]>([]);
const client = new FamilyClient();
const searchbar = ref<HTMLInputElement | null>(null);

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

</script>

<template>
  <div></div>
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

          <VBtn to="/families/create" prepend-icon="mdi-plus"> New Family </VBtn>
        </div>
        <VTable>
          <thead>
            <tr>
              <th class="text-left">Family name</th>
              <th class="text-right actions-column">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in families" :key="item.id">
              <td>
                <router-link :to="`/families/${item.id}`">
                  {{ item.familyName }}
                </router-link>
              </td>
              <td class="text-right">
                <VBtn
                  icon="mdi-pen"
                  variant="plain"
                  color="accent"
                  size="small"
                />
                <VBtn
                  icon="mdi-delete"
                  variant="plain"
                  color="accent"
                  size="small"
                />
              </td>
            </tr>
          </tbody>
        </VTable>
      </VCardText>
    </VCard>

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
