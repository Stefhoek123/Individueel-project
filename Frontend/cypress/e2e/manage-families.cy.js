describe('Manage Families Page', () => {
  beforeEach(() => {
    // Mock de API responses
    cy.intercept('GET', '/api/Family/SearchFamilyByName*', { fixture: 'families.json' }).as('getFamilies');
    cy.intercept('GET', '/api/Family/GetAllFamilies', { fixture: 'families.json' }).as('getAllFamilies');
    cy.intercept('DELETE', '/api/Family/DeleteFamilyById*', { fixture: 'families.json' }).as('deleteFamily');
    cy.intercept('GET', '/api/Family/SearchFamilyByName?search=Smith', {
      fixture: 'smith.json' 
    }).as('searchFamilies');

    // Bezoek de pagina
    cy.visit('http://localhost:3000/manage-families');
  });

  it('should display the list of families', () => {
    // Wacht tot de API is afgerond
    cy.wait('@getFamilies');

    // Controleer of de tabel wordt gevuld
    cy.get('tbody tr').should('have.length', 3); // Pas aan naar de lengte van je mockdata
  });

  it('should display "No results found" when no families match the search', () => {
    // Mock een lege response
    cy.intercept('GET', '/api/Family/SearchFamilyByName*', []).as('searchNoResults');

    // Typ een zoekterm die geen resultaten heeft
    cy.get('#searchbar').type('Nonexistent Family{enter}');

    // Wacht op de zoek-API
    cy.wait('@searchNoResults');

    // Controleer dat de "No results" melding wordt weergegeven
    cy.get('.no-results').should('be.visible');
  });

  it('should search for a Smith family', () => {
    // Typ "Smith" in de zoekbalk
    cy.get('#searchbar').type('Smith');

    // Wacht op de API-aanroep om de gefilterde resultaten te krijgen
    cy.wait('@searchFamilies');

    // Controleer of de tabel de Smith-familie bevat
    cy.get('tbody tr').should('have.length', 1); // Er zou maar 1 familie moeten zijn
    cy.get('tbody tr td').first().should('contain', 'Smith'); // Controleer of de eerste rij "Smith" bevat
  });

  it('should navigate to create new family page', () => {
    // Klik op de "New Family" knop
    cy.contains('New Family').click();

    // Controleer de URL
    cy.url().should('include', '/families/create');
  });

  it('should show a confirmation dialogue when deleting a family', () => {
    cy.wait('@getFamilies');
    // Klik op de delete knop van de eerste familie
    cy.get('tbody tr').first().find('.mdi-delete').click();

    // Simuleer het bevestigen van de dialog
    cy.get('.window').within(() => {
      cy.contains('Delete Forever').click();
    });    
   });
});
