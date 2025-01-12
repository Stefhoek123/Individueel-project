describe('Edit User Page', () => {
    beforeEach(() => {
      // Mock de API-aanroepen
      cy.intercept('GET', '/api/User/GetUserById/E8E79B32-571A-4124-9420-919D7797BEBA', {
        statusCode: 200,
        body: {
          id: "E8E79B32-571A-4124-9420-919D7797BEBA",
          firstName: 'Miranda2',
          lastName: 'NIllesen',
          email: 'miranda@gmail.com',
          familyId: 'A14ABB13-CAD1-4D57-872A-1EAF7032F6CB',
          isActive: 1,
        },
      }).as('getUserById');
  
      cy.intercept('GET', '/api/Family/GetAllFamilies', {
        statusCode: 200,
        body: [
          { id: 'a14abb13-cad1-4d57-872a-1eaf7032f6cb', familyName: 'Nillesen' },
          { id: '2', familyName: 'Johnson Family' },
        ],
      }).as('getFamilies');
  
      cy.visit('http://localhost:3000/users/update/E8E79B32-571A-4124-9420-919D7797BEBA');
       cy.wait('@getFamilies');
    });
  
    it('should load the user data and display it in the form', () => {
      // Controleer of de gegevens correct geladen zijn
      cy.get('#firstName').should('have.value', 'Miranda2');
      cy.get('#lastName').should('have.value', 'NIllesen');
      cy.get('#email').should('have.value', 'miranda@gmail.com');
    });
  
    it('should display validation errors for empty fields', () => {
      // Maak velden leeg door ze te verwijderen
      cy.get('#firstName').clear();
      cy.get('#lastName').clear();
      cy.get('#email').clear();
  
      cy.get('button[type="submit"]').click();
  
      // Controleer op validatiefouten
      cy.contains('First name is required.').should('be.visible');
      cy.contains('Last name is required.').should('be.visible');
      cy.contains('Email is required.').should('be.visible');
    });
  
    it('should allow filling the form and submitting successfully', () => {
      // Mock de API-aanroep voor het bijwerken van de gebruiker
      cy.intercept('PUT', '/api/User/UpdateUser', {
        statusCode: 200,
      }).as('updateUser');

      // Vul het formulier in met nieuwe gegevens
      cy.get('#firstName').clear().type('Jane');
      cy.get('#lastName').clear().type('Smith');
      cy.get('#email').clear().type('jane.smith@example.com');
      cy.get('.v-select').click();
      cy.contains('.v-list-item', 'Johnson Family').click();

        // Controleer dat validatiefouten verdwijnen
        cy.contains('First name is required.').should('not.exist');
        cy.contains('Last name is required.').should('not.exist');
        cy.contains('Email is required.').should('not.exist');
  
      // Klik op de submit-knop
      cy.get('button[type="submit"]').click();
  
      // Wacht op de API-aanroep en controleer navigatie
      cy.wait('@updateUser');
      cy.url().should('include', '/manage-users');
    });
  
    it('should load families into the select dropdown', () => {
      // Open de dropdown en controleer of families zichtbaar zijn
      cy.get('.v-select').click();
      cy.contains('.v-list-item', 'Nillesen').should('be.visible');
      cy.contains('.v-list-item', 'Johnson Family').should('be.visible');
    });
  });
  