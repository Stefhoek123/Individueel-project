<script setup lang="ts">
import { ref, onMounted } from "vue";
import type { TextPostDto } from "@/api/api";
import { TextPostClient } from "@/api/api";

// Register components
const textPosts = ref<TextPostDto[]>([]);
const client = new TextPostClient();

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
  <div class="outline">
  <div class="d-flex justify-center">
    <v-row justify="center">
      <v-col cols="12" md="8" v-for="item in textPosts" :key="item.id">
        <v-card color="#1F7087" class="card">
          <div class="d-flex flex-no-wrap justify-space-between"> 
            <v-card-title class="text-h5">
              <RouterLink :to="`/textposts/${item.id}`">
              {{ item.textContent }}
            </RouterLink>
            </v-card-title>
          </div>
        </v-card>
      </v-col>
    </v-row>
  </div>
</div>
</template>


<style scoped>
.outline{
  margin-top: 70px;
}

.card {
  margin: 20px auto; /* Adds vertical spacing and ensures horizontal centering */
  padding: 20px;
  background-color: #1F7087;
  color: white;
  border-radius: 10px;
  box-shadow: 0 4px 6px 0 rgba(0, 0, 0, 0.1);
  transition: 0.3s;
}

</style>
