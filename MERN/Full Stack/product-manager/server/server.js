
const express = require("express");
const cors = require("cors");
const app = express();
require("../server/config/mongoose.config")
const port = 8000
app.use(cors());

app.use(express.json(), express.urlencoded({ extended: true }));
const AllMyUserRoutes = require("../server/routes/product.routes");
AllMyUserRoutes(app);

app.listen(port, () => console.log(`The server is up and listening on port ${port}`));
