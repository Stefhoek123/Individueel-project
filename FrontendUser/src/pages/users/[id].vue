<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { AuthClient, FamilyClient } from "@/api/api";
import HeaderComponent from "@/components/HeaderComponent.vue";
import NavigationSide from "@/components/Navigation-side.vue";

const authClient = new AuthClient();
const familyClient = new FamilyClient();
const user = ref();
const loading = ref(true);

onMounted(async () => {
  try {
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

      const familyUser = await familyClient.getFamilyById(slicedUser.familyId);

      const model = {
        id: slicedUser.id,
        firstName: slicedUser.firstName,
        lastName: slicedUser.lastName,
        email: slicedUser.email,
        passwordHash: slicedUser.passwordHash,
        isActive: slicedUser.isActive,
        familyId: slicedUser.familyId,
        familyName: familyUser.familyName,
      };

      user.value = model;
    } else {
      throw new Error("JWT token is missing");
    }
  } catch (error) {
    console.error("Error loading user data:", error);
  } finally {
    loading.value = false;
  }
});
</script>

<template>
  <div class="clas">
    <HeaderComponent />
    <div class="account-view">
      <NavigationSide />
      <h1>My Account</h1>
      <div v-if="loading">Data loading...</div>
      <div v-else-if="user">
        <p><strong>User ID:</strong> {{ user.id }}</p>
        <p><strong>Firstname:</strong> {{ user.firstName }}</p>
        <p><strong>Lastname:</strong> {{ user.lastName }}</p>
        <p><strong>Email:</strong> {{ user.email }}</p>
        <p><strong>Family Name:</strong> {{ user.familyName }}</p>
        <router-link :to="`/users/update/${user.id}`">
          <VBtn class="ms-2" color="accent">Change data</VBtn>
        </router-link>
      </div>
      <div v-else>
        <p>No data found.</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.account-view {
  max-width: 600px;
  margin: 0 auto;
}

.clas {
  margin-top: 65px;
}
</style>
