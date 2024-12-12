<script lang="ts">
import { ref, onMounted } from "vue";
import type { FamilyDto } from "@/api/api";
import { FamilyClient } from "@/api/api";

// Register components
const familyMembers = ref<FamilyDto[]>([]);
const client = new FamilyClient();
const props = defineProps<{ id: string }>();

onMounted(() => {
  loadData();
});

async function loadData() {
  try {
    const familyMembersData = await client.getFamilyById(props.id);
    if (Array.isArray(familyMembersData)) {
      familyMembers.value = familyMembersData;
    } else {
      familyMembers.value = [familyMembersData]; // Wrap it in an array
    }
  } catch (error) {
    console.error("Failed to load family members:", error);
  }
}
</script>

<template>
  <div>
    <h1>My Family</h1>
    <ul>
      <li v-for="member in family" :key="member.id">{{ member.name }}</li>
    </ul>
  </div>
</template>

<style scoped>
h1 {
  color: #333;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  margin: 5px 0;
}
</style>
