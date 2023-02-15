import ModifiedFileList from "../../src/components/ModifiedFileList.vue";
import { setActivePinia } from "pinia";
import { createTestingPinia } from "@pinia/testing";
import { useModifiedFilesStore } from "../../src/stores/modifiedFiles";
import type { ModifiedFile } from "../../src/interfaces/ModifiedFile";
import { modifiedFileType } from "../../src/enum/modifiedFileType";

describe("<ModifiedFileList />", () => {
  beforeEach(() => {
    setActivePinia(createTestingPinia({ createSpy: cy.spy }));
    cy.mount(ModifiedFileList);
  });

  it("No modified files displays no change message", () => {
    cy.get("[data-test=display-message]").should("contain", "No change");
  });

  describe("Modified files", () => {
    const deletedFile: ModifiedFile = {
      fullName: "deleted",
      version: 1,
      type: modifiedFileType.deleted,
    };

    const addedFile: ModifiedFile = {
      fullName: "added",
      version: 1,
      type: modifiedFileType.added,
    };

    const editedFiles: Array<ModifiedFile> = [
      {
        fullName: "edited1",
        version: 2,
        type: modifiedFileType.edited,
      },
      {
        fullName: "edited2",
        version: 3,
        type: modifiedFileType.edited,
      },
    ];

    beforeEach(() => {
      const store = useModifiedFilesStore();
      const modifiedFiles: Array<ModifiedFile> = [
        deletedFile,
        addedFile,
        ...editedFiles,
      ];
      store.allModifiedFiles = modifiedFiles;
    });

    it("Added files displayed", () => {
      cy.get("[data-test=modified-file-list_added-files]").should(
        "contain",
        addedFile.fullName
      );
    });

    it("Edited files displayed", () => {
      cy.get("[data-test=modified-file-list_edited-files]")
        .should("contain", editedFiles[0].fullName)
        .and("contain", editedFiles[1].fullName);
    });

    it("Deleted files displayed", () => {
      cy.get("[data-test=modified-file-list_deleted-files]").should(
        "contain",
        deletedFile.fullName
      );
    });
  });
});
