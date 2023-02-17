from flask import render_template, redirect, request, session, flash
from flask_app import app

from flask_app.models.model_boilerplate import Boilerplate
from flask_app.config.helper import findBool

@app.route('/boilerplate/new')
def boilerplate_new():
    return render_template('boilerplate_new.html')

# Action route
@app.route('/boilerplate/create', methods=['POST'])
def boilerplate_create():
    data = {
        **request.form
    }

    data = findBool(data, 'is_vegan') #checkbox, change from 'on/off' to bool val

    Boilerplate.create(data)
    return redirect('/')

@app.route('/boilerplate/<int:id>')
def boilerplate_show(id):
    return render_template('boilerplate_show.html')

@app.route('/boilerplate/<int:id>/edit')
def boilerplate_edit(id):
    boilerplate = Boilerplate.get_one({'id':id})
    return render_template('boilerplate_edit.html', boilerplate=boilerplate)

@app.route('/boilerplate/<int:id>/update', methods=['POST'])
def boilerplate_update(id):
    data = {
        **request.form,
        'id': id
    }

    data = findBool(data, 'is_vegan') #checkbox, change from 'o/f' to boolval

    Boilerplate.update_one(data)
    return redirect('/')

@app.route('/boilerplate/<int:id>/delete')
def boilerplate_delete(id):

    return redirect('/')