from flask_app import app
from flask_app.controllers import controller_boilplate, controller_routes

# KEEP AT THE BOTTOM
if __name__=="__main__":
    app.run(debug=True)