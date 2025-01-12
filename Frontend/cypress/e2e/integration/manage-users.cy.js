describe("Manage Users Page", () => {
  beforeEach(() => {
    cy.intercept("GET", "/api/User/SearchUserByEmailOrName?search=John", {
      fixture: "john.json",
    }).as("searchUsers");

    // Stel in dat we de pagina bezoeken
    cy.visit("http://localhost:3000/manage-users");
  });

  it('should display the search bar and the "New User" button', () => {
    // Controleer of de zoekbalk en de knop "New User" aanwezig zijn
    cy.get("#searchbar").should("be.visible");
    cy.get(".newUser").should("be.visible");
  });

  it("should fetch and display users when page loads", () => {
    cy.intercept("GET", "/api/User/SearchUserByEmailOrName*", {
      fixture: "users.json",
    }).as("getUsers");
    cy.intercept("GET", "/api/User/GetAllUsers", { fixture: "users.json" }).as(
      "getAllUsers"
    );
    // Zorg ervoor dat de lijst met gebruikers wordt geladen
    cy.wait("@getUsers");

    cy.get("tbody tr").should("have.length.greaterThan", 0);
  });

  it("should allow searching for users", () => {
    cy.get("#searchbar").type("John");

    cy.wait("@searchUsers");

    cy.get("tbody tr").should("have.length", 1);
    cy.get("tbody tr td").first().should("contain", "John");
  });

  it("should open the confirm delete dialogue when delete button is clicked", () => {
    // Simuleer klikken op de delete-knop van een gebruiker
    cy.get(".mdi-delete").first().click();

    // Zorg ervoor dat het bevestigingsdialoogvenster wordt weergegeven
    cy.get(".window").should("be.visible");
  });
});
