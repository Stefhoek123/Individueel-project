<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import { PostDto, UserDto } from "@/api/api";
import ConfirmDialogue from "@/components/ConfirmDialogue.vue";
import { PostClient, UserClient } from "@/api/api";

const client = new PostClient();
const userClient = new UserClient();
const post = ref<PostDto | null>(null);
const users = ref<UserDto[]>([]);

const confirmDialogueRef = ref<InstanceType<typeof ConfirmDialogue> | null>(
  null
);

const router = useRouter();
const route = useRoute();
const routeId = (route.params as { id: string }).id;

onMounted(() => {
  getPostsById();
});

async function getPostsById() {
  post.value = await client.getPostById(routeId);
  users.value = await userClient.getUsersByFamilyId(routeId);
}

async function confirmAndDelete(id: string) {
  const confirmed = await confirmDialogueRef.value?.show({
    title: "Delete post",
    message: "Are you sure you want to delete this post? It cannot be undone.",
    okButton: "Delete Forever",
    cancelButton: "Cancel",
  });

  if (confirmed) {
    await deletePostById(id);
  }
}

async function deletePostById(id: string) {
  await client.deletePostById(id);
  await router.push("/");
}
</script>

<template>
  <div>
    <ConfirmDialogue ref="confirmDialogueRef" />
    <div>
      <v-container class="text-center" v-if="post">
        <VCardTitle class="title-achievement">
          {{ post.userId }}
        </VCardTitle>
        <v-row justify="center" class="my-5">
          <v-col cols="12" sm="8">
            <v-card elevation="2" class="pa-5">
              <v-row justify="center">
                <v-col cols="2">
                  <router-link :to="`/`">
                    <VBtn
                      icon="mdi-arrow-left"
                      variant="plain"
                      color="accent"
                      size="small"
                    />
                  </router-link>
                </v-col>
                <v-col cols="6">
                  <p class="headline">&nbsp;</p>
                </v-col>
                <v-col cols="2">
                  <router-link :to="`/posts/update/${routeId}`">
                    <VBtn
                      icon="mdi-pen"
                      variant="plain"
                      color="accent"
                      size="small"
                    />
                  </router-link>
                </v-col>
                <v-col cols="2">
                  <VBtn
                    icon="mdi-delete"
                    variant="plain"
                    color="accent"
                    size="small"
                    @click="confirmAndDelete(routeId)"
                  />
                </v-col>
              </v-row>
              <v-row justify="center">
                <v-img
                  v-if="post.imageUrl"
                  :src="`http://localhost:5190/${post.imageUrl}`"
                  aspect-ratio="4/3"
                  class="mb-4"
                  alt="Post image"
                />
              </v-row>
              <v-row justify="center">
                <v-col cols="12">
                  <p class="caption font-italic">
                   {{ post.textContent }}
                  </p>
                </v-col>
              </v-row>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </div>
  </div>
</template>

<style scoped>
.caption {
  color: white;
}
</style>
