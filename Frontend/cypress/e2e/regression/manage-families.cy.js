describe("Manage Families Page Regression Tests", () => {
  beforeEach(() => {
    // Visit the manage families page before each test
    cy.visit("http://localhost:3000/manage-families");
  });

  it("should load the page with a list of families and the search bar", () => {
    // Verify that the page title and elements are displayed correctly
    cy.contains("Manage Families").should("be.visible");
    cy.get("#searchbar").should("be.visible");
    cy.contains("New Family").should("be.visible");

    // Verify the table headers
    cy.get("th").eq(0).contains("Family name");
    cy.get("th").eq(1).contains("Actions");
  });

  it("should show no results message when no families are found", () => {
    // Simulate a search with no matching families
    cy.get("#searchbar").type("Nonexistent Family");

    // Wait for the results to load and check if "No results found" message is displayed
    cy.contains("No results found for your search.").should("be.visible");
  });

  it("should update the list of families when the search bar is used", () => {
    // Simulate a search for families
    cy.get("#searchbar").clear().type("Overig");

    // Wait for the families to load and check if the table rows contain matching data
    cy.get("tr").should("have.length.greaterThan", 0);
    cy.get("td").first().contains("Overig"); // Adjust based on the data being searched
  });

  it("should show the confirmation dialogue", () => {
    // Stub the confirmation dialog to simulate confirming the deletion
    cy.window().then((win) => {
      cy.stub(win, "confirm").returns(true); // Simulate clicking the "Delete Forever" button
    });
  });

  it("should open the family detail page when the family name is clicked", () => {
    // Ensure that the table is populated with families
    cy.get("tbody tr").should("have.length.greaterThan", 0);

    // Verify that the family name "Overig" is in the table
    cy.get("tbody").contains("Overig").should("be.visible");

    // Click the family name link for the "Overig" family
    cy.get("tbody").contains("Overig").click();

    // Verify that the URL changes to the family details page
    cy.url().should("include", "/families/");

    // Verify that the URL changes to the family details page
    cy.url().should("include", "/families/");
  });
});
