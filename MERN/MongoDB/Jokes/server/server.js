const express = require("express");
const app = express();
const port = 8000;
require("../server/config/mongoose.config");

app.use(express.json(), express.urlencoded({extended: true}));

const MyJokeRoutes = require("../server/routes/jokes.routes");
MyJokeRoutes(app);

app.listen(port, () => console.log(`The server is running on port ${port}`));