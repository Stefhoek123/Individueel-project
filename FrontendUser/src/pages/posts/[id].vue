<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import { PostDto, UserDto, ChatDto } from "@/api/api";
import ConfirmDialogue from "@/components/ConfirmDialogue.vue";
import { PostClient, UserClient, ChatClient } from "@/api/api";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { v4 as uuidv4 } from 'uuid';

// Clients
const postClient = new PostClient();
const userClient = new UserClient();
const chatClient = new ChatClient();

// url
const connection = new HubConnectionBuilder()
  .withUrl("http://localhost:5190/chat")
  .build();

connection.start().then(() => {
  console.log("Connected to SignalR Hub");
});

// Reactive Data
const post = ref<PostDto | null>(null);
const users = ref<UserDto[]>([]);
const messageList = ref<ChatDto[]>([]);

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
  chatContent: "",
  reactId: "00000000-0000-0000-0000-000000000000",
  senderName: "Test", // Temporary for testing
  userId: "00000000-0000-0000-0000-000000000000",
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
  if (messageList.value.length === 0) {
    fetchMessages();
  }
  connection.on("ReceiveMessage", (message) => {
    messageList.value.push(message);
  });

  fetchPostDetails();
});

// Fetch post details and users by family ID
async function fetchPostDetails() {
  post.value = await postClient.getPostById(routeId);
  users.value = await userClient.getUsersByFamilyId(routeId);
}

// Fetch chat messages by post ID
async function fetchMessages() {
  messageList.value = await chatClient.getChatsById(routeId);
  messageList.value.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
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
  await postClient.deletePostById(id);
  router.push("/"); // Redirect to home
}

// Send new chat message
async function sendMessage() {
  try {
    if (chat.value.senderName && chat.value.chatContent) {
      await connection.invoke("SendMessage", {
        postId: routeId,
        date: new Date(),
        chatContent: chat.value.chatContent,
        reactId: chat.value.reactId,
        senderName: chat.value.senderName,
        userId: chat.value.userId,
      });

      const model = new ChatDto({
        postId: routeId,
        date: new Date(),
        chatContent: chat.value.chatContent,
        reactId: "00000000-0000-0000-0000-000000000001",
        senderName: chat.value.senderName,
        userId: chat.value.userId,
      });
      await chatClient.createChat(model);
      chat.value.chatContent = "";
    } else {
      throw new Error("Chat content cannot be empty.");
    }
  } catch (error) {
    console.error("Failed to send message:", error);
    alert("Unable to send your message. Please try again.");
  }
}
</script>

<template>
  <div>
    <NavigationSide />
    <ConfirmDialogue ref="confirmDialogueRef" />
    <v-container class="text-center" v-if="post">
      <VCardTitle class="title-achievement">{{ post.userId }}</VCardTitle>

      <v-row justify="center" class="my-5">
        <v-col cols="12" sm="8">
          <v-card elevation="2" class="pa-5">
            <v-row justify="center">
              <v-col cols="2">
                <router-link :to="`/home`">
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
