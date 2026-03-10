// * Functions in Javascript

// ! Basic Function
function test(){
    console.log("Hello World")
}

// ! Arithmetic Function
function sum(num1, num2){
    return (num1 + num2);
}

// ! Arrow Functions
const greet = (name) => console.log(`Hello ${name}`)
const product = (num1, num2) => num1 * num2

// ! Calling Functions
test();
console.log(sum(1, 2));
greet("Shiv")
console.log(product(2, 3))


// * Variables in Javascript

// ! var
var a = ["Python","Java","Swift","C#"]

// ! let
let b = 20

// ! const
const c = 30

// ! `map` function 
var numbers = [1,2,3,4,5,6,7,8,9]
var squares = numbers.map(value => value * value)

// ! objects
const people = [
    {
        Id:1,
        Name:"VIII.III.MMV",
        Country:"India"
    },
    {
        Id:2,
        Name:"KPS",
        Country:"Canada"
    },
    {
        Id:3,
        Name:"COD.3",
        Country:"New Zealand"
    }
]

const names = people.map(p => p.Name);
const countries = people.map(p => p.Country);
const greaterThan5 = numbers.filter(n => n > 5);

// ! Calling Variables
console.log(a)
console.log(b)
console.log(c)
console.log(squares)
console.log(people)
console.log(names)
console.log(countries)
console.log(greaterThan5)