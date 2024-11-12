<script setup lang="ts">
import { useTheme } from 'vuetify'

</script>

<template>
  <VLocaleProvider>
      <VApp>
      <RouterView />
    </VApp>
  </VLocaleProvider>
</template>


<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

h1 {
  padding-top: 70px;
}

body{
  padding: 0;
  margin: 0;
}

.logo {
  width: 100px;
}

.register input, .login input, .add input, .update input{
  width: 300px;
  height: 40px;
  padding-left: 20px;
  display: block;
  margin-bottom: 30px;
  margin-right: auto;
  margin-left: auto;
  border: 1px solid orange;
  border-radius: 15px;
}

.register button, .login button, .add button, .update button {
  width: 320px;
  height: 40px;
  border: 1px solid orange;
  background-color: orange;
  color: #fff;
  cursor: pointer;
  border-radius: 15px;
}
</style>
