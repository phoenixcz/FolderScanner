<script setup lang="ts">
import { useModifiedFilesStore } from "@/stores/modifiedFiles";
import ModifiedFilesSection from "@/components/modifiedFilesList/ModifiedFilesSection.vue";
import DisplayMessage from "@/components/DisplayMessage.vue";

const modifiedFilesStore = useModifiedFilesStore();
</script>

<template>
  <modified-files-section
    title="Added files"
    :items="modifiedFilesStore.addedFiles"
  />
  <modified-files-section
    title="Edited files"
    display-version
    :items="modifiedFilesStore.editedFiles"
  />
  <modified-files-section
    title="Deleted files"
    :items="modifiedFilesStore.deletedFiles"
  />
  <DisplayMessage
    v-if="modifiedFilesStore.allModifiedFiles.length === 0"
    value="No change"
  />
</template>

<style scoped lang="scss"></style>
