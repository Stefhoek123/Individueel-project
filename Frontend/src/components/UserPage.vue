<template>
  <HeaderComponent />
  <FooterComponent />
  <h1>User</h1>

  <div class="userInfo">
    <img src="../assets/resto-logo.png" alt="" height="100px" />
    <div class="userDetails">
      <p>
        {{ firstName }} Stefanie<br />
        {{ lastName }} Hoeksma<br />
        {{ email }} stefanie@gmail.com
      </p>
    </div>
  </div>

  <div class="userDetails">
    <p>
      Onderdeel van de families:<br />
      {{ familyName }} Hoeksma<br />
      {{ familyName }} Nillesen
    </p>
  </div>

  <table border="1px">
    <tr v-for="item in TextPost" :key="item.id">
      <td>
        {{ item.id }}
      </td>
      <td>
        {{ item.textContent }}
      </td>
    </tr>
  </table>
  <img src="../assets/editing.png" alt="" height="30px" />
  <a v-on:click="logout" href="#" class="bloc-icon"><img src="../assets/exit.png" alt="" height="25px" /></a>
</template>

<script>
import HeaderComponent from "./HeaderComponent.vue";
import FooterComponent from "./FooterComponent.vue";

import axios from "axios";

export default {
  name: "UserPage",
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

    logout(){
        localStorage.clear();
        this.$router.push({name: 'LoginPage'});
    }
  },

  async mounted() {
    this.loadData();
  },
};
</script>

<style scoped>
td {
  width: 160px;
  height: 40px;
}

table {
  margin: 4px;
  margin-bottom: 70px;
}

.userInfo {
  display: flex;
  align-items: center; /* Vertically align items */
  margin-left: 20px;
}

.userDetails {
  display: flex;
  flex-direction: column;
  margin-left: 20px;
  text-align: left;
}
</style>
