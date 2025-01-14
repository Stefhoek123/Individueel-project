describe('Edit Family Page - E2E Test', () => {
    const familyId = '29d4119f-ef30-4878-8ebd-3fad603bf77a'; // Vervang dit door een geldig familie-ID
  
    beforeEach(() => {
      // Zorg ervoor dat je naar de juiste URL navigeert (pas de URL aan naar de juiste lokale setup)
      cy.visit(`http://localhost:3000/families/update/${familyId}`);
    });
  
    it('should display the family ID and current family name', () => {
      // Verifiëren dat het juiste familie-ID wordt weergegeven
      cy.get('.title-achievement').should('contain', `Family ID: ${familyId}`);
  
      // Verifiëren dat de familienaam correct wordt weergegeven (deze waarde komt uit de mockdata of API)
      cy.get('#familyName').should('have.value', 'Smith Family'); // Vervang met de verwachte familienaam
    });
  
    it('should show validation error when family name is empty and submit is clicked', () => {
      // Maak het veld 'familyName' leeg
      cy.get('#familyName').clear();
  
      // Klik op de 'Save' knop
      cy.get('button[type="submit"]').click();
  
      // Verifiëren dat de foutmelding voor een ontbrekende familienaam wordt weergegeven
      cy.get('.error').should('contain', 'Family name is required.');
    });
  
    it('should successfully submit the form and navigate to the manage families page', () => {
      // Vul een geldige familienaam in
      cy.get('#familyName').clear().type('Smith Family');
  
      // Klik op de 'Save' knop
      cy.get('button[type="submit"]').click();
  
      // Verifiëren dat de gebruiker wordt doorgestuurd naar de pagina voor het beheren van families
      cy.url().should('include', '/manage-families');
    });
  });
  