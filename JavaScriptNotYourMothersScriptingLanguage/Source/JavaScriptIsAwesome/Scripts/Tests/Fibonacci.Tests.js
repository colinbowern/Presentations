/// <reference path="../Fibonacci.js" />
/// <reference path="../QUnit/qunit.js" />

// Fixture with multiple assertions
test('when calling fibonacci(number)', function () {
    equal(fibonacci(-1), -1, 'Negative one returns itself');
    equal(fibonacci(0), 0, 'So does zero');
    equal(fibonacci(1), 1, 'So does one');
    equal(fibonacci(2), 1, 'Two returns one');
    equal(fibonacci(4), 3, 'Four returns three');
    equal(fibonacci(8), 21, 'Eight returns twenty one');
})

// AAA style fixture
test("given a hidden div element when show is called it should show the element", function () {
    // Arrange
    $('#subject').css('display', 'none');

    // Act
    $('#subject').show();

    // Assert
    equal($('#subject').css('display'), 'block', 'Element should be visible');
})

// Seperate Module
module('Module A');

test('test within module', function () {
    ok(true, 'all pass');
});

test('second test within module', function () {
    ok(true, 'all pass');
});
