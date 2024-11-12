<template>
  <nav class="mobile-nav-footer">
    <router-link to="/" class="bloc-icon">
            <img src="../assets/home.png" alt="">
          </router-link>
          <!-- <a href="#" class="bloc-icon">
            <img src="../assets/loupe.png" alt="">
        </a> -->
          <router-link to="/add" class="bloc-icon">
            <img src="../assets/add.png" alt="">
        </router-link>
        <router-link to="/chat" class="bloc-icon">
            <img src="../assets/chat.png" alt="">
        </router-link>
        <router-link to="/user" class="bloc-icon">
            <img src="../assets/user.png" alt="">
        </router-link>
    </nav>
</template>

<script>
export default {
  name: "FooterComponent",
};
</script>

<style>
.mobile-nav-footer {
  background: #F1F1F1;
  position: fixed;
  bottom: 0;
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
