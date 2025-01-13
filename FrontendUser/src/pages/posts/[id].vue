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


const errors = ref({
  chatContent: "",
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
    createdAt: getPost.createdAt,
    userId: getPost.userId,
    firstName: userName.value.firstName,
  };

  post.value = model;
}

async function fetchMessages() {
  messageList.value = await chatClient.getChatsById(routeId);
  messageList.value.sort(
    (a, b) => new Date(a.date).getTime() - new Date(b.date).getTime()
  );
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
  await chatClient.deleteChatById(id);
}

async function sendMessage() {
  if (!validateFields()) {
    return;
  }

  const guid = v4();
  if (userName.value.firstName && chat.value.chatContent) {
    await connection.invoke("SendMessage", {
      postId: routeId,
      date: new Date(),
      chatContent: chat.value.chatContent,
      reactId: guid,
      senderName: user.value.firstName,
      userId: user.value.id,
    });

    const model = new ChatDto({
      postId: routeId,
      date: new Date(),
      chatContent: chat.value.chatContent,
      reactId: guid,
      senderName: user.value.firstName,
      userId: user.value.id,
    });
    await chatClient.createChat(model);
    chat.value.chatContent = "";
  } else {
    throw new Error("Message cannot be empty.");
  }
}

function validateFields() {
  errors.value.chatContent = chat.value.chatContent ? "" : "Message is required.";

  return !Object.values(errors.value).some((error) => error !== "");
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
                    color="#30b8dd"
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
                    color="#30b8dd"
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
                  <th class="text-right">&nbsp;</th>
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
                  <td class="text-right" v-if="post.userId === user.id">
                    <VBtn
                      icon="mdi-delete"
                      variant="plain"
                      color="accent"
                      size="small"
                      @click="confirmAndDeleteChat(item.reactId)"
                    />
                  </td>
                  <td class="text-right" v-else>&nbsp;</td>
                </tr>
              </tbody>
            </VTable>

            <div class="wrapper">
              <VTextField
                v-model="chat.chatContent"
                label="Message"
                class="mb-2"
                @keyup.enter="sendMessage"
              />
              <VBtn class="card" type="submit" @click="sendMessage">Send</VBtn>
            </div>
            <p v-if="errors.chatContent" class="error">{{ errors.chatContent }}</p>
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

.error {
  color: red;
  font-size: 0.9em;
  margin-top: -10px;
  margin-bottom: 10px;
}

.card {
  background-color: #1F7087;
  color: white;
  border-radius: 10px;
  box-shadow: 0 4px 6px 0 rgba(0, 0, 0, 0.1);
  transition: 0.3s;
}
</style>
