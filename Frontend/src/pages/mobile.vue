<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import type { TextPostDto } from "@/api/api";
import { TextPostClient } from "@/api/api";
import HeaderComponent from "../components/HeaderComponentMobile.vue";
import FooterComponent from "../components/FooterComponentMobile.vue";

// Register components
const textPosts = ref<TextPostDto[]>([]);
const client = new TextPostClient();

// State variables
const TextPost = ref<Array<{ id: number; textContent: string }>>([]);
const name = ref<string>('');
const router = useRouter();

async function loadData() {
  try {
    const allTextPosts = await client.getAllTextPosts();
    console.log("All textposts:", allTextPosts);
  } catch (error) {
    console.error("Error fetching text posts:", error);
  }
}

onMounted(() => {
  loadData();
});
</script>

<template>
  <div>
    <HeaderComponent />
    <div>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Text Content</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="post in textPosts" :key="post.id">
            <td>{{ post.id }}</td>
            <td>{{ post.textContent }}</td>
          </tr>
        </tbody>
      </table>
    </div>
    <FooterComponent />
  </div>
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
