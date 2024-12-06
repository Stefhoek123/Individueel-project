<script setup lang="ts">
import type { UserDto } from "@/api/api";
import { UserClient } from "@/api/api";
import ConfirmDialogue from "@/components/ConfirmDialogue.vue";

const users = ref<UserDto[]>([]);
const client = new UserClient();
const searchbar = ref<HTMLInputElement | null>(null);

const confirmDialogueRef = ref<InstanceType<typeof ConfirmDialogue> | null>(null)

onMounted(() => getUserData());

async function getUsers() {
  let SearchText = "";
  if (searchbar.value) SearchText = searchbar.value.value;

  try {
    users.value = await client.searchUserByEmailOrName(SearchText);
  } catch (error) {
    console.error("Error fetching users:", error);
  }
}

async function getUserData() {
  try {
    const userdata = await client.searchUserByEmailOrName("");
    users.value = userdata;
    const allUsers = await client.getAllUsers();
    console.log("All users:", allUsers);
  } catch (error) {
    console.error("Error fetching user data:", error);
  }
}

async function confirmAndDelete(id: string) {
  const confirmed = await confirmDialogueRef.value?.show({
    title: 'Delete user',
    message: 'Are you sure you want to delete this user? It cannot be undone.',
    okButton: 'Delete Forever',
    cancelButton: 'Cancel',
  })

  if (confirmed)
    await deleteUserById(id)
}

async function deleteUserById(id: string) {
  await client.deleteUserById(id)
  getUserData()
}

</script>

<template>
  <div>
    <ConfirmDialogue ref="confirmDialogueRef" />

    <VCard title="Manage Users" class="manage-users">
      <VCardText> Users </VCardText>
      <VCardText>
        <div class="wrapper">
          <VTextField
            id="searchbar"
            ref="searchbar"
            style="margin-right: 0.625rem"
            prepend-inner-icon="mdi-search"
            placeholder="Search"
            @input="getUsers"
          />

          <VBtn to="/users/create" prepend-icon="mdi-plus"> New User </VBtn>
        </div>
        <VTable>
          <thead>
            <tr>
              <th class="text-left">Firstname</th>
              <th class="text-left">Lastname</th>
              <th class="text-left">Email</th>
              <th class="text-right actions-column">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in users" :key="item.id">
              <td>
                  {{ item.firstName }}
              </td>
              <td>{{ item.lastName }}</td>
              <td>{{ item.email }}</td>
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

.manage-users {
  margin-top: 65px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 70px;
  width: 100%;
}
</style>
