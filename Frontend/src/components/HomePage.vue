<template>
  <HeaderComponent />
  <FooterComponent />
  <table>
    <tr v-for="item in TextPost" :key="item.id">
      <td>
        <img src="../assets/resto-logo.png" alt="" height="200px" /><br />
        {{ item.textContent }}
      </td>
    </tr>
  </table>
</template>

<script>
import HeaderComponent from "./HeaderComponent.vue";
import FooterComponent from "./FooterComponent.vue";

import axios from "axios";

export default {
  name: "HomePage",
  data() {
    return {
      name: "",
      TextPost: [],
    };
  },
  components: {
    HeaderComponent,
    FooterComponent,
  },

  methods: {
    async delete(id) {
      let result = await axios.delete(
        "http://localhost:3000/TextPost/" + id
      );
      console.warn(result);
      if (result.status == 200) {
        this.loadData();
      }
    },
    async loadData() {
      let user = localStorage.getItem("user-info");
      this.name = JSON.parse(user).name;
      if (!user) {
        this.$router.push({ name: "SignUpPage" });
      }

      let result = await axios.get("http://localhost:3000/TextPost");
      console.warn(result);
      this.TextPost = result.data;
    },
  },

  async mounted() {
    this.loadData();
  },
};
</script>

<style scoped>
table {
  margin-top: 70px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 70px;
  border-collapse: collapse;
}
</style>
