<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import { ChatDto } from "@/api/api";
import ConfirmDialogue from "@/components/ConfirmDialogue.vue";
import { PostClient, UserClient, ChatClient, AuthClient } from "@/api/api";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { v4 } from "uuid";

const postClient = new PostClient();
const userClient = new UserClient();
const chatClient = new ChatClient();
const authClient = new AuthClient();
const user = ref();
const userName = ref(); 
const post = ref();
const messageList = ref<ChatDto[]>([]);
const router = useRouter();
const route = useRoute();
const routeId = (route.params as { id: string }).id;

const confirmDialogueRef = ref<InstanceType<typeof ConfirmDialogue> | null>(
  null
);

const connection = new HubConnectionBuilder()
  .withUrl("http://localhost:5190/chat")
  .build();

connection.start().then(() => {
  console.log("Connected to SignalR Hub");
});

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
  senderName: "",
  userId: "00000000-0000-0000-0000-000000000000",
});

onMounted(async () => {
   await getUser();
   await fetchPostDetails();

  if (messageList.value.length === 0) {
   await fetchMessages();
  }
  connection.on("ReceiveMessage", (message) => {
    messageList.value.push(message);
  });
  connection.on("DeleteMessage", (id: string) => {
    messageList.value = messageList.value.filter((msg) => msg.reactId !== id);
});
});

async function fetchPostDetails() {
  const getPost = await postClient.getPostById(routeId);
  userName.value = await userClient.getUserById(getPost.userId);

  const model = {
        id: getPost.id,
        textContent: getPost.textContent,
        imageUrl: getPost.imageUrl,
        userId: getPost.userId,
        firstName: userName.value.firstName,
      };

      post.value = model;
}

async function fetchMessages() {
  messageList.value = await chatClient.getChatsById(routeId);
  messageList.value.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
}

async function getUser() {
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

    user.value = slicedUser;
  }
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
  await postClient.deletePostById(id);
  router.push("/home"); 
}

async function confirmAndDeleteChat(id: string) {
  const confirmed = await confirmDialogueRef.value?.show({
    title: "Delete chat",
    message: "Are you sure you want to delete this chat? It cannot be undone.",
    okButton: "Delete Forever",
    cancelButton: "Cancel",
  });

  if (confirmed) {
    await deleteChatById(id);
  }
}

async function deleteChatById(id: string) {
    await connection.invoke("DeleteMessage", id);
    messageList.value = messageList.value.filter((msg) => msg.reactId !== id);
}

async function sendMessage() {
  try {
    const guid = v4();
    if (userName.value.firstName && chat.value.chatContent) {
      await connection.invoke("SendMessage", {
        postId: routeId,
        date: new Date(),
        chatContent: chat.value.chatContent,
        reactId: guid,
        senderName: userName.value.firstName,
        userId: user.value.id,
      });

      const model = new ChatDto({
        postId: routeId,
        date: new Date(),
        chatContent: chat.value.chatContent,
        reactId: guid,
        senderName: userName.value.firstName,
        userId: user.value.id,
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
      <VCardTitle class="title-achievement">{{ post.firstName }}</VCardTitle>

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
              <v-col cols="2" v-if="post.userId === user.id">
                <router-link :to="`/posts/update/${routeId}`">
                  <VBtn
                    icon="mdi-pen"
                    variant="plain"
                    color="accent"
                    size="small"
                  />
                </router-link>
              </v-col>
              <v-col cols="2" v-if="post.userId === user.id">
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
                  <th class="text-right">Delete</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in messageList" :key="item.reactId">
                  <td>
                    {{ item.senderName }}
                  </td>
                  <td class="text-right">
                    {{ item.chatContent }}
                  </td>
                  <td class="text-right">
                    <VBtn
                  icon="mdi-delete"
                  variant="plain"
                  color="accent"
                  size="small"
                  @click="confirmAndDeleteChat(item.reactId)"
                />
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

.message-item {
  margin-bottom: 10px;
  padding: 5px;
  border-bottom: 1px solid #ddd;
}

.table {
  inline-size: 100%;
}
</style>
