<template>
  <HeaderComponent />
  <FooterComponent />
  <h1>Update</h1>
  <form class="update">
    <input
      type="text"
      name="name"
      placeholder="Enter Name"
      v-model="restaurant.name"
    />
    <button type="button" v-on:click="updateTextPost">
      Update
    </button>
  </form>
</template>

<script>
import HeaderComponent from "../components/HeaderComponent.vue";
import FooterComponent from "../components/FooterComponent.vue";
import axios from "axios";

export default {
  name: "UpdatePage",
  components: {
    HeaderComponent,
    FooterComponent,
  },

  data() {
    return {
      TextPost: {
        textContent: "",
      },
    };
  },

  methods: {
    async updateTextPost() {
      console.warn(this.TextPost);
      const result = await axios.put(
        "http://localhost:5190/api/TextPost/UpdateTextPost" + this.$route.params.id,
        {
          textContent: this.TextPost.textContent,
        }
      );

      if (result.status == 200) {
        this.$router.push({ name: "HomePage" });
      }

      console.warn("result", result);
    },
  },

  async mounted() {
    let user = localStorage.getItem("user-info");
    if (!user) {
      this.$router.push({ name: "SignUpPage" });
    }

    const result = await axios.get(
      "http://localhost:5190/api/TextPost/" + this.$route.params.id
    );

    console.warn(result.data);

    this.TextPost = result.data;
  },
};
</script>
