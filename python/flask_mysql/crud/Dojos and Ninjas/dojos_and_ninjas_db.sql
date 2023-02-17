INSERT INTO dojos (name) VALUES ('Miyagi Do'), ('Cobra Kai'), ('AnaconDo');

SET SQL_SAFE_UPDATES = 0;
DELETE FROM dojos;

INSERT INTO dojos (name) VALUES ('Miyagi Do'), ('Cobra Kai'), ('AnaconDo');

INSERT INTO ninjas (first_name, last_name, age, dojo_id) 
VALUES ('Travis', 'G', 33, 4), ('Grant', 'S', 34, 4), ('Joe', 'P', 32, 4);

INSERT INTO ninjas (first_name, last_name, age, dojo_id) 
VALUES ('Nikki', 'H', 32, 5), ('Jeanett', 'I', 28, 5), ('Allison', 'R', 35, 5);

INSERT INTO ninjas (first_name, last_name, age, dojo_id) 
VALUES ('Devin', 'J', 30, 6), ('Kalypso', 'J', 32, 6), ('Alexis', 'P', 29, 6);

SELECT * FROM ninjas
JOIN dojos ON ninjas.dojo_id = dojos.id
WHERE dojos.id = 4;

SELECT * FROM ninjas
JOIN dojos ON ninjas.dojo_id = dojos.id
WHERE ninjas.dojo_id = (SELECT MAX(dojo_id) from ninjas);

SELECT dojos.name AS last_ninja_dojo FROM dojos
JOIN ninjas ON ninjas.dojo_id = dojos.id
WHERE ninjas.id = (SELECT MAX(ninjas.id) from ninjas);