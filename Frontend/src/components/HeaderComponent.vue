<script setup lang="ts">
import { useRouter } from 'vue-router';

const router = useRouter();

async function home() {
    await router.push('/')
  }
</script>

<template>
    <nav class="mobile-nav">
      <VForm
      validate-on="blur"
      @submit.prevent="home"
    >
      <VCardActions>
        <VBtn
          class="bloc-icon"
          type="home"
        >
        <img src="../assets/resto-logo.png" alt="">
        </VBtn>
      </VCardActions>
    </VForm>
      </nav>
  </template>
    
  <style>
  .mobile-nav {
    background: #F1F1F1;
    position: fixed;
    top: 0;
    height: 65px;
    width: 100%;
    display: flex;
    justify-content: space-around;
    padding-top: 5px;
  }
  
  @media screen and (min-width: 600px) {
    .mobile-nav {
    display: none;
    }
  }
  </style>
  