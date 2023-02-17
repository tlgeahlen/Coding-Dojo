const mongoose = require('mongoose');
const database = 'authors_db';

mongoose
    .connect(`mongodb://127.0.0.1:27017/${database}`, {
        useNewUrlParser: true,
        useUnifiedTopology: true
    })
    .then(() => {
        console.log(`Successfully connected to ${database}`);
    })
    .catch((err) =>
        console.log(`Mongoose connection to ${database} failed:`, err)
    );