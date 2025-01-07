<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import type { SubmitEventPromise } from "vuetify/lib/framework.mjs";
import NavigationSide from "@/components/Navigation-side.vue";
import { PostDto, PostClient, AuthClient } from "@/api/api";

const route = useRoute();
const router = useRouter();
const routeId = (route.params as { id: string }).id;
const postDto = ref<PostDto>();
const postClient = new PostClient();
const authClient = new AuthClient();
const user = ref();

onMounted(async () => {
  await getPost();
  await getUser();
});

async function getPost() {
  postDto.value = await postClient.getPostById(routeId);
}

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

async function submit(event: SubmitEventPromise) {
  const { valid } = await event;

  const fileInput = document.querySelector(
    'input[type="file"]'
  ) as HTMLInputElement;
  const file = fileInput.files?.[0];

    if (!valid) {
        return;
    }

  if (file) {
    const fileParameter = { data: file, fileName: file.name };
    const response = await postClient.getImageUrl(fileParameter);
    const jsonResponse = await response.data.text().then((text) => JSON.parse(text));
    const url = jsonResponse.fileName;

    const model = new PostDto({
        id: routeId,
        textContent: postDto.value?.textContent ?? '',
        imageUrl: url,
        userId: user.value.id,
    });

    await postClient.updatePost(model);
    await router.push("/home");
  } else {
    const model = new PostDto({
      id: routeId,
      textContent:  postDto.value?.textContent ?? '',
      imageUrl: " ",
      userId: user.value.id,
    });

    await postClient.updatePost(model);
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
    <VCard v-if="postDto" title="Edit post">
      <VCard title="Create a new post" class="vcard">
        <VForm validate-on="blur" @submit.prevent="submit">
          <VCardText>
            <v-file-input clearable label="File input"></v-file-input>
            <VTextarea
              v-model="postDto.textContent"
              label="Caption"
              :rules="[required('Caption')]"
              class="mb-2"
              clearable
            />
          </VCardText>
          <VCardActions>
            <VBtn class="me-4" type="submit"> Save </VBtn>
          </VCardActions>
        </VForm>
      </VCard>
    </VCard>
  </div>
</template>
