CREATE DATABASE CursoMod129;

USE CursoMod129;

CREATE TABLE clientes (
	id INT PRIMARY KEY IDENTITY(1,1), -- INT => número inteiro, chave primária
	nome VARCHAR(MAX) NOT NULL, -- string => conjunto de caracteres, Ver MAX 255
	data_de_nascimento DATE NOT NULL, -- Date => Data ('AAAA-MM-DD'),
	morada VARCHAR(50) NOT NULL,
	localidade VARCHAR(100) NOT NULL,
	NIF VARCHAR(50) NOT NULL,
	numero_seguro_saude VARCHAR(50) NULL -- TODO Bruno Pode ser null
);

-- ALTER TABLE

DROP TABLE clientes;


INSERT INTO clientes
(nome, data_de_nascimento, morada, localidade, NIF, numero_seguro_saude)
VALUES
('António Manuel', '1980-01-01', 'Ruas das flores nº27', 'Argozelo', 'PT999999990', NULL);

INSERT INTO clientes
(nome, data_de_nascimento, morada, localidade, NIF, numero_seguro_saude)
VALUES
('Maria Joaquina', '1980-01-01', 'Ruas das flores nº27', 'Argozelo', 'PT999999991', 'MEDIS 1'),
('Félix Costa', '1980-01-01', 'Ruas das flores nº27', 'Argozelo', 'PT999999992', NULL),
('Joana Silva', '1980-01-01', 'Ruas das flores nº27', 'Argozelo', 'PT999999993', 'MULTICARE 1'),
('Manuela Damásio', '1980-01-01', 'Ruas das flores nº27', 'Argozelo', 'PT999999994', 'SAMS 2');


SELECT *
FROM clientes;

BEGIN TRANSACTION;

UPDATE clientes
SET nome = 'António Manuel Silva';

SELECT *
FROM clientes;

ROLLBACK; -- COMMIT


DELETE FROM clientes
WHERE id = 6;


ALTER TABLE clientes
ADD data_de_adesao DATE NOT NULL DEFAULT '2000-01-01';


INSERT INTO clientes
(nome, data_de_nascimento, morada, localidade, NIF, numero_seguro_saude, data_de_adesao)
VALUES
('Barbosa Fernandes', '1980-01-01', 'Ruas das flores nº27', 'Argozelo', 'PT999999990', NULL, '2020-01-01');


ALTER TABLE clientes
DROP DF__clientes__data_d__38996AB5;

ALTER TABLE clientes
DROP COLUMN data_de_adesao;


CREATE TABLE facturas (
	id				INT PRIMARY KEY IDENTITY(1,1),
	numero_factura	VARCHAR(100) NOT NULL,
	descricao		TEXT,
	valor_final		MONEY NOT NULL,
	pago			BIT NOT NULL,
	idCliente		INT NOT NULL FOREIGN KEY REFERENCES clientes (id)
);

SELECT *
FROM facturas;


INSERT INTO facturas
(numero_factura, descricao, valor_final, pago, idCliente)
VALUES
('FS 2023 / 00001', 'Prótese dentária', 499.99, 1, 1);


INSERT INTO facturas
(numero_factura, descricao, valor_final, pago, idCliente)
VALUES
('FS 2023 / 00004', 'Prótese dentária', 599.99, 1, 7),
('FS 2023 / 00005', 'Implante', 799.99, 1, 7),
('FS 2023 / 00006', 'Ortodontia', 899.99, 0, 3),
('FS 2023 / 00007', 'Análise', 199.99, 0, 2),
('FS 2023 / 00008', 'Geral', 99.99, 1, 1);

INSERT INTO facturas
(numero_factura, descricao, valor_final, pago, idCliente)
VALUES
('FS 2023 / 00009', 'Ortodontia', 899.99, 1, 2);

SELECT * FROM clientes;


SELECT facturas.id, nome, numero_factura, descricao, valor_final, pago, NIF
FROM facturas
INNER JOIN clientes
ON facturas.idCliente = clientes.id;



SELECT *
FROM facturas
WHERE descricao LIKE '%o%';

SELECT SUM(valor_final) AS 'Valor a Receber'
FROM facturas
WHERE pago = 0;

SELECT MAX(valor_final) AS 'Maior Factura'
FROM facturas;

SELECT MIN(valor_final) AS 'Menor Factura'
FROM facturas;

SELECT AVG(valor_final) AS 'Média das Factura'
FROM facturas;

SELECT SUM(valor_final), COUNT(id) AS '# Facturas',
	 CASE WHEN pago = 1
		THEN 'Valor Recebido'
		ELSE 'Valor a Receber'
	END
FROM facturas
GROUP BY pago;

SELECT *
FROM facturas;


SELECT *
FROM facturas
WHERE valor_final = (
			SELECT MAX(valor_final)
			FROM facturas
);

SELECT * 
FROM clientes
WHERE YEAR(data_de_nascimento) = 1980;

SELECT * 
FROM clientes
WHERE MONTH(data_de_nascimento) = 2;

SELECT * 
FROM clientes
WHERE DAY(data_de_nascimento) = 1;

SELECT * 
FROM clientes
WHERE data_de_nascimento = '1980-01-01';

SELECT * 
FROM clientes
WHERE data_de_nascimento >= '1980-01-01' AND data_de_nascimento <= '1981-12-31';


SELECT * 
FROM clientes
WHERE data_de_nascimento BETWEEN '1980-01-01' AND '1981-12-31';



CREATE TABLE medicos (
	id					INT PRIMARY KEY IDENTITY(1,1),
	nome				VARCHAR(MAX) NOT NULL, 
	data_de_nascimento	DATE NOT NULL,
	morada				VARCHAR(50) NOT NULL,
	localidade			VARCHAR(100) NOT NULL,
	codigo_postal		VARCHAR(8) NOT NULL,
	NIF					VARCHAR(50) NOT NULL UNIQUE,
	especialidade		VARCHAR(50) NOT NULL
);

CREATE TABLE consultas (
	id				INT PRIMARY KEY IDENTITY(1,1),
	numero_consulta	VARCHAR(100) NOT NULL,
	data_hora		DATETIME NOT NULL,
	observacoes		TEXT,
	realizada		BIT NOT NULL,

	idCliente		INT NOT NULL FOREIGN KEY REFERENCES clientes (id),
	idMedico		INT NOT NULL FOREIGN KEY REFERENCES medicos (id),
	idFactura		INT NULL FOREIGN KEY REFERENCES facturas (id)
);


INSERT INTO medicos
(nome, data_de_nascimento, morada, localidade, codigo_postal, NIF, especialidade)
VALUES
('João das Dores', '1940-08-5', 'Rua dos paralelos nº 42', 'Jerusalém do Romeu', '5370-620', '100130890', 'Generalista'),
('Francisca Fernandes', '1990-03-27', 'Travessa da Rua Torta nº 5', 'Vergas', '3840-555', '250589092', 'Ortodontista'),
('Mariana Maldonada', '1989-06-28', 'Rua das pedras altas nº1', 'Angustias', '4940-401', '270234780', 'Cirurgia Oral');


INSERT INTO consultas
(numero_consulta, data_hora, observacoes, realizada, idCliente, idMedico, idFactura)
VALUES
-- ('C01-0001', '2023-01-01 9:00:00', 'Nada a observar.', 1, 3, 1, 1),
-- ('C02-0010', '2023-04-10 9:00:00', '', 0, 4, 2, NULL),
-- ('C02-0010', '2023-04-10 10:00:00', '', 0, 2, 1, NULL);
-- ('C01-0002', '2023-01-10 10:00:00', '', 1, 2, 1, 6);
('C01-0003', '2023-01-2 9:00:00', '', 1, 2, 2, 7);

SELECT * FROM consultas;

SELECT * FROM facturas;

SELECT * FROM medicos;

SELECT *, CONVERT(DATE, data_hora)
FROM consultas
WHERE CONVERT(DATE, data_hora) = '2023-04-10';

SELECT *
FROM consultas
WHERE idMedico = (
	SELECT id
	FROM medicos
	WHERE nome = 'João das Dores'
	);

SELECT *
FROM consultas
INNER JOIN facturas
ON consultas.idFactura = facturas.id
WHERE facturas.valor_final = (
	SELECT MAX(valor_final)
	FROM facturas
	);


SELECT AVG(facturas.valor_final)
FROM consultas
INNER JOIN facturas
ON consultas.idFactura = facturas.id;


SELECT AVG(facturas.valor_final), idMedico
FROM consultas
INNER JOIN facturas
ON consultas.idFactura = facturas.id
GROUP BY idMedico;

SELECT *
FROM consultas
INNER JOIN facturas
ON consultas.idFactura = facturas.id;