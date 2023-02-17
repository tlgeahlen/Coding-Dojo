const express = require('express');
const {faker} = require('@faker-js/faker')
const app = express();
const port = 8000;

// make sure these lines are above any app.get or app.post code blocks
app.use( express.json() );
app.use( express.urlencoded({ extended: true }) );

const users = [];
const companies = [];

const createUser = () => {
    const newFakeUser = {
        _id: faker.datatype.uuid(),
        firstName: faker.name.firstName(),
        lastName: faker.name.lastName(),
        email: faker.internet.email(),
        phoneNumber: faker.phone.number(),
        password: faker.internet.password()
    };
    return newFakeUser;
};
const createCompany = () => {
    const newFakeCompany = {
        _id: faker.datatype.uuid(),
        name: faker.company.name(),
        city: faker.address.city(),
        state: faker.address.state()
    };
    return newFakeCompany;
};

app.get("/api/users/new", (req, res) => {
    const newUserToAdd = createUser();
    users.push(newUserToAdd);
    res.json({users})
})

app.get("/api/companies/new", (req, res) => {
    const newCompanyToAdd = createCompany();
    companies.push(newCompanyToAdd);
    res.json({companies})
})

app.get("/api/user/company", (req, res) => {
    const newUserToAdd = createUser();
    users.push(newUserToAdd);
    const newCompanyToAdd = createCompany();
    companies.push(newCompanyToAdd);
    res.json({message: 'it worked'})
})

app.listen( port, () => console.log(`Listening on port: ${port}`) );