describe('Create Family Page', () => {
    beforeEach(() => {
      cy.visit('http://localhost:3000/families/create'); // Pas dit aan naar de juiste URL van je pagina
    });
  
    it('should display an error when the family name is empty and the form is submitted', () => {
      // Klik op de submit-knop zonder een familienaam in te voeren
      cy.get('button[type="submit"]').click();
  
      // Controleer of de foutmelding voor de familienaam wordt weergegeven
      cy.get('.error').should('contain', 'Family name is required.');
    });
  
    it('should navigate to manage families page when form is submitted with valid input', () => {
      // Vul de familienaam in
      cy.get('#familyName').type('Smith Family');
  
      // Klik op de submit-knop
      cy.get('button[type="submit"]').click();
  
      // Wacht op de navigatie naar de manage-families pagina
      cy.url().should('include', '/manage-families');
    });
  
    it('should not submit the form with invalid input', () => {
      // Vul een ongeldige waarde in (bijvoorbeeld geen familienaam)
      cy.get('#familyName').clear();
  
      // Klik op de submit-knop zonder in te vullen
      cy.get('button[type="submit"]').click();
  
      // Controleer of de foutmelding zichtbaar is
      cy.get('.error').should('contain', 'Family name is required.');
    });
  });
  