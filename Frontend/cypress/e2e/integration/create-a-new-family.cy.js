describe('API Integration Test for Creating a Family', () => {
    it('should create a new family', () => {
      cy.intercept('POST', '/api/Family/CreateFamily', {
        statusCode: 201,
        body: { id: '123', familyName: 'Smith Family' },
      }).as('createFamily');
  
      cy.visit('http://localhost:3000/families/create');
      cy.get('#familyName').type('Smith Family');
      cy.get('button[type="submit"]').click();
      cy.wait('@createFamily').its('response.statusCode').should('eq', 201);
    });
  });
  