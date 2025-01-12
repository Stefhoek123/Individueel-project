describe('Family and Members Page with Mock Data', () => {
    beforeEach(() => {
      // Mock de API-aanroepen naar de Family en User endpoints
      cy.intercept('GET', '/api/Family/*', {
        statusCode: 200,
        body: {
          id: '10000000-0000-0000-0000-000000000000',
          familyName: 'Smith Family',
        },
      }).as('getFamily');
  
      cy.intercept('GET', '/api/User/*', {
        statusCode: 200,
        body: [
          {
            id: '1',
            firstName: 'John',
            lastName: 'Smith',
            email: 'john.smith@example.com',
          },
          {
            id: '2',
            firstName: 'Jane',
            lastName: 'Smith',
            email: 'jane.smith@example.com',
          },
        ],
      }).as('getUsers');
  
      cy.visit('http://localhost:3000/families/10000000-0000-0000-0000-000000000000'); // Pas dit aan naar de juiste URL van je pagina
    });
  
    it('should load family data and display members', () => {
      // Wacht op het laden van de familiegegevens
      cy.wait('@getFamily');
  
      // Controleer of de familienaam wordt weergegeven
      cy.get('.title-achievement').should('contain', 'Smith Family');
  
      // Controleer of de tabel met leden aanwezig is
      cy.get('tbody tr').should('have.length', 2); // Twee leden in de mockdata
  
      // Controleer of de leden correct worden weergegeven
      cy.get('tbody tr td').first().should('contain', 'John Smith'); // Controleer of de eerste rij de naam 'John Smith' bevat
      cy.get('tbody tr td').eq(2).should('contain', 'Jane Smith'); // Controleer of de tweede rij de naam 'Jane Smith' bevat
    });
  
    it('should show confirmation dialog when trying to delete a family member', () => {
      // Wacht op de lijst van gebruikers
      cy.wait('@getUsers');
  
      // Klik op de verwijderknop voor het eerste lid
      cy.get('tbody tr').first().find('.mdi-delete').click();
  
      // Controleer of de bevestigingsdialoog zichtbaar is
      cy.get('.window').should('be.visible');
    
    });
  });
  