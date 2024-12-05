<script lang="ts" setup>
import NavigationSide from './components/Navigation-side.vue';
import HeaderComponent from './components/HeaderComponent.vue';
</script>

<template>
  <v-app>
    <v-main>
      <HeaderComponent />
      <NavigationSide />
      <router-view />

    </v-main>
  </v-app>
</template>


