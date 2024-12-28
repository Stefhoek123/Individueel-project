<script setup lang="ts">
import { PostClient, PostDto } from "@/api/api";
import { ref } from "vue";
import { useRouter } from "vue-router";
import { onMounted } from "vue";

const router = useRouter();

interface Post {
  textContent: string;
  imageUrl: string;
}

const post = ref<Post>({
  textContent: "",
  imageUrl: "",
});

const client = new PostClient();

const isLoggedIn = ref(false);

function isAuthenticated() {
  return document.cookie.includes('.AspNetCore.Cookies');
}

console.log("Is authenticated:", isAuthenticated());

onMounted(() => {
  isLoggedIn.value = isAuthenticated();
  if (isLoggedIn.value === false) {
    router.push("/");
  }
});


async function submit() {
  const fileInput = document.querySelector(
    'input[type="file"]'
  ) as HTMLInputElement;
  const file = fileInput.files?.[0];

  if (file) {
    const fileParameter = { data: file, fileName: file.name };

    const response = await client.getImageUrl(fileParameter);

    // Parse the Blob into a JSON object
    const jsonResponse = await response.data
      .text()
      .then((text) => JSON.parse(text));

    const url = jsonResponse.fileName;

    const model = new PostDto({
      textContent: post.value.textContent,
      imageUrl: url,
      userId: "10000000-0000-0000-0000-000000000000", // change when you can login
    });

    await client.createPost(model);
    await router.push("/home");
  } else {
    const model = new PostDto({
      textContent: post.value.textContent,
      imageUrl: " ",
      userId: "10000000-0000-0000-0000-000000000000", // change when you can login
    });

    await client.createPost(model);
    await router.push("/home");
  }
}

function required(fieldName: string): (v: string) => true | string {
  return (v) => !!v || `${fieldName} is required`;
}
</script>

<template>
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
