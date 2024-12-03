<script setup lang="ts">
import { FamilyDto, FamilyClient, UserClient, UserDto } from "@/api/api";
import { onMounted } from "vue";
import ConfirmDialogue from "@/components/ConfirmDialogue.vue";
import { useRoute } from 'vue-router';

const props = defineProps<{ id: string }>();
const client = new FamilyClient();
const userClient = new UserClient();
const family = ref<FamilyDto>();
const users = ref<UserDto>();

const confirmDialogueRef = ref<InstanceType<typeof ConfirmDialogue> | null>(
  null
);

const route = useRoute();
console.log('Route Params:', route.params);

onMounted(() => {
  console.log("Props " + props.id);
  getFamilyById();
  getUserByFamilyId();
});

async function getFamilyById() {
  family.value = await client.getFamilyById(props.id);
}

async function getUserByFamilyId() {
  users.value = await userClient.getUserByFamilyId(props.id);
}

async function confirmAndDelete(id: string) {
  const confirmed = await confirmDialogueRef.value?.show({
    title: "Delete Person from Family",
    message:
      "Are you sure you want to delete this person from this family? It cannot be undone.",
    okButton: "Delete Forever",
    cancelButton: "Cancel",
  });

  if (confirmed) {
    await deleteUserByFamilyId(id);
  }
}

async function deleteUserByFamilyId(id: string) {
  await userClient.deleteUserByFamilyId(id);
  getFamilyById();
}
</script>

<template>
  <div>
    <ConfirmDialogue ref="confirmDialogueRef" />
    <div>
      <VCard v-if="family">
        <VCardTitle class="title-achievement">
          {{ family.familyName }}
        </VCardTitle>
        <VTable>
          <thead>
            <tr>
              <th class="text-left">Persons</th>
              <th class="text-right actions-column">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in users" :key="item.id">
              <td>
                {{ item.firstName }}
                {{ item.lastName }}
              </td>
              <td class="text-right">
                <router-link :to="`/families/update/${item.id}`">
                  <VBtn
                    icon="mdi-pen"
                    variant="plain"
                    color="accent"
                    size="small"
                  />
                </router-link>
                <VBtn
                  icon="mdi-delete"
                  variant="plain"
                  color="accent"
                  size="small"
                  @click="confirmAndDelete(item.id!)"
                />
              </td>
            </tr>
          </tbody>
        </VTable>
      </VCard>
    </div>
  </div>
</template>

<style lang="ts">

</style>
