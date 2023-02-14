import DisplayMessage from "../../src/components/DisplayMessage.vue";

describe("<DisplayMessage />", () => {
  it("renders value", () => {
    const text = "Hello!";

    cy.mount(DisplayMessage, { props: { value: text } });

    cy.get("[data-test=display-message]").should("contain", text);
  });

  it("Displays error in red", () => {
    const text = "Hello!";

    cy.mount(DisplayMessage, { props: { value: text, isError: true } });

    cy.get("[data-test=display-message]").should(
      "have.css",
      "color",
      "rgb(255, 0, 0)"
    );
  });
});
