/// <reference path="../TaxCalculator.js" />
/// <reference path="../Jasmine/jasmine.js" />

// Feature: Tax Rate Calculator
// * Canadian customers pay GST
// * Overseas customers do not pay GST

// Scenario: Calculate Order Total with Taxes
//    Given a customer from <country>
//    When the customer has an order for <sub-total>
//    Then the customer <pays sales tax>

describe("Given a customer from Canada", function () {
    var target = new TaxCalculator('CA');

    it("When the customer has an order for $10 then the order total should be $11.30", function () {
        var result = target.calculate(10);
        expect(result).toEqual('11.30');
    });
});

describe("Given a customer from New Zealand", function () {
    var target = new TaxCalculator('NZ');

    it("When the customer has an order for $10 then the order total should be $10", function () {
        var result = target.calculate(10);
        expect(result).toEqual('10.00');
    });
});

describe("Given a customer from United States", function () {
    var target = new TaxCalculator('US');

    it("When the customer has an order for $10 then the order total should be $10.40", function () {
        var result = target.calculate(10);
        expect(result).toEqual('10.40');
    });
});