create table club (
	phoneNumber varchar(15) primary key,
	password varchar(15),
	name varchar(100),
	email varchar(50),
	address varchar(100),
	logo VarBinary(max),
	logoPath varchar(200),
	homeJersey VarBinary(max),
	homeJerseyPath varchar(200),
	awayJersey VarBinary(max),
	awayJerseyPath varchar(200),
	countUpdate int default 0
); 

create  table manager (
	phoneNumber char(15) primary key,
	name varchar(100),
	dob date,
	pob varchar(100),
	age int,
	height float,
	citizenship varchar(50),
	joined date,
	role varchar(50),
	salary int,
	avt VarBinary(max),
	avtPath varchar(200),
	countUpdate int default 0
);

create  table coach (
	phoneNumber char(15) primary key,
	name varchar(100),
	dob date,
	pob varchar(100),
	age int,
	height float,
	citizenship varchar(50),
	joined date,
	role varchar(50),
	salary int,
	avt VarBinary(max),
	avtPath varchar(200),
	countUpdate int default 0
);

create  table player (
	phoneNumber char(15) primary key,
	name char(50),
	dob date,
	pob varchar(100),
	age int,
	height float,
	citizenship char(30),
	joined date,
	role char(30),
	salary int,
	avt VarBinary(max),
	avtPath varchar(200),
	position char(20),
	foot char(10),
	countUpdate int default 0
);


