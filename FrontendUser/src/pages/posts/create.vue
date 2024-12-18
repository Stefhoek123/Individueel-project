<script setup lang="ts">
import { PostClient, PostDto } from "@/api/api";
import { ref } from "vue";
import { useRouter } from "vue-router";

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

async function submit() {
  const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
  const file = fileInput.files?.[0];

  console.log(file);

  if (file) {
      const response = await client.getImageUrl(
        file.type, // contentType
        null,      // contentDisposition (optional, use null if not needed)
        null,      // headers (optional, use null if not needed)
        file.size, // length
        file.name, // name
        file.name  // fileName
      );

      console.log(response); // Assuming response is structured
      const url = response.fileName;

      console.log(url);

      const model = new PostDto({
        textContent: post.value.textContent,
        imageUrl: url,
        userId: "10000000-0000-0000-0000-000000000000", // change when you can login
      });

      console.log(model);

      await client.createPost(model);
      await router.push("/");
  } else {
    alert("Please select a file.");
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
