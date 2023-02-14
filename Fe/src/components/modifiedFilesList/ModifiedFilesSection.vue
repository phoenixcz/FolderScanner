<script setup lang="ts">
import type { PropType } from "vue";
import type { ModifiedFile } from "@/interfaces/ModifiedFile";

defineProps({
  title: {
    type: String,
    required: true,
  },
  items: {
    type: Array as PropType<Array<ModifiedFile>>,
    required: true,
  },
  displayVersion: {
    type: Boolean,
    default: false,
  },
});
</script>

<template>
  <div v-if="items.length > 0">
    <h2>{{ title }}</h2>
    <ul>
      <li v-for="file in items" :key="file.fullName">
        {{ file.fullName }}
        {{ displayVersion ? ` (in version ${file.version})` : "" }}
      </li>
    </ul>
  </div>
</template>

<style scoped lang="scss"></style>
