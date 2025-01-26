<script setup lang="ts">
import { ref } from "vue";
import BaseButton from "@/components/ui/BaseButton.vue";
import BaseInput from "@/components/ui/BaseInput.vue";

const emit = defineEmits<{
  (e: "submit", path: string): void;
}>();

const path = ref<string>(import.meta.env.VITE_DEFAULT_FOLDER_PATH);

const handleSubmit = async () => {
  emit("submit", path.value);
};
</script>

<template>
  <form class="scan-folder-form" @submit.prevent="handleSubmit()">
    <div class="scan-folder-form__input-group">
      <BaseInput
        type="text"
        placeholder="Enter path to the folder..."
        v-model="path"
        required
        data-test="scan-folder_input"
      />

      <BaseButton
        label="Submit"
        type="submit"
        class="scan-folder-form__button"
        data-test="scan-folder_submit"
      />
    </div>
  </form>
</template>

<style scoped lang="scss">
.scan-folder-form {
  width: 100%;

  &__input-group {
    display: flex;
    flex-direction: row;
    align-items: baseline;
    gap: 1rem;
  }
}
</style>
