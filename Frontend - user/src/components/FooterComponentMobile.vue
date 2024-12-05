<script setup lang="ts">
import { useRouter } from 'vue-router';

const router = useRouter();

async function home() {
    await router.push('/')
  }
  
  async function add() {
    await router.push('/add')
  }

  async function chat() {
    await router.push('/chat')
  }

  async function user() {
    await router.push('/users/:id')
  }
</script>


<template>
    <nav class="mobile-nav-footer">
      <VForm
      validate-on="blur"
      @submit.prevent="home"
    >
      <VCardActions>
        <VBtn
          class="bloc-icon"
          type="home"
        >
        <img src="../assets/home.png" alt="">
        </VBtn>
      </VCardActions>
    </VForm>
    <VForm
      validate-on="blur"
      @submit.prevent="add"
    >
      <VCardActions>
        <VBtn
          class="bloc-icon"
          type="add"
        >
        <img src="../assets/add.png" alt="">
        </VBtn>
      </VCardActions>
    </VForm>
    <VForm
      validate-on="blur"
      @submit.prevent="chat"
    >
      <VCardActions>
        <VBtn
          class="bloc-icon"
          type="chat"
        >
        <img src="../assets/chat.png" alt="">
        </VBtn>
      </VCardActions>
    </VForm>
    <VForm
      validate-on="blur"
      @submit.prevent="user"
    >
      <VCardActions>
        <VBtn
          class="bloc-icon"
          type="user"
        >
        <img src="../assets/user.png" alt="">
        </VBtn>
      </VCardActions>
    </VForm>
      </nav>
  </template>

  <style>
  .mobile-nav-footer {
    background: #F1F1F1;
    position: fixed;
    bottom: 0;
    height: 65px;
    width: 100%;
    display: flex;
    justify-content: space-around;
    padding-top: 5px;
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
  