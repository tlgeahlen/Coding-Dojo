from flask_app import app
from flask import render_template, redirect, request, session, flash
from flask_app.models.model_dojo import Dojo

@app.route('/dojos')
def dojos():
    dojos = Dojo.get_all_dojos()
    print(dojos)
    return render_template('index.html', dojos=dojos)

@app.route('/dojos/create', methods=['POST'])
def create_dojo():
    data = {
        **request.form
    }
    Dojo.create_dojo(data)
    return redirect('/dojos')
    
@app.route('/dojos/show/<int:id>')
def show_dojo(id):
    dojo = Dojo.get_one_dojo({'id': id})
    return render_template('dojo.html', dojo=dojo)