
describe('My First Test', function() {
    it('Does not do much!', function() {
        cy.visit('http://localhost:44340/Client/Transporters/Transporters');
        
        cy.get('a btn').click();
        
        
        
        
        cy.get('Password').type('supplier');
        
        cy.get("button").click();
        
        cy.url().should('include' , "/Transporter");
    })
})