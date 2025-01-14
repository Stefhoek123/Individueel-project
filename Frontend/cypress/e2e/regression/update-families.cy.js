describe('Edit Family Page', () => {
    const familyId = '10000000-0000-0000-0000-000000000000'; // Vervang door een geldig familie-ID
  
    beforeEach(() => {
      cy.visit(`http://localhost:3000/families/update/${familyId}`); // Navigeer naar de bewerkpagina voor de familie
    });
  
    it('should display the correct family name and ID', () => {
      cy.get('.title-achievement').should('contain', familyId); // Controleer of het juiste familie-ID wordt weergegeven
  
      cy.get('#familyName').should('have.value', 'Overig'); // Vervang met de verwachte familienaam
    });
  
    it('should show a validation error when family name is empty and submit is clicked', () => {
      // Stel de familienaam in op leeg
      cy.get('#familyName').clear();
  
      // Klik op de opslaan-knop
      cy.get('button[type="submit"]').click();
  
      // Controleer of de validatiefout wordt weergegeven
      cy.get('.error').should('contain', 'Family name is required.');
    });
  
    it('should successfully submit the form when the family name is valid', () => {
      cy.get('#familyName').clear().type('Overig'); // Vul een geldige familienaam in
  
      // Klik op de opslaan-knop
      cy.get('button[type="submit"]').click();
  
      // Controleer of de gebruiker wordt doorgestuurd naar de beheerpagina van de families
      cy.url().should('include', '/manage-families');
    });
  });
  