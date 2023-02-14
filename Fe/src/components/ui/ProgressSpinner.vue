<template>
  <div class="progress-spinner" />
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
.progress-spinner {
  width: 50px;
  height: 50px;
  border: 3px solid rgba(255, 255, 255, 0.3);
  border-radius: 50%;
  border-top-color: #fff;
  animation: spin 1s ease-in-out infinite;
}
</style>
