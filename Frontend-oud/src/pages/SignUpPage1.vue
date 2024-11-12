<template>
  <img class="logo" src="../assets/resto-logo.png" />
  <h1>Sign Up</h1>
  <div class="register">
    <input type="text" v-model="firstName" placeholder="Enter Name" />
    <input type="text" v-model="email" placeholder="Enter Email" />
    <input type="password" v-model="password" placeholder="Enter Password" />
    <button v-on:click="signUp">Sign Up</button>
    <p>
        <router-link to="/login">Login</router-link>
    </p>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "SignUp",
  data() {
    return {
      firstName: "",
      email: "",
      password: "",
    };
  },
  methods: {
    async signUp() {
      let result = await axios.post("http://localhost:5190/api/User/CreateUser", {
        firstName: this.firstName,
        email: this.email,
        password: this.password,
      });

      console.warn(result);
      if (result.status == 201) {
        localStorage.setItem("user-info", JSON.stringify(result.data));
        this.$router.push({ name: "HomePage" });
      }
    },
  },
  mounted(){
    let user = localStorage.getItem("user-info")
    if(user){
        this.$router.push({ name: "HomePage" })
    }
  },
};
</script>

<style scoped>
.logo{
  padding-top: 70px;
}
</style>