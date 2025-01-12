describe("Manage Users Page", () => {
  beforeEach(() => {
    // Mock API calls
    cy.intercept('GET', '/api/User/SearchUserByEmailOrName*', { fixture: 'users.json' }).as('getUsers');
    cy.intercept('GET', '/api/User/GetAllUsers', { fixture: 'users.json' }).as('getAllUsers');
    cy.intercept('DELETE', '/api/User/DeleteUserById*', { fixture: 'users.json' }).as('deleteUser');
    cy.intercept('GET', '/api/User/SearchUserByEmailOrName?search=John', {
      fixture: 'john.json' 
    }).as('searchUsers');

    cy.visit("http://localhost:3000/manage-users");
  });

  it("should display a list of users", () => {
    // Wait for the users to load
    cy.wait("@getUsers");

    // Check that the table displays users
    cy.get("tbody tr").should("have.length.greaterThan", 0);
    cy.get("thead tr th").first().should("contain", "Firstname");
    cy.get("tbody tr td").first().should("contain", "John");
  });

  it('should display "No results found" when no users match the search', () => {
    // Mock een lege response
    cy.intercept('GET', '/api/User/SearchUserByEmailOrName*', []).as('searchNoResults');

    // Typ een zoekterm die geen resultaten heeft
    cy.get('#searchbar').type('Nonexistent User{enter}');

    // Wacht op de zoek-API
    cy.wait('@searchNoResults');

    // Controleer dat de "No results" melding wordt weergegeven
    cy.get('.no-results').should('be.visible');
  });


  it('should search for John', () => {

    cy.get('#searchbar').type('John');

    cy.wait('@searchUsers');

    cy.get('tbody tr').should('have.length', 1); 
    cy.get('tbody tr td').first().should('contain', 'John'); 
  });

  it("should show a confirmation dialogue when deleting a user", () => {
    // Wait for users to load
    cy.wait("@getUsers");

    // Click the delete button for the first user
    cy.get("tbody tr").first().find(".mdi-delete").click();

    // Check for the confirmation dialog
    cy.get(".window").should("be.visible");
  });

  it("should navigate to the create user page", () => {
    // Click the "New User" button
    cy.get(".newUser").click();

    // Verify navigation to the create user page
    cy.url().should("include", "/users/create");
  });
});
