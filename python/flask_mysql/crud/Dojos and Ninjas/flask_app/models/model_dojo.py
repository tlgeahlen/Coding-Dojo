from flask_app.config.mysqlconnection import connectToMySQL
from flask_app.models import model_ninja

class Dojo:
    def __init__(self, data):
        self.name = data['name']
        self.created_at = data['created_at']
        self.updated_at = data['updated_at']
        self.ninjas = []

    @classmethod
    def create_dojo(cls, data):
        query = 'INSERT INTO dojos (name) VALUES (%(name)s);'
        return connectToMySQL('dojos_and_ninjas_db').query_db(query, data)

    @classmethod
    def get_all_dojos(cls):
        query = 'SELECT * FROM dojos'
        dojos_from_db = connectToMySQL('dojos_and_ninjas_db').query_db(query)
        if not dojos_from_db:
            return []
        dojos = []
        for dict in dojos_from_db:
            print(dict)
            dojos.append(dict)
        return dojos

    @classmethod
    def get_one_dojo(cls, data):
        query = 'SELECT * FROM dojos LEFT JOIN ninjas ON dojos.id = ninjas.dojo_id WHERE dojos.id = %(id)s;'
        result = connectToMySQL('dojos_and_ninjas_db').query_db(query, data)
        print(result)
        if len(result) < 1:
            return False
        dojo = cls(result[0])
        for ninja in result:
            ninja_data = {
                'id': ninja['ninjas.id'],
                'first_name': ninja['first_name'],
                'last_name': ninja['last_name'],
                'age': ninja['age'],
                'created_at': ninja['ninjas.created_at'],
                'updated_at': ninja['ninjas.updated_at']
            }
            dojo.ninjas.append(model_ninja.Ninja(ninja_data))
        return dojo