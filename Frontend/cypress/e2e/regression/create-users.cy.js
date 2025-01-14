describe('Create a User Page', () => {
    beforeEach(() => {
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
  
    it('should display an error for invalid email format', () => {
      cy.get('#email').type('invalidemail');
      cy.get('button[type="submit"]').click();
  
      cy.contains('Email is required.').should('not.exist');
    });
  
    it('should display an error if the email already exists', () => {
      cy.intercept('POST', '/api/User/checkAccount', {
        statusCode: 200,
        body: { message: 'Account exists' },
      }).as('checkAccount');
  
      cy.get('#firstName').type('Jane');
      cy.get('#lastName').type('Smith');
      cy.get('#email').type('Janesmith@gmail.com');
      cy.get('#password').type('ValidPassword1');
      cy.get('.v-select').click();
      cy.contains('.v-list-item', 'Smith Family').click();
      cy.get('button[type="submit"]').click();
  
      cy.contains('Email already exists.').should('be.visible');
    });
  
    it('should successfully create a user with valid data', () => {
        cy.intercept('POST', '/api/User/CreateUser', {
            statusCode: 200,
          }).as('createUser');
  
      cy.get('#firstName').type('John');
      cy.get('#lastName').type('Doe');
      cy.get('#email').type('newuser@example.com');
      cy.get('#password').type('ValidPassword1');
      cy.get('.v-select').click();
      cy.contains('.v-list-item', 'Smith Family').click();
      cy.get('button[type="submit"]').click();
  
      cy.wait('@createUser');
      cy.url().should('include', '/manage-users');
    });
  });
  