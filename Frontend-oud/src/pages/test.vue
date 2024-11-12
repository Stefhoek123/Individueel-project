<template>
  <HeaderComponent />
  <FooterComponent />
  <h1>Hello {{ name }}, Welcome on Home page</h1>
  <table border="1px">
    <tr>
      <td>Id</td>
      <td>Name</td>
      <td>Contact</td>
      <td>Address</td>
      <td>Actions</td>
    </tr>
    <tr v-for="item in restaurant" :key="item.id">
      <td>
        {{ item.id }}
      </td>
      <td>
        {{ item.name }}
      </td>
      <td>
        {{ item.contact }}
      </td>
      <td>
        {{ item.address }}
      </td>
      <td>
        <router-link :to="'/update/' + item.id">Update</router-link>
        <button v-on:click="deleteRestaurant(item.id)">Delete</button>
      </td>
    </tr>
  </table>
</template>

<script>
import HeaderComponent from "../components/HeaderComponent.vue";
import FooterComponent from "../components/FooterComponent.vue";

import axios from "axios";

export default {
  name: "HomePage",
  data() {
    return {
      name: "",
      restaurant: [],
    };
  },
  components: {
    HeaderComponent,
    FooterComponent,
  },

  methods: {
    async deleteRestaurant(id) {
      let result = await axios.delete(
        "http://localhost:3000/restaurants/" + id
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

      let result = await axios.get("http://localhost:3000/restaurants");
      console.warn(result);
      this.restaurant = result.data;
    },
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

table{
  margin: 4px;
}
</style>
