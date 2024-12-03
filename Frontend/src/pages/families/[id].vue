<script setup lang="ts">
import { computed } from 'vue'
import { FamilyDto, FamilyClient, UserClient, UserDto } from '@/api/api'
import { onMounted } from 'vue';
import ConfirmDialogue from '@/components/ConfirmDialogue.vue';

const props = defineProps<{ id: string }>()
const client = new FamilyClient()
const userClient = new UserClient()

// const notificationClient = new NotificationClient()
const family = ref<FamilyDto>()
const users = ref<UserDto>()

const confirmDialogueRef = ref<InstanceType<typeof ConfirmDialogue> | null>(null);

onMounted(() => {
  getFamilyById();
  getUserById();
});

async function getFamilyById() {
  family.value = await client.getFamilyById(props.id)
}

async function getUserById() {
 users.value = await userClient.getUserByFamilyId(props.id)
}

async function confirmAndDelete(id: string) {
  const confirmed = await confirmDialogueRef.value?.show({
    title: "Delete Achievement",
    message: "Are you sure you want to delete this achievement? It cannot be undone.",
    okButton: "Delete Forever",
    cancelButton: "Cancel",
  });

  if (confirmed) {
    await deleteUserByFamilyId(id);
  }
}

async function deleteUserByFamilyId(id: string) {
  await userClient.deleteUserByFamilyId(id);
  getFamilyById(); // Refresh achievements list
}

</script>

<template>
  <div>
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
                <AppLink :to="`/families/update/${item.id}`">
                  <VBtn
                    icon="mdi-pen"
                    variant="plain"
                    color="accent"
                    size="small"
                  />
                </AppLink>
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

<style lang="scss">
.Confetti {
  align-content: baseline;
}

.pill-box {
  display: inline-block; /* Ensure the pill shape fits the content */
  // border: 2px solid #25293c; /* Yellow border */
  border-radius: 50px; /* Rounded edges for pill shape */
  background-color: #645ec1; /* Yellow background (#FDBF40) with 25% opacity */
  box-shadow: 0 4px 6px rgba(0, 0, 0, 10%); /* Add a subtle shadow */
  color: inherit; /* Text color */
  font-size: 12px; /* Text size */
  padding-block: 5px;
  padding-inline: 10px; /* Space inside the box */
  text-align: center; /* Center the text */
  margin: 2px;
}

.title-achievement {
  padding-block-start: 15px;
}
</style>
