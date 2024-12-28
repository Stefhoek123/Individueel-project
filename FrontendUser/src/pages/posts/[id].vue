<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import { PostDto, UserDto, ChatDto } from "@/api/api";
import ConfirmDialogue from "@/components/ConfirmDialogue.vue";
import { PostClient, UserClient, ChatClient } from "@/api/api";

// Clients
const client = new PostClient();
const userClient = new UserClient();
const chatClient = new ChatClient();

// Reactive Data
const post = ref<PostDto | null>(null);
const users = ref<UserDto[]>([]);
const messageList = ref<ChatDto[]>([]);
const chat = ref<ChatDto>({
  postId: "",
  date: new Date(),
  chatContent: "",
  reactId: "00000000-0000-0000-0000-000000000000",
  senderName: "Test", // Temporary for testing
  userId: { id: "10000000-0000-0000-0000-000000000000" },
});

// Router
const router = useRouter();
const route = useRoute();
const routeId = (route.params as { id: string }).id;

const confirmDialogueRef = ref<InstanceType<typeof ConfirmDialogue> | null>(
  null
);

// Fetch Post Data
onMounted(() => {
  fetchPostDetails();
  fetchMessages();
});

// Fetch post details and users by family ID
async function fetchPostDetails() {
  post.value = await client.getPostById(routeId);
  users.value = await userClient.getUsersByFamilyId(routeId);
}

// Handle delete post confirmation
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

// Delete post by ID
async function deletePostById(id: string) {
  await client.deletePostById(id);
  router.push("/"); // Redirect to home
}

// Fetch chat messages by post ID
async function fetchMessages() {
  messageList.value = await chatClient.getChatsById(routeId);
  messageList.value.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
}

// Send new chat message
async function sendMessage() {
  const model = new ChatDto({
    postId: routeId,
    date: new Date(),
    chatContent: chat.value.chatContent,
    reactId: "00000000-0000-0000-0000-000000000001",
    senderName: "Test", // Replace with actual user data
    userId: "10000000-0000-0000-0000-000000000000",
  });

  await chatClient.createChat(model);
  chat.value.chatContent = ""; // Clear input
  fetchMessages(); // Reload messages
}
</script>

<template>
  <div>
    <ConfirmDialogue ref="confirmDialogueRef" />
    <v-container class="text-center" v-if="post">
      <VCardTitle class="title-achievement">{{ post.userId }}</VCardTitle>

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

            <v-img
              v-if="post.imageUrl"
              :src="`http://localhost:5190/${post.imageUrl}`"
              aspect-ratio="4/3"
              class="mb-4"
              alt="Post image"
            />

            <v-row justify="center">
              <v-col cols="12">
                <p class="caption font-italic">{{ post.textContent }}</p>
              </v-col>
            </v-row>
          </v-card>
        </v-col>
      </v-row>
    </v-container>

    <v-row justify="center">
      <v-col cols="12" sm="7">
        <v-card elevation="2" class="pa-5">
          <v-row justify="center">
            <VTable class="table">
              <thead>
                <tr>
                  <th class="text-left">Sendername</th>
                  <th class="text-right">Message</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in messageList" :key="item.postId">
                  <td>
                    {{ item.senderName }}
                  </td>
                  <td class="text-right">
                    {{ item.chatContent }}
                  </td>
                </tr>
              </tbody>
            </VTable>

            <div class="wrapper">
              <VTextField
                v-model="chat.chatContent"
                label="Chat Content"
                class="mb-2"
              />
              <VBtn class="me-4" type="submit" @click="sendMessage">Send</VBtn>
            </div>
          </v-row>
        </v-card>
      </v-col>
    </v-row>
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

/* Style for individual message blocks */
.message-item {
  margin-bottom: 10px; /* Space between messages */
  padding: 5px;
  border-bottom: 1px solid #ddd; /* Optional, adds a separator between messages */
}

.table {
  inline-size: 100%;
}
</style>
