<script setup lang="ts">
import { PostClient, PostDto, AuthClient } from "@/api/api";
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import NavigationSide from "@/components/Navigation-side.vue";
import HeaderComponent from "@/components/HeaderComponent.vue";

const router = useRouter();
const postClient = new PostClient();
const authClient = new AuthClient();
const user = ref();

interface Post {
  textContent: string;
  imageUrl: string;
  createdAt?: Date;
}

const post = ref<Post>({
  textContent: "",
  imageUrl: "",
  createdAt: new Date(),
});

onMounted(() => {
  getUser();
});


async function getUser() {
  const token = sessionStorage.getItem("JWT");
  if (token) {
    const currentUser = await authClient.getCurrentUser(token);

    const userData = JSON.parse(await currentUser.data.text());

    const slicedUser = {
      id: userData.id,
      firstName: userData.firstName,
      lastName: userData.lastName,
      email: userData.email,
      passwordHash: userData.passwordHash,
      isActive: userData.isActive,
      familyId: userData.familyId,
    };

    user.value = slicedUser;
  }
}

async function submit() {
  const fileInput = document.querySelector(
    'input[type="file"]'
  ) as HTMLInputElement;
  const file = fileInput.files?.[0];

  if (file) {
    const fileParameter = { data: file, fileName: file.name };
    const response = await postClient.getImageUrl(fileParameter);
    const jsonResponse = await response.data.text().then((text) => JSON.parse(text));
    const url = jsonResponse.fileName;

    const model = new PostDto({
      textContent: post.value.textContent,
      imageUrl: url,
      createdAt: new Date(),
      userId: user.value.id,
    });

    await postClient.createPost(model);
    await router.push("/home");
  } else {
    const model = new PostDto({
      textContent: post.value.textContent,
      imageUrl: " ",
      createdAt: new Date(),
      userId: user.value.id,
    });

    await postClient.createPost(model);
    await router.push("/home");
  }
}

function required(fieldName: string): (v: string) => true | string {
  return (v) => !!v || `${fieldName} is required`;
}
</script>

<template>
  <div>
    <NavigationSide />
    <HeaderComponent />
  <VCard title="Create a new post" class="vcard">
    <VForm validate-on="blur" @submit.prevent="submit">
      <VCardText>
        <v-file-input clearable label="File input"></v-file-input>
        <VTextarea
          v-model="post.textContent"
          label="Caption"
          :rules="[required('Caption')]"
          class="mb-2"
        />
      </VCardText>
      <VCardActions>
        <VBtn class="me-4" type="submit"> submit </VBtn>
      </VCardActions>
    </VForm>
  </VCard>
</div>
</template>

<style scoped>
.vcard {
  margin-top: 170px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 70px;
  width: 70%;
}
</style>
