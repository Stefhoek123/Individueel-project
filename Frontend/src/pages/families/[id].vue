<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { FamilyDto, UserDto } from '@/types';
import ConfirmDialogue from '@/components/ConfirmDialogue.vue';
import { FamilyClient, UserClient } from '@/api/api';


const client = new FamilyClient();
const userClient = new UserClient();
const family = ref<FamilyDto| null>(null);
  const users = ref<UserDto[]>([]);

const confirmDialogueRef = ref<InstanceType<typeof ConfirmDialogue> | null>(
  null
);

const route = useRoute();

onMounted(() => {
  console.log('Route Params:', route.params);
  getFamilyAndMembersById();
});

async function getFamilyAndMembersById() {
  family.value = await client.getFamilyById(route.params.id);
  users.value = await userClient.getUsersByFamilyId(route.params.id);
  console.log('Family:', family.value);
  console.log('Users:', users.value);
}

async function confirmAndDelete(id: string) {
  const confirmed = await confirmDialogueRef.value?.show({
    title: "Delete person from family",
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
  getFamilyAndMembersById();
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
              <th class="text-left">Members</th>
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