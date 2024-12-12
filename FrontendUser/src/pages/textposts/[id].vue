<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import { TextPostDto, UserDto } from "@/api/api";
import ConfirmDialogue from "@/components/ConfirmDialogue.vue";
import { TextPostClient, UserClient } from "@/api/api";

const client = new TextPostClient();
const userClient = new UserClient();
const textPost = ref<TextPostDto | null>(null);
const users = ref<UserDto[]>([]);

const confirmDialogueRef = ref<InstanceType<typeof ConfirmDialogue> | null>(
  null
);

const router = useRouter();
const route = useRoute();
const routeId = (route.params as { id: string }).id;

onMounted(() => {
  getTextPostsById();
});

async function getTextPostsById() {
  textPost.value = await client.getTextPostById(routeId);
  users.value = await userClient.getUsersByFamilyId(routeId);
  console.log("Family:", textPost.value);
  console.log("Users:", users.value);
}

async function confirmAndDelete(id: string) {
  const confirmed = await confirmDialogueRef.value?.show({
    title: "Delete textpost",
    message:
      "Are you sure you want to delete this textpost? It cannot be undone.",
    okButton: "Delete Forever",
    cancelButton: "Cancel",
  });

  if (confirmed) {
    await deleteTextPostById(id);
  }
}

async function deleteTextPostById(id: string) {
  await client.deleteTextPostById(id);
  await router.push("/");
}
</script>

<template>
  <div>
    <ConfirmDialogue ref="confirmDialogueRef" />
    <div>
      <v-container class="text-center" v-if="textPost">
        <VCardTitle class="title-achievement">
          {{ textPost.userId }}
        </VCardTitle>
        <v-row justify="center" class="my-5">
          <v-col cols="12" sm="8">
            <v-card elevation="2" class="pa-5">
              <v-row justify="center">
                <v-col cols="2">
                  <router-link :to="`/`">
                    <VBtn
                      icon="mdi-arrow-left"
                      variant="plain"
                      color="accent"
                      size="small"
                    />
                  </router-link>
                </v-col>
                <v-col cols="6">
                  <p class="headline">&nbsp;</p>
                </v-col>
                <v-col cols="2">
                  <router-link :to="`/textposts/update/${routeId}`">
                    <VBtn
                      icon="mdi-pen"
                      variant="plain"
                      color="accent"
                      size="small"
                    />
                  </router-link>
                </v-col>
                <v-col cols="2">
                  <VBtn
                    icon="mdi-delete"
                    variant="plain"
                    color="accent"
                    size="small"
                    @click="confirmAndDelete(routeId)"
                  />
                </v-col>
              </v-row>
              <v-row justify="center">
                <v-img
                  src="https://via.placeholder.com/400x300.png?text=Santa+and+Child"
                  aspect-ratio="4/3"
                  class="mb-4"
                ></v-img>
              </v-row>
              <v-row justify="center">
                <v-col cols="12">
                  <p class="caption font-italic">
                    "While waiting in line to see Santa, this baby fell asleep.
                    When it came time for the picture, Santa told the parents
                    not to wake him."
                  </p>
                </v-col>
              </v-row>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </div>
  </div>
</template>

<style scoped>
.caption {
  color: #555;
}
</style>
