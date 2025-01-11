<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { SubmitEventPromise } from "vuetify";
import NavigationSide from "@/components/Navigation-side.vue";
import { PostDto, PostClient, AuthClient } from "@/api/api";

const route = useRoute();
const router = useRouter();
const routeId = (route.params as { id: string }).id;
const postDto = ref<PostDto>();
const postClient = new PostClient();
const authClient = new AuthClient();
const user = ref();
const imagePreviewUrl = ref<string | null>(null);

const errors = ref({
  textContent: "",
});

onMounted(async () => {
  await getPost();
  await getUser();
});

async function getPost() {
  const post = await postClient.getPostById(routeId);
  postDto.value = post;

  imagePreviewUrl.value = post.imageUrl || null;
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
  if (!validateFields()) {
    return;
  }

  const { valid } = await event;
  if (!valid) return;

  const fileInput = document.querySelector(
    'input[type="file"]'
  ) as HTMLInputElement;
  const file = fileInput.files?.[0];

  let imageUrl = imagePreviewUrl.value;

  if (file) {
    const fileParameter = { data: file, fileName: file.name };
    const response = await postClient.getImageUrl(fileParameter);
    const jsonResponse = await response.data
      .text()
      .then((text) => JSON.parse(text));
    imageUrl = jsonResponse.fileName;
  }

  if (!file && !imageUrl) {
    imageUrl = "";
  }

  const model = new PostDto({
    id: routeId,
    textContent: postDto.value?.textContent ?? "",
    imageUrl: imageUrl ?? "",
    createdAt: new Date(),
    userId: user.value.id,
  });

  await postClient.updatePost(model);
  await router.push("/home");
}

function deleteFile() {
  imagePreviewUrl.value = null;
}

function validateFields() {
  errors.value.textContent = postDto.value?.textContent ? "" : "Caption is required.";

  return !Object.values(errors.value).some((error) => error !== "");
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
            <div v-if="imagePreviewUrl">
              <p>Keep the current file: Do nothing</p>
              <p>OR</p>
              <p>Delete the current file: </p>
              <VBtn class="ms-2" color="error" @click="deleteFile">Click this button</VBtn>
              <p>OR</p>
              <p>Change the current file:</p>
            </div>
            <v-file-input clearable label="Add a file"></v-file-input>
            <VTextarea
              v-model="postDto.textContent"
              label="Caption"
              class="mb-2"
              clearable
            />
            <p v-if="errors.textContent" class="error">{{ errors.textContent }}</p>
          </VCardText>
          <VCardActions>
            <VBtn class="me-4" type="submit"> Save </VBtn>
          </VCardActions>
        </VForm>
      </VCard>
    </VCard>
  </div>
</template>

<style scoped>
.error {
  color: red;
  font-size: 0.9em;
  margin-top: -10px;
  margin-bottom: 10px;
}
</style>