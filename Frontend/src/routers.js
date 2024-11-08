import HomePage from "./pages/HomePage.vue";
import SignUpPage from "./pages/SignUpPage.vue";
import LoginPage from "./pages/LoginPage.vue";
import AddPage from "./pages/AddPage.vue";
import UpdatePage from "./pages/UpdatePage.vue";
import UserPage from "./pages/UserPage.vue";
import ChatPage from "./pages/ChatPage.vue";
import testPage from "./components/testPage.vue";
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
  {
    name: "testPage",
    component: testPage,
    path: "/test",
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
