describe('Edit User Page', () => {
    const mockUser = {
      id: '123',
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@example.com',
      familyId: '1',
      isActive: 1,
    };
  
    const mockFamilies = [
      { id: '1', name: 'Smith Family' },
      { id: '2', name: 'Johnson Family' },
    ];
  
    beforeEach(() => {
      // Mock de API-respons voor het ophalen van de gebruiker
      cy.intercept('GET', '**/api/User/getUserById/*', {
        statusCode: 200,
        body: mockUser,
      }).as('getUserById');
  
      // Mock de API-respons voor het ophalen van de families
      cy.intercept('GET', '**/api/Family/getAllFamilies', {
        statusCode: 200,
        body: mockFamilies,
      }).as('getFamilies');
  
      // Bezoek de pagina om een gebruiker te bewerken
      cy.visit('http://localhost:3000/users/update/E8E79B32-571A-4124-9420-919D7797BEBA'); // Zorg ervoor dat de URL klopt
    });
  
    it('should display user information', () => {
      cy.get('#firstName').should('have.value', "Jane");
      cy.get('#lastName').should('have.value', "NIllesen");
      cy.get('#email').should('have.value', "miranda@gmail.com");
    });
  
    it('should display a validation error for an empty first name', () => {
      cy.get('#firstName').clear(); // Maak het veld leeg
      cy.get('button[type="submit"]').click();
  
      // Controleer of de foutmelding voor de voornaam wordt weergegeven
      cy.get('.error').should('contain', 'First name is required.');
    });
  
    it('should successfully update user information', () => {
      // Wijzig de voornaam van de gebruiker
      cy.get('#firstName').clear().type('Jane');
  
      // Klik op de opslaan knop
      cy.get('button[type="submit"]').click();
  
      // Controleer of de gebruiker wordt doorgestuurd naar de beheerpagina
      cy.url().should('include', '/manage-users');
    });
  
    it('should open a confirmation dialog when clicking delete', () => {
      // Klik op de delete knop
      cy.get('.cardDelete').click();
  
      // Controleer of de confirmatiedialoog wordt weergegeven
      cy.get('.window').should('be.visible');
    });
  });
  