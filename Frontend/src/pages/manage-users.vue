<script setup lang="ts">
import type { UserDto } from "@/api/api";
import { UserClient } from "@/api/api";
import AppLink from "@/components/AppLink.vue";

const users = ref<UserDto[]>([]);
const client = new UserClient();
const searchbar = ref<HTMLInputElement | null>(null);

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

</script>

<template>
  <div>
    <VCard title="Manage Users">
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
                <AppLink :to="`/users/${item.id}`">
                  {{ item.firstName }}
                </AppLink>
              </td>
              <td>{{ item.lastName }}</td>
              <td>{{ item.email }}</td>
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
</style>
