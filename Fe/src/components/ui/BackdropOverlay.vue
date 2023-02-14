<script setup lang="ts"></script>

<template>
  <div class="backdrop-overlay__backdrop">
    <div class="backdrop-overlay__content_wrapper">
      <slot />
    </div>
  </div>
</template>

<style scoped lang="scss">
.backdrop-overlay {
  &__backdrop {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    z-index: 10;
    background: rgba(0, 0, 0, 0.75);

    display: flex;
    justify-content: center;
    align-items: center;

    transition: background 0.4s ease-in-out;
    animation: fadeIn 0.4s ease-in-out;
  }

  &__content_wrapper {
    z-index: 11;
  }
}
</style>
