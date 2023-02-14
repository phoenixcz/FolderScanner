// https://docs.cypress.io/api/introduction/api.html

describe("Home", () => {
  it("Scan non existing directory displays error", () => {
    cy.visit("/");

    cy.get("[data-test=scan-folder_input]")
      .clear()
      .type("SomeNotExistingFolder");
    cy.get("[data-test=scan-folder_submit]").click();

    cy.get("[data-test=home_error_message]").should(
      "contain",
      "Entered directory does not exist"
    );
  });
});
