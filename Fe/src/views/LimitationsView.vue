<script setup lang="ts">
import SimpleLayout from "@/components/layout/SimpleLayout.vue";
</script>

<template>
  <SimpleLayout title="Limitations">
    <ul>
      <li>Doesn't persist state between application executions</li>
      <li>
        Performance might not be optimal for large files or folders with many
        files.
      </li>
      <li>UI/UX is very basic</li>
      <li>FE contains only example of tests. Not fully covered.</li>
    </ul>
  </SimpleLayout>
</template>

<style scoped lang="scss"></style>
