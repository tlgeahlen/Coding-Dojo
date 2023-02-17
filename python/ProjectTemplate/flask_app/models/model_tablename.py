# Import the function that will return an instance of a connection
from flask_app.config.mysqlconnection import MySQLConnection, connectToMySQL

DATABASE = 'boilerplate_db'

class Boilerplate:
    def __init__( self, data ):
        # in the database table
        self.id = data['id']
        self.attribute1 = data['attribute1']
        self.attribute2 = data['attribute2']
        self.attribute3 = data['attribute3']
        self.attribute4 = data['attribute4']
        self.attribute5 = data['attribute5']
        #so on so forth
        self.created_at = data['created_at']
        self.updated_at = data['updated_at']

    @classmethod
    def create(cls, data):
        query = "INSERT INTO boilerplate (attribute1, attribute2, attribute3-4-5) VALUES (%(attribute1)s, %(attribute2)s, %(attribute3)s);"
        boilerplate_id = MySQLConnection(DATABASE).query_db(query, data)
        return boilerplate_id

    @classmethod
    def get_all(cls):
        query = "SELECT * FROM boilerplate;"

        # return a list of dictionaries
        results = connectToMySQL(DATABASE).query_db(query)

        if not results:
            return []

        all_boilerplate = []
        for dict in results:
            all_boilerplate.append( cls(dict) )
            #returns a list of instances
        return all_boilerplate

    @classmethod
    def get_one(cls, data):
        query = "SELECT * FROM boilerplate WHERE id = %(id)s"

        # datatype ? LIST OF DICTIONARY
        result = connectToMySQL(DATABASE).query_db(query, data)

        if not result:
            return False

        # return an instance
        return cls (result[0])

    @classmethod
    def update_one(cls, data): 
        query = "UPDATE boilerplate SET attribute1=%(attribute1)s, attribute2=%(attribute2)s, attribute3=%(attribute3)s, attribute4=%(attribute4)s WHERE id=%(id)s;"
        return connectToMySQL(DATABASE).query_db(query, data)

    @classmethod
    def delete_one(cls):
        pass