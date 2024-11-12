<script setup lang="ts">
import type { UserDto } from '@/api/api';
import { UserClient } from '@/api/api';

const users = ref<UserDto[]>([])
const client = new UserClient()
const searchbar = ref<HTMLInputElement | null>(null)

onMounted(() => getUserData())

async function getUsers() {
  let SearchText = ''
  if (searchbar.value)
    SearchText = searchbar.value.value

  users.value = await client.searchUserByEmailOrName(SearchText)
}

async function getUserData(){
  Promise.all(client.searchUserByEmailOrName(''))
    .then(([ userdata]) => {
      users.value = userdata
    })
}

async function Submit(model : UserDto) {
   client.updateUser(model)
}

</script>

<template>
  <div>
    <VCard title="Manage Users">
      <VCardText>
        Manage your employees. Update or Delete the employees.
      </VCardText>
      <VCardText>
        <div class="wrapper">
          <VTextField
            id="searchbar"
            ref="searchbar"
            style="margin-right: 0.625rem;"
            prepend-inner-icon="mdi-search"
            placeholder="Search"
            @input="getUsers"
          />

          <VBtn
            to="/users/create"
            prepend-icon="mdi-plus"
          >
            New User
          </VBtn>
        </div>
        <VTable>
          <thead>
            <tr>
              <th class="text-left">
                User
              </th>
              <th class="text-left">
                Email
              </th>
              <th>
                Departments
              </th>
              <!--              <th class="text-left"> -->
              <!--                Role -->
              <!--              </th> -->
              <th>
                Achievement Progress
              </th>
              <th class="text-right actions-column">
                Actions
              </th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="item in users"
              :key="item.name"
            >
              <td>
                <AppLink
                  :to="`/users/${item.id}`"
                >
                  {{ item.name }}
                </AppLink>
              </td>
              <td>{{ item.email }}</td>
              <td>
                <v-snackbar
                :timeout="2000"
                color="light-green"
                elevation="10"
                v-model="snackbar"
                >
                <template v-slot:activator="{submit}">

                  <v-autocomplete
                  class="inline"
                    v-model="item.labels"
                    :items="labels"
                    item-title="name"
                    item-value="id"
                    label="Select Department"
                    chips
                    multiple
                    @blur="Submit(item)"
                  >
                    <template #chip="{ props, item }">
                      <VChip
                        variant="outlined"
                        color="primary"
                        v-bind="props"
                      >
                        {{ item.title }}
                      </VChip>
                    </template>
                  </v-autocomplete>
                </template>
                  New departments have been assigned
                </v-snackbar>
              </td>
              <!--              <td>{{ item.role }} </td> -->
              <td>
                <VProgressLinear
                  :model-value="item.currentAchievements"
                  :max="item.maxAchievements"
                  color="success"
                />
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

.inline{
  inline-size: 200px;
  max-width: 200px;
}
</style>