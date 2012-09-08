// KEY MESSAGE: JavaScript has the things you would expect in any C-like language
// Strings, Numbers, and Boolean
var x;
x = 'foo is "bar"';
x = "foo is 'bar'";
x = 1;
x = 3.14;
x = true;
x = false;

// Arrays
x = [1, 2, 3, 4, 5];
x = [1, 2, 'foo'];

// JavaScript Object Notation (JSON)
x = { foo: 'bar', fizz: 'buzz', baz: 42 };

// Regular Expressions
x = new RegExp('^\\d{5}$');
x.test('12345');

x = /[\d+.?(\d)*]+/;
x.exec('The price is $55.99');
'The price is $55.99'.match(x);
'The price is $55.99'.replace(x, '23.49');

// Date
x = new Date();
x = new Date(2012, 11, 26);

x = new Date();
x.setDate(x.getDate() - 1);
x;

// Control Structures
// if and bitwise operators
if(2 + 3 == 5) {
	console.log('yo dawg!');
}
if((2 + 3 == 4) || (4 + 5 == 9)) {
	console.log('yo dawg!');
}

// while, do...while
x = 1;
while( x < 5 ) {
	console.log('yo');
	x++;
}

do {
	console.log('yo');
	x++;
} while( x < 5 );

// for
x = [1, 2, 3, 4, 5];
for (var i = 0; i < x.length; i++)
{
	if(x[i] % 2) continue;
	console.log(x[i]);
}
// **for-in is used for walking over properties, not iterate through arrays
x = { firstName: 'Chuck', lastName: 'Norris' };
for(var key in x) {
	console.log(key + ': ' + x[key]);
}

// switch
x = 'sunny';
switch(x)
{
	case 'sunny':
		console.log('It''s always sunny in Philadelphia');
		break;
	case 'rainy':
		console.log('Are you sure you are not in Seattle?');
		break;
	case 'cloudy':
		console.log('Spending a bit too much time with Azure, eh?');
		break;
	default:
		console.log('Huh?');
		break;
}

// with
x = { name: 'Mr. T' };
with(x) {
	console.log(name);
}

// functions
function sayHello() {
	console.log('hello!');
}
sayHello();

function add(a, b) {
	return a + b;
}
console.log(add(1, 2));

// TAKE AWAY: Feels like a familiar C-like language


// KEY MESSAGE: JavaScript is a dynamic language, don't treat it like a static language
// Equality and Type Conversion
1 == 1
1 == '1'
1 == true
true == 1
0 == ''
0 == '0'
'' == '0'
true + true
true + false
true - true
true * 5 === 5
parseInt(true)
1 === 1
1 === '1'
1 === true
true === 1

function isNumber(n) {
	return !isNaN(parseFloat(n)) && isFinite(n);
}
// source: http://stackoverflow.com/questions/18082/validate-numbers-in-javascript-isnumeric

// Undefined comes before null - Null is value not set at this time, undefined value has never been set
null == undefined; // or is it?
var a;
a == null;
a === null;
a === undefined;
typeof a === 'undefined';

a = 'bar';
a == null;
a === null;
a === undefined;
typeof a === 'undefined';

a = null;
a == null;
a === null;
a === undefined;
typeof a === 'undefined';

if(a == null) { // what about undefined?
	console.log('foo!');
}
if(!a) { // better way - falsey: undefined, null, '', 0 and false
	console.log('foo!');
}

// You can do ugly stuff in JavaScript
Math.random();
Math.random();
Math.random = function() { return 42; }
Math.random();
Math.random();

// TAKE AWAY: Don't underestimate the dynamic nature of JavaScript and it's ability to trip you up

// KEY MESSAGE: Functions are really where it is at (not objects like you might have in a CLR world)

// Objects - always public
var foo = new Object();
foo.name = 'Chuck Norris';
foo.hello = function() {
	console.log('Hello! This is ' + this.name);
};
foo.hello();

// Object Literal format - look familiar? JSON anyone?
var foo = {
	name: 'Chuck Norris',
	hello: function() {
		console.log('Hello! This is ' + this.name);
	}
}

// Functions
function hello(name) {
	var greeting = 'Hello! This is ' + name;
	console.log(greeting);
}

// Function Literals - ability to create instances, mix in privates
var Foo = function(name) {
	// private
	var greeting = 'Hello! This is ' + name;

	// public
	this.hello = function() {
		console.log(greeting);
	};
};

var foo = new Foo('Chuck Norris');
var bar = new Foo('Steven Segal');

foo.hello();
bar.hello();
bar.greeting = 'Imma let you finish...';
bar.hello();

// Mixed Function and Object Literals
var Foo = function(name) {
	// private
	var greeting = 'Hello! This is ' + name;
	var privateHello = function() {
		console.log(greeting);
	};

	// public
	return {
		hello: privateHello
	};
};

// Convert takeaway to slide
// TAKE AWAY: Objects - can't create instances, all public; Functions - create instances, hide the privates
// TAKE AWAY: Functions are the top dog in JavaScript enabling for some really interesting usages (i.e. very natural async / callback patterns)

// KEY MESSAGE: Inheritance is messy

// Prototypes - enabling inheritance and saving you memory

// Consider the following:
var Animal = function(name)
{
	console.log('new animal!');
	var name = 'Hello! ' + name;
	this.getName = function() { return name; }
};

var a = new Animal('Fido');
var b = new Animal('Mr. Jangles');
a.getName == b.getName; // false - we created the function twice in memory

// Prototypes are defined outside of the function literal
Animal.prototype.hello = function()
{
	// Down side is no access to privates
	console.log(this.getName());
};

a.hello == b.hello; // true - sharing the same prototype instance

// Inheritance
var Dog = function(name)
{
	console.log('new dog!');

	// Call parent constructor - think Dog(name) : base(name)
	Animal.call(this, name);
};
// Associate Dog with parent class Animal
Dog.prototype = new Animal();
Dog.prototype.constructor = Dog;
Dog.prototype.hello = function()
{
		console.log('Woof ' + this.getName());

		// Call parent implementation
		Animal.prototype.hello.call(this);
}

var foo = new Animal('Whiskers');
var bar = new Dog('Bananas');
foo.hello();
bar.hello();

foo instanceof Animal
foo instanceof Dog
foo instanceof Object
bar instanceof Animal
bar instanceof Dog
bar instanceof Object

// TAKE AWAY: If you are creating an "object model", use prototypes for inheritance and really test the relationships to ensure they work

// KEY MESSAGE: Be aware of your scope

// Scope - http://stackoverflow.com/a/500459
// a globally-scoped variable
var a=1;

// global scope - like a closure (let's come back to that again)
function one() {
    console.log(a); 
}

// local scope
function two(a){ 
    console.log(a);
}

// local scope again
function three() {
  var a = 3;
  console.log(a);
}

// Intermediate: no such thing as block scope in javascript, functions are scope boundaries
function four() {
    if(true){
        var a=4;
    }

    console.log(a); // console.logs '4', not the global value of '1'
}

one();
two(2);
three();
four();

// TAKE AWAY: Functions are scope boundaries

// KEY MESSAGE: When constructing rich web pages or native apps using JS, understanding when a closure is created will determine how effective the GC can be in stopping memory leakage
// Closures	
function InvoiceCalculator(taxRate) { // Factory anyone?
	var lotsOfBigData = 'fizzbuzz';

	return function(subtotal)
	{
		return parseFloat(subtotal + (subtotal * taxRate)).toFixed(2);
	};
}

var calculator = InvoiceCalculator(0.13);
console.log("$10 + 13% Sales Tax = " + calculator(10));

// Modules - "static classes" / self-executing functions that close the loop
var Foo = (function()
{
	// Private properties and functions
	name = 'Chuck Norris';
	privateHello = function() {
		console.log('Private hello! This is ' + name);
	}

	return {
		// Public properties and functions
		name: 'Steven Segal',
		hello: function() {
			privateHello();
			console.log('Public hello! This is ' + this.name);
		}
	}
})();

Foo.name = 'Bob Smith';
Foo.hello();

// TAKE AWAY: Module pattern rocks

// *** SWITCH TO BROWSER ***

// KEY MESSAGE: When working in the browser, libraries really help
// In the browser, JavaScript is the primary way to manipulate the document object model

// Browser Debugging Tools, JSBin/JSFiddle

<!DOCTYPE html>
<html>
<head>
    <meta charset=utf-8 />
    <title>Hello jQuery</title>
    <style>
        button {
            position: relative;
            margin: 5px;
        }
    </style>
</head>
<body>
    <nav id="mainnav">
        <h1>Navigation</h1>
        <ul>
            <li><a href="#a">Apples</a></li>
            <li class="active"><a href="#b">Bananas</a></li>
            <li><a href="#c">Oranges</a></li>
        </ul>
    </nav>
    <div>
        <ul>
            <li>Foo</li>
            <li>Bar</li>
        </ul>
    </div>
    <div>
        <input id="SecretPhrase" type="password" />
        <button type="submit">Sign In</button>
    </div>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js"></script>  
</body>
</html>


$('li'); 						// Element Selector
$('#mainnav'); 					// ID Selector
$('.active');					// Class Selector
$('nav > ul');					// Child selector
$('input[type="password"]');	// Attribute selector
$(':password');

// on/off
function onSubmit(e) {
	alert('Submitting!');
}
$('button[type=submit]').on('click', onSubmit);

$('button[type=submit]').off('click', onSubmit);

// Shortcuts
$('button[type=submit]').click(function(e) {
	alert('Submitting via the shortcut binding!');
});

// You can even bind to multiple events
$('nav').on('mouseenter mouseleave', function(e) {
  	console.log('navigation: ' + e.type);
});

// Event bubbling
$('li a').on('click', function(e) {
	// this will create three instances
});
$('#mainnav').on('click', 'a', function(e) {
	// this will create one instance
});


// Manipulating Markup
// Change CSS Properties
$('#mainnav').on('click', 'a', function(e) {
	// Change styles
	$(this).css('color', 'black').css('background-color', 'yellow');
	e.preventDefault();
});

// Built-in animations
$('#mainnav h1').click(function() {
	// Animate using pre-built animations
	$(this).parent().children('ul').toggle();
});

// Custom animations
$('button').hover(function() {
	// Animate with specific instrutions
	$(this).animate({"left": '+=50px'}, 'fast');
});

// Change the contents of an element
$('h1').html('<strong>Hello everybody!</strong>');

// Append new elements
$('<p>I am a new element</p>').appendTo('div');

// AJAX
var feedUrl = 'http://feeds.feedburner.com/ICanHasCheezburger?xml';
$.ajax({ 
	url: 'http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&callback=?&q=' + encodeURIComponent(feedUrl),
	dataType: 'json',
	success: function(e) {
		var result = e.responseData.feed;
		if (!result.error) {
		    var root = $('div > ul');
		    root.empty();
		    for (var i = 0; i < result.entries.length; i++) {
		      	var entry = result.entries[i];
		      	if($.inArray('Image', entry.categories) >= 0)
		      	{
			      	var item = $('<li></li>');
			      	$('<strong></strong>').text(entry.title).appendTo(item);
			      	var image = $(entry.content).find('img:first');
			      	$('<div></div>').html(image).appendTo(item);
			      	item.appendTo(root);
			  	}
		    }
		}
	}
});

// TAKE AWAY: You will love jQuery and other JS libraries (microjs.com)

// *** SWITCH TO VISUAL STUDIO ***

// KEY MESSAGE: Testing is there, but it's not as strong as the xUnit frameworks on C#, etc...

// QUnit with the JustCode runner

// System under test
function fibonacci(number) {
    if (number <= 1) {
        return number;
    } else {
      return fibonacci(number-1) + fibonacci(number-2);
    }
}

// Fixture with multiple assertions
test('when calling fibonacci(number)', function() { 
    equal(fibonacci(-1), -1, 'Negative one returns itself'); 
    equal(fibonacci(0), 0, 'So does zero'); 
    equal(fibonacci(1), 1, 'So does one'); 
    equal(fibonacci(2), 1, 'Two returns one'); 
    equal(fibonacci(4), 3, 'Four returns three'); 
    equal(fibonacci(8), 21, 'Eight returns twenty one'); 
})

// AAA style fixture
test("given a hidden div element when show is called it should show the element", function(){
     // Arrange
    $('#subject').css('display', 'none');

    // Act
    $('#subject').show();

    // Assert
    equal($('#subject').css('display'), 'block', 'Element should be visible');
})

// Seperate Module
module('Module A');

test('test within module', function() {
  ok( true, 'all pass' );
});

test('second test within module', function() {
  ok( true, 'all pass' );
});

// JSLint - Must have Visual Studio add-in for JavaScript static code analysis

// TAKE AWAY: JavaScript is not scary and worth learning