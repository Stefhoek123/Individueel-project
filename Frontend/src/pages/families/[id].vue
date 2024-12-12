<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import { FamilyDto, UserDto } from "@/api/api";
import ConfirmDialogue from "@/components/ConfirmDialogue.vue";
import { FamilyClient, UserClient } from "@/api/api";

const client = new FamilyClient();
const userClient = new UserClient();
const family = ref<FamilyDto | null>(null);
const users = ref<UserDto[]>([]);
const user = ref<UserDto | null>(null);
const guid = "10000000-0000-0000-0000-000000000000";

const confirmDialogueRef = ref<InstanceType<typeof ConfirmDialogue> | null>(
  null
);

const route = useRoute();
const routeId = (route.params as { id: string }).id;

onMounted(() => {
  getFamilyAndMembersById();
});

async function getFamilyAndMembersById() {
  family.value = await client.getFamilyById(routeId);
  users.value = await userClient.getUsersByFamilyId(routeId);
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
  const userData = await userClient.getUserById(id);
  if (!userData) {
    console.error("User not found");
    return;
  }

  const model = new UserDto({
    id: userData.id,
    firstName: userData.firstName,
    lastName: userData.lastName,
    email: userData.email,
    passwordHash: userData.passwordHash,
    familyId: guid,
  });

  await userClient.updateUser(model);
  getFamilyAndMembersById();
}
</script>

<template>
  <div>
    <ConfirmDialogue ref="confirmDialogueRef" />
    <div>
      <VCard v-if="family">
        <VCardTitle class="title-achievement">
          {{ family.familyName || "Unknown Family" }}
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
                {{ item.firstName || "Unknown" }}
                {{ item.lastName || "Unknown" }}
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
      </VCard>
    </div>
  </div>
</template>
