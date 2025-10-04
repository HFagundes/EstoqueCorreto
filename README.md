**TRABALHO GERENCIAMENTO DE ESTOQUE**

Para utilizar o código tem que ser criado o Banco de dados.

Código para criação:

CREATE TABLE produto (
  id_produto INT NOT NULL AUTO_INCREMENT,
  nome VARCHAR(45) NOT NULL,
  preco DECIMAL(10,2) NOT NULL,
  quantidade INT DEFAULT 0,
  PRIMARY KEY (id_produto)
);
