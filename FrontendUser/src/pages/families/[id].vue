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

function goToAccount() {
  router.push("/users/:id");
}
</script>

<template>
  <div>
    <NavigationSide />
    <div>
      <VCard v-if="family">
        <!-- Case: Family name is "overig" -->
        <template v-if="family.familyName === 'Overig'">
          <VCardTitle class="title-achievement">
            <p>You are not connected to a family. Send a request to your admin.</p>
          </VCardTitle>
        </template>

        <!-- Case: Family name is NOT "overig" -->
        <template v-else>
          <VCardTitle class="title-achievement">
            {{ family.familyName || "Unknown Family" }}
          </VCardTitle>
          <VTable>
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
        </template>
      </VCard>
    </div>
  </div>
</template>

<style scoped>
.card {
  background-color: #1F7087;
  color: white;
  border-radius: 10px;
  box-shadow: 0 4px 6px 0 rgba(0, 0, 0, 0.1);
  transition: 0.3s;
}
</style>