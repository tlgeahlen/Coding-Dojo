from flask_app.config.mysqlconnection import connectToMySQL

class Ninja:
    def __init__(self, data):
        self.id = data['id']
        self.first_name = data['first_name']
        self.last_name = data['last_name']
        self.age = data['age']
        self.created_at = data['created_at']
        self.updated_at = data['updated_at']

    @classmethod
    def create_ninja(cls, data):
        query = 'INSERT INTO ninjas (first_name, last_name, age, dojo_id) VALUES (%(first_name)s, %(last_name)s, %(age)s, %(dojo)s);'
        return connectToMySQL('dojos_and_ninjas_db').query_db(query, data)

    @classmethod
    def get_all_ninjas(cls):
        query = 'SELECT * FROM ninjas'
        ninjas_from_db = connectToMySQL('dojos_and_ninjas_db').query_db(query)
        if not ninjas_from_db:
            return []
        ninjas = []
        for dict in ninjas_from_db:
            print(dict)
            ninjas.append(dict)
        return ninjas