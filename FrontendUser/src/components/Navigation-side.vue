<script setup lang="ts">

</script>

<template>
    <v-navigation-drawer>
  
  <template v-slot:prepend>
          <v-list-item
            lines="two"
            prepend-avatar="../assets/resto-logo.png"
            subtitle="Logged in"
            title="Jane Smith"
          ></v-list-item>
        </template>

  <v-divider></v-divider>
  <v-list-item link to="/" prepend-icon="mdi-home-city" title="Home" value="home"></v-list-item>
  <v-list-item link to="/posts/create" prepend-icon="mdi-plus" title="Add" value="add"></v-list-item>
  <v-list-item to="/family" title="Family" prepend-icon="mdi-forum"></v-list-item>
    <v-list-item to="/account" prepend-icon="mdi-account" title="My Account" value="account"></v-list-item>
    <v-list-item to="/login" prepend-icon="mdi-login" title="Login" value="login"></v-list-item>
    <v-list-item to="/sign-up" prepend-icon="mdi-account-plus" title="Sign Up" value="sign-up"></v-list-item>
</v-navigation-drawer>
  </template>
    
  <style>  
  </style>
  