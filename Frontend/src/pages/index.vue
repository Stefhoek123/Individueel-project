<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import HeaderComponent from "../components/HeaderComponent.vue";
import FooterComponent from "../components/FooterComponent.vue";
import axios from "axios";

// Register components
const components = { HeaderComponent, FooterComponent };

// State variables
const TextPost = ref<Array<{ id: number; textContent: string }>>([]);
const name = ref<string>('');
const router = useRouter();

// Function to load data
async function loadData() {
  // to do: kijken of de user is ingelogd. anders doorsturen naar signup
  // const user = localStorage.getItem("user-info");
  // if (!user) {
  //   router.push({ name: "/SignUp" });
  //   return;
  // }

  // name.value = JSON.parse(user).name;
  
  try {
    const result = await axios.get("http://localhost:5190/api/TextPost/GetAllTextPosts");
    console.warn(result);
    // TextPost.value = result.data;
  } catch (error) {
    console.error("Error loading data:", error);
  }
}

// Function to delete an item
// async function deleteItem(id: number) {
//   try {
//     const result = await axios.delete(`http://localhost:5190/api/TextPost/${id}`);
//     console.warn(result);
//     if (result.status === 200) {
//       loadData(); // Reload data after deletion
//     }
//   } catch (error) {
//     console.error("Error deleting item:", error);
//   }
// }

// Lifecycle hook
onMounted(() => {
  loadData();
});
</script>

<template>
  <HeaderComponent />
  <FooterComponent />
  <table>
    <tr v-for="item in TextPost" :key="item.id">
      <td>
        <img src="../assets/resto-logo.png" alt="" height="200px" /><br />
        {{ item.textContent }}
      </td>
    </tr>
  </table>
</template>

<style scoped>
table {
  margin-top: 70px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 70px;
  border-collapse: collapse;
}
</style>
