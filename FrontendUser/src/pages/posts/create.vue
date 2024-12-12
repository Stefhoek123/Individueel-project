<script setup lang="ts">
import { PostClient, PostDto } from '@/api/api'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

interface Post {
 textContent: string
 imageUrl: string
}

const post = ref<Post>({
  textContent: '',
  imageUrl: '',
})

const client = new PostClient()

async function submit() {

    const model = new PostDto({
      textContent: post.value.textContent,
      imageUrl: post.value.imageUrl,
      userId: "10000000-0000-0000-0000-000000000000", // change when you can login
    })

    await client.createPost(model)
    await router.push('/')
  }

function required(fieldName: string): (v: string) => true | string {
  return v => !!v || `${fieldName} is required`
}
</script>

<template>
  <VCard title="Create new text post" class="vcard">
    <VForm
      validate-on="blur"
      @submit.prevent="submit"
    >
      <VCardText>
        <VTextarea
          v-model="post.textContent"
          label="TextContent"
          :rules="[required('TextContent')]"
          class="mb-2"
        />
        <!--aanpassen naar iets te kiezen-->
        <VTextarea
          v-model="post.imageUrl"
          label="ImageUrl"
          :rules="[required('ImageUrl')]"
          class="mb-2"
        />
      </VCardText>
      <VCardActions>
        <VBtn
          class="me-4"
          type="submit"
        >
          submit
        </VBtn>
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