<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import { ChatDto } from "@/api/api";
import { ChatClient } from "@/api/api";
import { HubConnectionBuilder } from "@microsoft/signalr";

// Clients
const chatClient = new ChatClient();

// url
const connection = new HubConnectionBuilder().withUrl("http://localhost:5190/chat")
  .build();

connection.start().then(() => {
  console.log("Connected to SignalR Hub");
});

// Reactive Data
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
  userId: "10000000-0000-0000-0000-000000000000",
});

// Router
const route = useRoute();
const routeId = (route.params as { id: string }).id;

// Fetch Post Data
onMounted(() => {
    if (messageList.value.length === 0) {
        fetchMessages();
    }
});

// Fetch chat messages by post ID
async function fetchMessages() {
  messageList.value = await chatClient.getChatsById(routeId);
  messageList.value.sort(
    (a, b) => new Date(a.date).getTime() - new Date(b.date).getTime()
  );
}

// Send new chat message
async function sendMessage() {
  try {
    if (chat.value.senderName && chat.value.chatContent) {
      await connection.invoke(
        "SendMessage",
        chat.value.senderName,
        chat.value.chatContent
      );
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
