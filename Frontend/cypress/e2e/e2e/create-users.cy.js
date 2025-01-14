describe('Create User Page', () => {
    beforeEach(() => {
      // Mock de families API
      cy.intercept('GET', '/api/Family/GetAllFamilies', {
        statusCode: 200,
        body: [
          { id: '1', familyName: 'Smith Family' },
          { id: '2', familyName: 'Johnson Family' },
        ],
      }).as('getFamilies');
  
      // Bezoek de pagina
      cy.visit('http://localhost:3000/users/create');
      cy.wait('@getFamilies');
    });
  
    it('should display validation errors for empty fields', () => {
      cy.get('button[type="submit"]').click();
  
      // Controleer op validatiefouten
      cy.contains('First name is required.').should('be.visible');
      cy.contains('Last name is required.').should('be.visible');
      cy.contains('Email is required.').should('be.visible');
      cy.contains('Password must be at least 8 characters long and contain at least one letter and one number.').should('be.visible');
      cy.contains('Family is required.').should('be.visible');
    });
  
    it('should allow filling the form and submitting successfully', () => {
      // Mock de API-aanroep voor het aanmaken van een gebruiker
      cy.intercept('POST', '/api/User/CreateUser', {
        statusCode: 200,
      }).as('createUser');
  
      // Vul het formulier in
      cy.get('#firstName').type('John');
      cy.get('#lastName').type('Doe');
      cy.get('#email').type('john.doe@example.com');
      cy.get('#password').type('password123');
      cy.get('.v-select').click();
      cy.contains('.v-list-item', 'Smith Family').click();
  
      // Controleer dat validatiefouten verdwijnen
      cy.contains('First name is required.').should('not.exist');
      cy.contains('Last name is required.').should('not.exist');
      cy.contains('Email is required.').should('not.exist');
      cy.contains('Password must be at least 8 characters long and contain at least one letter and one number.').should('not.exist');
      cy.contains('Family is required.').should('not.exist');
  
      // Klik op de submit-knop
      cy.get('button[type="submit"]').click();
  
      // Wacht op de API-aanroep en controleer navigatie
      cy.wait('@createUser');
      cy.url().should('include', '/manage-users');
    });
  
    it('should load families into the select dropdown', () => {
      // Open de dropdown en controleer dat families zichtbaar zijn
      cy.get('.v-select').click();
      cy.contains('.v-list-item', 'Smith Family').should('be.visible');
      cy.contains('.v-list-item', 'Johnson Family').should('be.visible');
    });
  });
  