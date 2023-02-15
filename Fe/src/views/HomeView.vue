<script setup lang="ts">
import { useModifiedFilesStore } from "@/stores/modifiedFiles";
import { ref } from "vue";
import ScanFolderForm from "@/components/ScanFolderForm.vue";
import ModifiedFileList from "@/components/ModifiedFileList.vue";
import DisplayMessage from "@/components/DisplayMessage.vue";
import IntroductionText from "@/components/IntroductionText.vue";
import SimpleLayout from "@/components/layout/SimpleLayout.vue";
import LoadingBackdrop from "@/components/ui/LoadingBackdrop.vue";

const modifiedFilesStore = useModifiedFilesStore();

let lastPath: string;
let displayNewFolder = ref(false);

const handleSubmit = async (path: string) => {
  await modifiedFilesStore.getModifiedFiles(path);
  displayNewFolder.value = path !== lastPath;
  lastPath = path;
};
</script>

<template>
  <SimpleLayout title="Folder Scanner">
    <IntroductionText />

    <div class="home__scan_form">
      <ScanFolderForm @submit="handleSubmit" />
    </div>

    <LoadingBackdrop v-if="modifiedFilesStore.isLoading" />

    <div v-else>
      <DisplayMessage
        data-test="home_error_message"
        v-if="modifiedFilesStore.error"
        :value="modifiedFilesStore.error"
        isError
      />

      <template v-else>
        <ModifiedFileList v-if="lastPath && !displayNewFolder" />
        <DisplayMessage v-if="displayNewFolder" value="New folder" />
      </template>
    </div>
  </SimpleLayout>
</template>

<style scoped lang="scss">
.home {
  &__scan_form {
    margin: var(--default-margin-gap) 0;
  }
}
</style>
