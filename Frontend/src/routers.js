import HomePage from "./components/HomePage.vue";
import SignUpPage from "./components/SignUpPage.vue";
import LoginPage from "./components/LoginPage.vue";
import AddPage from "./components/AddPage.vue";
import UpdatePage from "./components/UpdatePage.vue";
import UserPage from "./components/UserPage.vue";
import ChatPage from "./components/ChatPage.vue";
import { createRouter, createWebHistory } from "vue-router";

const routes = [
  {
    name: "HomePage",
    component: HomePage,
    path: "/",
  },
  {
    name: "SignUpPage",
    component: SignUpPage,
    path: "/sign-up",
  },
  {
    name: "LoginPage",
    component: LoginPage,
    path: "/login",
  },
  {
    name: "AddPage",
    component: AddPage,
    path: "/add",
  },
  {
    name: "UpdatePage",
    component: UpdatePage,
    path: "/update/:id",
  },
  {
    name: "UserPage",
    component: UserPage,
    path: "/user",
  },
  {
    name: "ChatPage",
    component: ChatPage,
    path: "/chat",
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
