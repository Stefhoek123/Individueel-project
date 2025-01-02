<script setup lang="ts">
import { PostClient, PostDto, UserClient } from "@/api/api";
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
const userClient = new UserClient();
const isLoggedIn = ref(false);

// async function isAuthenticated(): Promise<boolean> {
//   try {
//     const response = await authClient.checkAuthStatus();  // Call the backend API to check session
//     console.log("Response:", response);
//     const responseData = await response.data.text();  // Read the response as text
//     console.log("Response data:", responseData);
//     const data = JSON.parse(responseData);  // Parse the response as JSON
//     console.log("Is authenticated:", data.isAuthenticated);  // Log the authentication status
//     return data.isAuthenticated;  // Access the isAuthenticated property
//   } catch (error) {
//     console.error("Error checking authentication:", error);
//     return false;
//   }
// }

// onMounted(async () => {
//   isLoggedIn.value = await isAuthenticated();
//   if (!isLoggedIn.value) {
//     router.push("/");  // Redirect to login if not authenticated
//   }
// });

async function submit() {
  const fileInput = document.querySelector(
    'input[type="file"]'
  ) as HTMLInputElement;
  const file = fileInput.files?.[0];

//  const userIdResponse = await authClient.getUserIdFromSession();  // Fetch the user ID from the session
 // const userId = await userIdResponse.data.text();  // Extract the user ID from the response

  if (file) {
    const fileParameter = { data: file, fileName: file.name };

    const response = await client.getImageUrl(fileParameter);

    // Parse the Blob into a JSON object
    const jsonResponse = await response.data.text().then((text) => JSON.parse(text));

    const url = jsonResponse.fileName;

    const model = new PostDto({
      textContent: post.value.textContent,
      imageUrl: url,
      userId: "userId",
    });

    await client.createPost(model);
    await router.push("/home");
  } else {
    const model = new PostDto({
      textContent: post.value.textContent,
      imageUrl: " ",
      userId: "userId",
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
