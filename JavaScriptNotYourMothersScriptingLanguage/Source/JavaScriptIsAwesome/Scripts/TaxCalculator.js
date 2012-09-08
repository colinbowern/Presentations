function TaxCalculator(country) {
    var taxRates = {
        CA: 0.13,
        US: 0.04
    };
    var taxRate = isNaN(taxRates[country]) ? 0 : taxRates[country];

    this.calculate = function (orderSubTotal) {
        return parseFloat(orderSubTotal * (1 + taxRate)).toFixed(2);
    };
};