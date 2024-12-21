<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import { PostDto, UserDto, ChatDto } from "@/api/api";
import ConfirmDialogue from "@/components/ConfirmDialogue.vue";
import { PostClient, UserClient, ChatClient } from "@/api/api";

const client = new PostClient();
const userClient = new UserClient();
const chatClient = new ChatClient();
const post = ref<PostDto | null>(null);
const users = ref<UserDto[]>([]);
const chats = ref<Chat[]>([]);

interface Chat {
  postId: string;
  date: Date;
  chatContent: string;
  reactId: string;
  senderName: string;
  userId: string;
}

const chat = ref<Chat>({
  postId: "",
  date: new Date(),
  chatContent: " ",
  reactId: " ",
  senderName: " ",
  userId: "10000000-0000-0000-0000-000000000000",
});

const confirmDialogueRef = ref<InstanceType<typeof ConfirmDialogue> | null>(
  null
);

const router = useRouter();
const route = useRoute();
const routeId = (route.params as { id: string }).id;

onMounted(() => {
  getPostsById();
  getMessages();
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

async function getMessages() {
  chat.value = await chatClient.getChatById(routeId);
}

function sendMessage() {
  const model = new ChatDto({
    postId: routeId,
    date: new Date(),
    chatContent: chat.value.chatContent,
    reactId: "00000000-0000-0000-0000-000000000001",
    senderName: "Test",
    userId: "10000000-0000-0000-0000-000000000000",
  });

  chatClient.createChat(model);

  chat.value.chatContent = " ";

  getPostsById();
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
    <div>
      <v-row justify="center">
        <v-col cols="12" sm="7">
          <v-card elevation="2" class="pa-5">
            <v-row justify="center">
              <div v-if="chat">
                {{ chat.chatContent }}
              </div>
                <div class="wrapper">
                  <VTextField
                    v-model="chat.chatContent"
                    label="chat content"
                    class="mb-2"
                  />

                  <VBtn class="me-4" type="submit" @click="sendMessage()">
                    Send
                  </VBtn>
                </div>
            </v-row>
          </v-card>
        </v-col>
      </v-row>
    </div>
  </div>
</template>

<style scoped>
.caption {
  color: white;
}

.wrapper {
  display: flex;
  justify-content: flex-end;
  inline-size: 100%;
  padding-block: 16px;
}

.mb-2 {
  margin-left: 10px;
  margin-right: 10px;
}
</style>
