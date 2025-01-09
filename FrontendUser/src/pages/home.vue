<script setup lang="ts">
import NavigationSide from '@/components/Navigation-side.vue';
import HeaderComponent from '@/components/HeaderComponent.vue';
import { ref, onMounted, computed } from "vue";
import type { PostDto } from "@/api/api";
import { PostClient } from "@/api/api";

const posts = ref<PostDto[]>([]);
const postClient = new PostClient();

onMounted(() => {
  loadData();
});

async function loadData() {
  const postsData = await postClient.getAllPosts();
  posts.value = postsData.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
}
</script>

<template>
  <div class="outline">
    <HeaderComponent />
    <NavigationSide />
    <div class="d-flex justify-center">
      <v-row justify="center">
        <v-col cols="12" md="8" v-for="item in posts" :key="item.id">
          <v-card color="#1F7087" class="card">
            <div class="d-flex flex-no-wrap justify-space-between">
              <v-card-title class="text-h5">
                <RouterLink :to="`/posts/${item.id}`">
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
.outline {
  margin-top: 70px;
}

.card {
  margin: 20px auto;
  padding: 20px;
  background-color: #1F7087;
  color: white;
  border-radius: 10px;
  box-shadow: 0 4px 6px 0 rgba(0, 0, 0, 0.1);
  transition: 0.3s;
}
</style>
