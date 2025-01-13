<script setup lang="ts">
import type { UserDto } from "@/api/api";
import { UserClient, FamilyClient } from "@/api/api";
import ConfirmDialogue from "@/components/ConfirmDialogue.vue";
import { ref, onMounted } from "vue";

const users = ref<UserDto[]>([]);
const userClient = new UserClient();
const searchbar = ref<HTMLInputElement | null>(null);
const familyClient = new FamilyClient();

const confirmDialogueRef = ref<InstanceType<typeof ConfirmDialogue> | null>(
  null
);

onMounted(() => getUserData());

async function getUsers() {
  let SearchText = "";
  if (searchbar.value) SearchText = searchbar.value.value;

  try {
    const userData= await userClient.searchUserByEmailOrName(SearchText);
    users.value = userData;
    const familyData = await familyClient.getAllFamilies();
    users.value = userData.map(user => {
      const family = familyData.find(f => f.id === user.familyId);
      return {
        ...user,
        familyName: family ? family.familyName : '',
        init: user.init,
        toJSON: user.toJSON
      };
    });

  } catch (error) {
    console.error("Error fetching users:", error);
  }
}

async function getUserData() {
  try {
    const userData = await userClient.searchUserByEmailOrName("");
    users.value = userData;

    const familyData = await familyClient.getAllFamilies();
    users.value = userData.map(user => {
      const family = familyData.find(f => f.id === user.familyId);
      return {
        ...user,
        familyName: family ? family.familyName : '',
        init: user.init,
        toJSON: user.toJSON
      };
    });

  } catch (error) {
    console.error("Error fetching user data:", error);
  }
}

async function confirmAndDelete(id: string) {
  const confirmed = await confirmDialogueRef.value?.show({
    title: "Delete user",
    message: "Are you sure you want to delete this user? It cannot be undone.",
    okButton: "Delete Forever",
    cancelButton: "Cancel",
  });

  if (confirmed) await deleteUserById(id);
}

async function deleteUserById(id: string) {
  await userClient.deleteUserById(id);
  getUserData();
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
            color="#1F7087"
          />

          <VBtn to="/users/create" prepend-icon="mdi-plus" class="newUser"> New User </VBtn>
        </div>
        <VTable>
          <thead>
            <div v-if="users.length === 0">
              <p class="no-results">No results found for your search.</p>
            </div>
            <tr>
              <th class="text-left">Firstname</th>
              <th class="text-left">Lastname</th>
              <th class="text-left">Email</th>
              <th class="text-left">Family</th>
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
              <td>{{ item.familyName }}</td>
              <td class="text-right">
                <RouterLink :to="`/users/update/${item.id}`">
                  <VBtn
                    icon="mdi-pen"
                    variant="plain"
                    color="#30b8dd"
                    size="small"
                  />
                </RouterLink>
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

.manage-users {
  margin-top: 65px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 70px;
  width: 100%;
}

.newUser{
  background-color: #1F7087;
  color: white;
  border-radius: 10px;
  box-shadow: 0 4px 6px 0 rgba(0, 0, 0, 0.1);
  transition: 0.3s;
}
</style>
