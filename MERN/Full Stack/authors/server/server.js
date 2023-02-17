const express = require("express");
const cors = require("cors")
const app = express();
require("../server/config/mongoose.config");

app.use(cors());

app.use(express.json(), express.urlencoded({ extended: true }));

const AllMyUserRoutes = require("../server/routes/author.routes");
AllMyUserRoutes(app);

app.listen(8000, () => console.log("The server is all fired up on port 8000"));