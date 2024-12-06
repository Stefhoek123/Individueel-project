<script setup lang="ts">
import { UserClient } from '@/api/api'

const user = defineProps<{ id: string }>()
const userClient = new UserClient()
const linkAmount = ref(0)

onMounted(() => getLinks())
async function getLinks() {
  links.value = await linkClient.getAllLinks()
  links.value?.forEach(async link => {
    [link.isClicked] = await Promise.all([linkClient.getIfUserLinkClicked(user.id, link.id)])
    linkAmount.value++
    if (link.isClicked)
      completedLinkAmount.value++
  })
}
</script>

<template>
  <VCard
    class="card"
    title="Links"
    :subtitle="`${completedLinkAmount.toString()}/${linkAmount.toString()}`"
  >
    <VCardText>
      <VTable>
        <thead>
          <tr>
            <th class="text-left">
              Name
            </th>
            <th class="text-left">
              Description
            </th>
            <th class="text-left">
              Opened
            </th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="item in links"
            :key="item.id"
          >
            <td>{{ item.title }}</td>
            <td>{{ item.description }}</td>
            <td>
              <VCheckbox
                :model-value="item.isClicked"
                @click.prevent
                disabled
              />
            </td>
          </tr>
        </tbody>
      </VTable>
    </VCardText>
  </VCard>
</template>
