CREATE TABLE BOOKS(
	id INT NOT NULL PRIMARY KEY IDENTITY,
	title VARCHAR (100) NOT NULL,
	authors VARCHAR (255) NOT NULL,
	num_pages INT NOT NULL,
	price DECIMAL (16,2) NOT NULL,
	category VARCHAR(100) NOT NULL,
	description TEXT NOT NULL,
	image_filename VARCHAR(255) NOT NULL,
	created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
	);

INSERT INTO BOOKS(title,authors,num_pages,price,category,description,image_filename)
VALUES
('Zamuri-supe,zupe si nacreli(retete ardelenesti)','Mircea groza',320,119,'culinary','','1.png'),
('Prin pietele lumii','Maria Bahareva',80,75,'illustrative','','2.png'),
('Beyond the story','MYEONGSEEOK KANG',512,149,'biography','','3.png');


