<script setup lang="ts"></script>

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
    <v-list-item
      link
      to="/"
      prepend-icon="mdi-home-city"
      title="Home"
      value="home"
    ></v-list-item>
    <v-list-item
      to="/manage-users"
      title="Manage users"
      prepend-icon="mdi-account-group-outline"
    ></v-list-item>
    <v-list-item
      to="/manage-families"
      title="Manage families"
      prepend-icon="mdi-account-group-outline"
    ></v-list-item>
  </v-navigation-drawer>
</template>

<style></style>
