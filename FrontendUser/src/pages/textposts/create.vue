<script setup lang="ts">
import { TextPostClient, TextPostDto } from '@/api/api'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

interface TextPost {
 textContent: string
}

const textPost = ref<TextPost>({
  textContent: '',
})

const client = new TextPostClient()

async function submit() {

    const model = new TextPostDto({
      textContent: textPost.value.textContent,
      userId: "10000000-0000-0000-0000-000000000000", // change when you can login
    })

    await client.createTextPost(model)

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
          v-model="textPost.textContent"
          label="TextContent"
          :rules="[required('TextContent')]"
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