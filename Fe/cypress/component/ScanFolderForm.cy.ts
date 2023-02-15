import ScanFolderForm from "../../src/components/ScanFolderForm.vue";

describe("<ScanFolderForm />", () => {
  it("Emits path on submit", () => {
    const path = "C:\\test";

    const onSubmitSpy = cy.spy().as("onSubmitSpy");
    cy.mount(ScanFolderForm, { props: { onSubmit: onSubmitSpy } });

    cy.get("[data-test=scan-folder_input]").clear().type(path);

    cy.get("[data-test=scan-folder_submit]").click();

    cy.get("@onSubmitSpy").should("have.been.calledWith", path);
  });

  it("Displays validation error on empty path submit", () => {
    cy.mount(ScanFolderForm);

    cy.get("[data-test=scan-folder_input]").clear();

    cy.get("[data-test=scan-folder_submit]").click();

    cy.get("[data-test=scan-folder_input]:invalid")
      .invoke("prop", "validationMessage")
      .should("equal", "Please fill out this field.");
  });
});
