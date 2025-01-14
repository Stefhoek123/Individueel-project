describe('Create a Family Page', () => {
    beforeEach(() => {
      // Bezoek de pagina waar families aangemaakt worden
      cy.visit('http://localhost:3000/families/create'); // Zorg ervoor dat de route correct is
    });
  
    it('should display a validation error for an empty family name', () => {
      cy.get('button[type="submit"]').click();
    
      // Controleer of de foutmelding wordt weergegeven
      cy.get('.error').should('contain', 'Family name is required.');
    });
  
    it('should successfully create a family with a valid name', () => {
      cy.intercept('POST', '/api/Family/CreateFamily', {
        statusCode: 200,
      }).as('createUser');

      // Vul de familienaam in
      cy.get('#familyName').type('Smith Family');
      
      // Klik op de verzendknop
      cy.get('button[type="submit"]').click();

      cy.url().should('include', '/manage-families');
    });
  });
  