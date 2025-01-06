describe('Manage Users Page', () => {
    beforeEach(() => {
      // Mock API calls
      cy.intercept('GET', '/api/users', { fixture: 'users.json' }).as('getUsers');
      cy.intercept('POST', '/api/users', { statusCode: 201 }).as('createUser');
      cy.intercept('DELETE', '/api/users/*', { statusCode: 200 }).as('deleteUser');
    });
  
    it('should display a list of users', () => {
      cy.visit('/users');
      
      // Wait for the users to load
      cy.wait('@getUsers');
  
      // Check that the table displays users
      cy.get('tbody tr').should('have.length.greaterThan', 0);
      cy.get('tbody tr td').first().should('contain', 'Firstname');  // Assuming the first row has column headers
    });
  
    it('should filter users based on search text', () => {
      cy.visit('/users');
      
      // Wait for users to load
      cy.wait('@getUsers');
      
      // Interact with the search bar and simulate input
      cy.get('#searchbar').type('john');
      
      // Wait for the users to be filtered
      cy.wait('@getUsers');
      
      // Ensure the filtered users are shown in the table
      cy.get('tbody tr').should('have.length', 1);
      cy.get('tbody tr td').first().should('contain', 'John');
    });
  
    it('should show a confirmation dialogue when deleting a user', () => {
      cy.visit('/users');
      
      // Wait for users to load
      cy.wait('@getUsers');
      
      // Click the delete button for the first user
      cy.get('tbody tr').first().find('.mdi-delete').click();
      
      // Check for the confirmation dialog
      cy.get('.v-dialog').should('be.visible');
      cy.get('.v-dialog button').contains('Delete Forever').click();
      
      // Wait for delete request
      cy.wait('@deleteUser');
      
      // Ensure the user is removed from the list
      cy.get('tbody tr').should('have.length', 0);  // Assuming the user is deleted and no other users are displayed
    });
    
    it('should navigate to the create user page', () => {
      cy.visit('/users');
      
      // Click the "New User" button
      cy.get('button').contains('New User').click();
      
      // Verify navigation to the create user page
      cy.url().should('include', '/users/create');
    });
  });
  