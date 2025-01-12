describe('Manage Families Page', () => {
    beforeEach(() => {
      cy.visit('http://localhost:3000/manage-families');
    });
  
    it('should display the search bar, "New Family" button, and families table', () => {
      cy.get('#searchbar').should('be.visible');
      cy.contains('New Family').should('be.visible');
      cy.get('tbody').should('be.visible');
    });

  it('should fetch and display families when page loads', () => {
    cy.intercept('GET', '/api/Family/SearchFamilyByName*', { fixture: 'families.json' }).as('getFamilies');
    cy.wait('@getFamilies');
  
    cy.get('tbody tr').should('have.length.greaterThan', 0);
  });

  it('should allow searching for families', () => {
    cy.get('#searchbar').type('Smith');  
    cy.get('tbody tr').should('have.length', 3); 
  });

  it('should open the confirm delete dialogue when delete button is clicked', () => {
    cy.get('.mdi-delete').first().click();
    cy.get('.window').should('be.visible');
  });

});
  