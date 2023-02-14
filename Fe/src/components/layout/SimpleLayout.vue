<script setup lang="ts">
defineProps({
  title: {
    type: String,
    required: false,
  },
});
</script>

<template>
  <div class="simple-layout">
    <h1 class="green simple-layout__header">{{ $props.title }}</h1>
    <div class="simple-layout__content">
      <slot />
    </div>
  </div>
</template>

<style scoped lang="scss">
.simple-layout {
  display: flex;
  flex-direction: column;
  align-items: center;

  &__header {
    margin-bottom: var(--default-margin-gap);
  }

  &__content {
    max-width: 800px;
  }
}
</style>
