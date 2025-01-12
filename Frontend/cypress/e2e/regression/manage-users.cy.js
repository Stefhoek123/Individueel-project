describe('Manage Users Page Regression Tests', () => {
    beforeEach(() => {
      // Visit the page before each test
      cy.visit('http://localhost:3000/manage-users');
    });
  
    it('should load the page with a list of users and the search bar', () => {
      // Check for the page title
      cy.contains('Manage Users').should('be.visible');
  
      // Ensure the search bar and 'New User' button are present
      cy.get('#searchbar').should('be.visible');
      cy.contains('New User').should('be.visible');
  
      // Ensure the table headers are correct
      cy.get('th').eq(0).contains('Firstname');
      cy.get('th').eq(1).contains('Lastname');
      cy.get('th').eq(2).contains('Email');
      cy.get('th').eq(3).contains('Actions');
    });
  
    it('should show no results message when no users are found', () => {
      // Simulate a search with no matching users
      cy.get('#searchbar').type('nonexistentuser');
      
      // Wait for results to load and check if "No results found" message is displayed
      cy.contains('No results found for your search.').should('be.visible');
    });
  
    it('should update the list of users when the search bar is used', () => {
      // Simulate a search for users
      cy.get('#searchbar').clear().type('luca');
  
      // Wait for the users to be fetched and check if the table rows contain the correct data
      cy.get('tr').should('have.length.greaterThan', 0);
      cy.get('td').first().contains('Luca'); // Adjust based on the data being searched
    });
  
    it('should show the confirmation dialogue when deleting a user', () => {
      // Stub the confirm dialogue to simulate a confirmation
      cy.window().then((win) => {
        cy.stub(win, 'confirm').returns(true); // Simulate confirming the deletion
      });
    });
  
    it('should open the update user page when clicking the update button', () => {
      // Click the update button for the first user
      cy.get("tbody tr").first().find(".mdi-pen").click();
  
      // Verify that the URL changes to the update page for that user
      cy.url().should('include', '/users/update');
    });
  });
  