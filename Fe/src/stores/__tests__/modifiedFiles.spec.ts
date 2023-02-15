import { beforeEach, describe, expect, it, vi } from "vitest";
import { createPinia, setActivePinia } from "pinia";
import { useModifiedFilesStore } from "../modifiedFiles";
import type { ModifiedFile } from "../../interfaces/ModifiedFile";
import { modifiedFileType } from "../../enum/modifiedFileType";

describe("Modified Files Store", () => {
  beforeEach(() => {
    setActivePinia(createPinia());
  });

  it("ClearState resets allModifiedFiles", () => {
    const store = useModifiedFilesStore();
    const modifiedFiles: Array<ModifiedFile> = [
      { fullName: "a", version: 1, type: modifiedFileType.deleted },
    ];
    store.allModifiedFiles = modifiedFiles;

    expect(store.allModifiedFiles).to.not.be.empty;
    store.clearState();

    expect(store.allModifiedFiles).to.be.empty;
  });

  it("ClearState resets error", () => {
    const store = useModifiedFilesStore();
    store.error = "oops";

    expect(store.error).to.not.be.empty;
    store.clearState();

    expect(store.error).to.be.null;
  });
});
