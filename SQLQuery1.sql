CREATE TABLE messages(
	id int not null primary key identity,
	firstname varchar (100) not null,
	lastname varchar (100) not null,
	email varchar (150) not null,
	phone varchar(20) not null,
	subject varchar(255) not null,
	message TEXT not null,
	created_at DATETIME not null default CURRENT_TIMESTAMP
	);