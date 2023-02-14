<script setup lang="ts">
import { RouterView } from "vue-router";
import TheHeader from "@/components/layout/TheHeader.vue";
</script>

<template>
  <div class="layout">
    <div class="layout__header">
      <TheHeader />
    </div>

    <div class="layout__content">
      <RouterView />
    </div>
  </div>
</template>

<style scoped lang="scss">
.layout {
  height: 100%;
  display: flex;
  flex-direction: column;

  &__content {
    margin: var(--default-margin-gap);
  }
}
</style>
