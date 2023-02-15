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
    data-test="modified-file-list_added-files"
  />
  <modified-files-section
    title="Edited files"
    display-version
    :items="modifiedFilesStore.editedFiles"
    data-test="modified-file-list_edited-files"
  />
  <modified-files-section
    title="Deleted files"
    :items="modifiedFilesStore.deletedFiles"
    data-test="modified-file-list_deleted-files"
  />
  <DisplayMessage
    v-if="modifiedFilesStore.allModifiedFiles.length === 0"
    value="No change"
  />
</template>

<style scoped lang="scss"></style>
