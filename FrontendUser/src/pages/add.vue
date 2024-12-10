<template>
    <div class="outline">
      <h1 class="d-flex justify-center">Do you want to post a text or image?</h1>
    <div class="d-flex justify-center">
      <v-row justify="center">
        <v-col cols="12" md="2">
          <v-card color="#1F7087" class="card">
            <div class="d-flex flex-no-wrap justify-space-between">
              <a href="/textposts/create">Text</a>
            </div>
          </v-card>
        </v-col>
        <v-col cols="12" md="2">
          <v-card color="#1F7087" class="card">
            <div class="d-flex flex-no-wrap justify-space-between">
              <a href="/imageposts/create">Image</a> 
            </div>
          </v-card>
        </v-col>
      </v-row>
    </div>
  </div>
  </template>
  
  <style scoped>
  .outline{
    margin-top: 70px;
  }
  
  .card {
    margin: 20px auto;
    padding: 20px;
    background-color: #1F7087;
    color: white;
    border-radius: 10px;
    box-shadow: 0 4px 6px 0 rgba(0, 0, 0, 0.1);
    transition: 0.3s;
  }
  
  .card:hover {
    background-color: #155a6b;
    transform: translateY(-5px);
  }
  
  a {
    color: white;
    text-decoration: none;
  }
  
  </style>