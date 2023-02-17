const AuthorController = require('../controllers/author.controllers');

module.exports = app => {
    app.get('/api/authors', AuthorController.findAllAuthors);
    app.get('/api/authors/:id', AuthorController.findOneAuthor);
    app.put('/api/authors/:id', AuthorController.updateAuthor);
    app.post('/api/authors', AuthorController.createAuthor);
    app.delete('/api/authors/:id', AuthorController.deleteAuthor);
}