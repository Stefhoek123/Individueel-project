<script setup lang="ts">
import { useRouter } from 'vue-router';

const router = useRouter();

async function home() {
    await router.push('/')
  }
</script>

<template>
    <nav class="computer-nav">

      </nav>
  </template>
    
  <style>
  .computer-nav {
    background: #212121;
    position: fixed;
    top: 0;
    height: 65px;
    width: 100%;
    display: flex;
    justify-content: space-around;
    padding-top: 5px;
  }

  .logo-icon {
    width: 35px;
    height: 35px;
  }
  
  </style>
  