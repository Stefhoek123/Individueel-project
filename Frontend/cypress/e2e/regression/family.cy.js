describe('Manage Family Page', () => {
    beforeEach(() => {
      cy.visit('http://localhost:3000/families/10000000-0000-0000-0000-000000000000'); // Zorg ervoor dat de URL klopt
    });
  
    it('should display family name and members', () => {
      // Controleer of de familienaam correct wordt weergegeven
      cy.get('.title-achievement').should('contain', 'Overig'); // Dit kan worden aangepast naar een verwachte familienaam
  
      // Controleer of de leden van de familie correct worden weergegeven
      cy.get('tbody tr').should('have.length.greaterThan', 0); // Verifieer dat er meer dan 0 leden in de lijst staan
    });
  
    it('should navigate to user edit page when clicking edit button', () => {
      cy.get('.mdi-pen').first().click(); // Klik op de eerste bewerken-knop
      // Controleer of we naar de juiste route voor het bewerken van de gebruiker navigeren
      cy.url().should('include', '/users/update/'); // De URL zou moeten beginnen met /users/update/
    });
  
    it('should open a confirmation dialog when clicking delete button', () => {
      cy.get('.mdi-delete').first().click(); // Klik op de eerste verwijder-knop
      cy.get('.window').should('be.visible'); // Controleer of de bevestigingsdialoog zichtbaar is
    });
  
    it('should navigate back to manage families page when clicking the back button', () => {
      cy.get('.mdi-arrow-left').click(); // Klik op de terugknop
      cy.url().should('include', '/manage-families'); // Controleer of we naar de manage-families pagina navigeren
    });
  });
  