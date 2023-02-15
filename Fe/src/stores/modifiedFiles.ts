import { defineStore } from "pinia";
import { computed, ref } from "vue";
import type { ModifiedFile } from "@/interfaces/ModifiedFile";
import { modifiedFilesApi } from "@/services/api/modifiedFilesApi";
import { modifiedFileType } from "@/enum/modifiedFileType";

export const useModifiedFilesStore = defineStore("modifiedFiles", () => {
  const allModifiedFiles = ref<Array<ModifiedFile>>([]);
  const error = ref<string | null>(null);
  const isLoading = ref<boolean>(false);

  const addedFiles = computed(() =>
    allModifiedFiles.value.filter((f) => f.type === modifiedFileType.added)
  );
  const editedFiles = computed(() =>
    allModifiedFiles.value.filter((f) => f.type === modifiedFileType.edited)
  );
  const deletedFiles = computed(() =>
    allModifiedFiles.value.filter((f) => f.type === modifiedFileType.deleted)
  );

  function clearState() {
    allModifiedFiles.value = [];
    error.value = null;
  }

  async function getModifiedFiles(path: string) {
    isLoading.value = true;
    clearState();

    try {
      const result = await modifiedFilesApi.getModifiedFiles(path);
      allModifiedFiles.value = result;
    } catch (e) {
      console.error(e);

      if (e instanceof Error) {
        error.value = e.message;
      } else {
        error.value = "Oops something went wrong";
      }
    } finally {
      isLoading.value = false;
    }
  }

  return {
    allModifiedFiles,
    error,
    isLoading,

    addedFiles,
    editedFiles,
    deletedFiles,

    clearState,
    getModifiedFiles,
  };
});
