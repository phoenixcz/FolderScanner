<script setup lang="ts">
import { ref } from "vue";

const emit = defineEmits<{
  (e: "submit", path: string): void;
}>();

const path = ref<string>("C:\\scan");

const handleSubmit = async () => {
  emit("submit", path.value);
};
</script>

<template>
  <form class="scan-folder-form" @submit.prevent="handleSubmit()">
    <div class="scan-folder-form__input-group">
      <input
        class="scan-folder-form__input"
        type="text"
        placeholder="Enter path to the folder..."
        v-model="path"
        required
        data-test="scan-folder_input"
      />

      <button
        class="scan-folder-form__button"
        type="submit"
        data-test="scan-folder_submit"
      >
        Submit
      </button>
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
  }

  &__button {
    font: inherit;
    cursor: pointer;
    padding: 0 1rem;
    border: 1px solid darkgreen;
    background-color: hsla(160, 100%, 37%, 1);
    color: white;
    border-radius: 12px;
    margin-left: 1rem;
    height: 2.5rem;

    &:hover,
    :active {
      background-color: hsla(160, 100%, 37%, 0.9);
    }
  }

  &__input {
    font: inherit;
    padding: 0.5rem;
    border-radius: 6px;
    border: 1px solid #ccc;
    width: 100%;
    height: 2.5rem;
    margin-bottom: 1rem;
  }
}
</style>
