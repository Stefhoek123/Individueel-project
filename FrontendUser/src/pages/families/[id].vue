<script setup lang="ts">
import { ref, onMounted } from "vue";
import { FamilyDto, UserDto } from "@/api/api";
import NavigationSide from "@/components/Navigation-side.vue";
import { FamilyClient, UserClient, AuthClient } from "@/api/api";
import { useRouter } from "vue-router";

const familyClient = new FamilyClient();
const userClient = new UserClient();
const authClient = new AuthClient();
const family = ref<FamilyDto | null>(null);
const users = ref<UserDto[]>([]);
const user = ref();
const router = useRouter();

onMounted(() => {
  getUser();
});

async function getUser() {
  const token = sessionStorage.getItem("JWT");
  if (token) {
    const currentUser = await authClient.getCurrentUser(token);

    const userData = JSON.parse(await currentUser.data.text());

    const slicedUser = {
      id: userData.id,
      firstName: userData.firstName,
      lastName: userData.lastName,
      email: userData.email,
      passwordHash: userData.passwordHash,
      isActive: userData.isActive,
      familyId: userData.familyId,
    };

    user.value = slicedUser;
    family.value = await familyClient.getFamilyById(slicedUser.familyId);
    users.value = await userClient.getUsersByFamilyId(slicedUser.familyId);
  }
}

function createFamily() {
  router.push("/families/create");
}
</script>

<template>
  <div>
    <NavigationSide />
    <div>
      <VCard v-if="family">
        <VCardTitle class="title-achievement" v-if="family.familyName != 'overig'">
          <p>You are not connected to a family.</p>
          <VBtn class="ms-2" color="#1F7087" @click="createFamily">Click this button to create a family</VBtn>
        </VCardTitle>
        <VCardTitle class="title-achievement" v-if="family.familyName == 'overig'">
          {{ family.familyName || "Unknown Family" }}
        </VCardTitle>
        <VTable v-if="family.familyName == 'overig'">
          <thead>
            <tr>
              <th class="text-left">Members</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in users" :key="item.id">
              <td>
                {{ item.firstName || "Unknown" }}
                {{ item.lastName || "Unknown" }}
              </td>
             </tr>
          </tbody>
        </VTable>
      </VCard>
    </div>
  </div>
</template>
