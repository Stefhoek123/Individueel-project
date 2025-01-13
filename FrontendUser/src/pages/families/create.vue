<script setup lang="ts">
import { FamilyClient, FamilyDto, AuthClient, UserClient, UserDto } from "@/api/api";
import { useRouter } from "vue-router";
import { ref } from "vue";
import NavigationSide from "@/components/Navigation-side.vue";
import { onMounted } from "vue";

const router = useRouter();
const familyClient = new FamilyClient();
const authClient = new AuthClient();
const userClient = new UserClient();	
const user = ref();
const users = ref<UserDto[]>([]);

interface Family {
  familyName: string;
}

const family = ref<Family>({
  familyName: "",
});

const errors = ref({
  familyName: "",
});

onMounted(() => {
  getUser();
});

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
    family.value = await familyClient.getFamilyById(slicedUser.familyId);
    users.value = await userClient.getUsersByFamilyId(slicedUser.familyId);
  }
}

async function familyNameExists(familyName: string) {
  const families = await familyClient.getAllFamilies();
  return families.some(family => family.familyName === familyName);
}

async function submit() {
  if (!validateFields()) {
    return;
  }

  if (await familyNameExists(family.value.familyName)) {
    errors.value.familyName = "Family name already exist.";
    return;
  }

  const model = new FamilyDto({
    familyName: family.value.familyName,
  });

  await familyClient.createFamily(model);
 const familyId =  await familyClient.getFamilyIdByName(model.familyName);

    const modelUser = new UserDto({
      id: user.value.id,
      firstName: user.value?.firstName || "",
      lastName: user.value?.lastName || "",
      email: user.value?.email || "",
      passwordHash: user.value?.passwordHash || "",
      familyId: familyId.id,
      isActive: user.value.isActive,
    });

  await userClient.updateUser(modelUser);
  await router.push("/families/:id");
}

function validateFields() {
  errors.value.familyName = family.value.familyName ? "" : "Family Name is required.";

  return !Object.values(errors.value).some((error) => error !== "");
}
</script>

<template>
  <div>
    <NavigationSide />
  <VCard title="Create a family" class="vcard">
    <VForm validate-on="blur" @submit.prevent="submit">
      <VCardText>
        <VTextField
          v-model="family.familyName"
          label="Family Name"
          class="mb-2"
        />
        <p v-if="errors.familyName" class="error">{{ errors.familyName }}</p>
      </VCardText>
      <VCardActions>
        <VBtn class="card" type="submit"> Submit </VBtn>
      </VCardActions>
    </VForm>
  </VCard>
</div>
</template>

<style scoped>
.vcard {
  margin-top: 170px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 70px;
  width: 70%;
}

.error {
  color: red;
  font-size: 0.9em;
  margin-top: -10px;
  margin-bottom: 10px;
}

.card {
  background-color: #1F7087;
  color: white;
  border-radius: 10px;
  box-shadow: 0 4px 6px 0 rgba(0, 0, 0, 0.1);
  transition: 0.3s;
}
</style>
