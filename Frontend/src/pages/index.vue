<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import type { TextPostDto } from "@/api/api";
import { TextPostClient } from "@/api/api";

// Register components
const textPosts = ref<TextPostDto[]>([]);
const client = new TextPostClient();


// State variables
const router = useRouter();

onMounted(() => {
  loadData();
});

async function loadData(){
  try {
    const textPostsData = await client.getAllTextPosts();
    textPosts.value = textPostsData;
  } catch (error) {
    console.error("Failed to load text posts:", error);
  }
}


</script>

<template>
  <div class="d-flex justify-center">
    <v-col cols="6">
      <v-card color="#1F7087">
        <div class="d-flex flex-no-wrap justify-space-between">
          <div  v-for="item in textPosts"
          :key="item.id">
            <v-card-title class="text-h5">
              {{ item.textContent }}
              HEllo
            </v-card-title>
          </div>
        </div>
      </v-card>
    </v-col>
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
