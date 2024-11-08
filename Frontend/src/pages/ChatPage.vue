<template>
  <HeaderComponent />
  <FooterComponent />
  <h1>Chat</h1>
  <form class="add">
    <input
      type="text"
      name="name"
      placeholder="Enter"
      v-model="AddChatWithUser.senderName"
      maxlength="300"
    />
    <button type="button" v-on:click="addChat">Add</button>
  </form>
</template>

<script>
import HeaderComponent from "../components/HeaderComponent.vue";
import FooterComponent from "../components/FooterComponent.vue";
import axios from "axios";

export default {
  name: "ChatPage",
  data() {
    return {
      AddChatWithUser: {
        senderName: ""
      },
    };
  },

  components: {
    HeaderComponent,
    FooterComponent,
  },

  methods: {
    async addChat() {
      console.warn(this.AddChatWithUser);
      const result = await axios.post("http://localhost:3000/AddChatWithUser", {
        senderName: this.AddChatWithUser.senderName
      });

      if (result.status == 201) {
        this.$router.push({ name: "HomePage" });
      }

      console.warn("result", result);
    },
  },

  mounted() {
    let user = localStorage.getItem("user-info");
    if (!user) {
      this.$router.push({ name: "SignUpPage" });
    }
  },
};
</script>

<style scoped>

</style>
