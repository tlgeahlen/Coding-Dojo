// immutable - unable to push or pop objects from anything that has .freeze
const groceryList = Object.freeze([
    { "item": "carrots",           "haveIngredient": false },
    { "item": "onions",            "haveIngredient": true  },
    { "item": "celery",            "haveIngredient": false },
    { "item": "cremini mushrooms", "haveIngredient": false },
    { "item": "butter",            "haveIngredient": true  }
]);

// decide we need to add thyme to array - create a copy of array using ...
// const needThyme = [ ...groceryList, { "item": "thyme", "haveIngredient": false } ];

// instead of using ... can do a concat if wanted
const needThyme = groceryList.concat( [ { "item": "thyme", "haveIngredient": false } ] );

// if we find that we already have thyme and want to change haveIngredient to true (.slice can take in 2 params, I'm assuming 0 = index 0 and 5 = index 5 of copied array, index 5 would be thyme)
const gotTheThyme = [ ...needThyme.slice(0, 5), {...needThyme[5], "haveIngredient": true}];

// if we don't need celery in the array as an ingredient anymore, we can remove it by also using slice
// Once again we can use slice, the first slice giving us the ingredients at indexes 0 and 1, the second slice giving us all the ingredients with indexes from 3 to the end.
const notNeceCelery = [ ...gotTheThyme.slice(0,2), ...gotTheThyme.slice(3)];
console.log(notNeceCelery)

// Now we need to sort the array, but remember, it's on a .freeze, so how?
const items = Object.freeze(["carrots", "onions", "celery", "mushrooms", "butter", "thyme"]);
// items.sort(); // this will throw an error, but can use the spread "..." operator to create a copy and get around the immutable
const sortedItems = [...items].sort();

// ******************************************************************************************************************************
// Sorting numbers however doesn't always return what we expect, or in any particular order
const numbers = [10, 5, 3, 12, 22, 8];
numbers.sort();
// this will return [10, 12, 22, 3, 5, 8 ]

// w3Schools workaround
// Sort array:

const fruits = ["Banana", "Orange", "Apple", "Mango"];
fruits.sort();
// Sort and then reverse the order:

const fruits = ["Banana", "Orange", "Apple", "Mango"];
fruits.sort();
fruits.reverse();
// ******************************************************************************************************************************

