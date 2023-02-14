<script setup lang="ts">
const props = defineProps({
  value: {
    type: String,
    required: true,
  },
  isError: {
    type: Boolean,
    default: false,
  },
});
</script>

<template>
  <div
    :class="{ 'display-message__error': isError }"
    data-test="display-message"
  >
    {{ props.value }}
  </div>
</template>

<style scoped lang="scss">
.display-message {
  &__error {
    color: red;
  }
}
</style>
