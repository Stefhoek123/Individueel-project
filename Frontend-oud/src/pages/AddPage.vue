<template>
  <HeaderComponent />
  <FooterComponent />
  <h1>Add</h1>
  <form class="add">
    <input
      type="text"
      name="name"
      placeholder="Enter"
      v-model="TextPost.textContent"
      maxlength="300"
    />
    <button type="button" v-on:click="addTextPost">Add</button>
  </form>
  <br />
  <FileButton />
</template>

<script>
import HeaderComponent from "../components/HeaderComponent.vue";
import FooterComponent from "../components/FooterComponent.vue";
import FileButton from "../components/FileButton.vue";
import axios from "axios";

export default {
  name: "AddPage",
  components: {
    HeaderComponent,
    FooterComponent,
    FileButton,
  },

  data() {
    return {
      TextPost: {
        textContent: "",
      },
    };
  },

  methods: {
    async addTextPost() {
      console.warn(this.TextPost);
      const result = await axios.post("http://localhost:5190/api/TextPost/", {
        textContent: this.TextPost.textContent
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
