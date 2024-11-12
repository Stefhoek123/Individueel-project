<template>
    <nav class="mobile-nav">
      <router-link to="/" class="bloc-icon">
              <img src="../assets/resto-logo.png" alt="">
              <!-- <img src="../assets/home.png" alt=""> -->
            </router-link>
      </nav>
  </template>
  
  <script>
  export default {
    name: "HeaderComponent",
    methods: {
      logout(){
          localStorage.clear();
          this.$router.push({name: 'LoginPage'});
      }
    }
  };
  </script>
  
  <style>
  .mobile-nav {
    background: #F1F1F1;
    position: fixed;
    top: 0;
    height: 65px;
    width: 100%;
    display: flex;
    justify-content: space-around;
  }
  
  .bloc-icon {
    display: flex;
    justify-content: center;
    align-items: center;
  }
  
  .bloc-icon img {
    width: 30px;
  }
  
  @media screen and (min-width: 600px) {
    .mobile-nav {
    display: none;
    }
  }
  </style>
  