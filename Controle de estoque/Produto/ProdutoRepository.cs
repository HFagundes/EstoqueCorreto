using MySql.Data.MySqlClient;

public class ProdutoRepository
{
    private ConexaoBD conexaoBD;

    public ProdutoRepository()
    {
        conexaoBD = new ConexaoBD();
    }

    public bool CadastrarProduto(Produto produto)
    {
        using (MySqlConnection conexao = conexaoBD.ObterConexao())
        {
            try
            {
                conexao.Open();
                string query = "INSERT INTO produto (nome, preco, quantidade) VALUES (@nome, @preco, @quantidade)";
                
                using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", produto.Nome);
                    cmd.Parameters.AddWithValue("@preco", produto.Preco);
                    cmd.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                    
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar produto: {ex.Message}", "Erro", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    public bool IncrementarQuantidade(int idProduto, int quantidade)
    {
        using (MySqlConnection conexao = conexaoBD.ObterConexao())
        {
            try
            {
                conexao.Open();
                string query = "UPDATE produto SET quantidade = quantidade + @quantidade WHERE id_produto = @id";
                
                using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@quantidade", quantidade);
                    cmd.Parameters.AddWithValue("@id", idProduto);
                    
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao incrementar quantidade: {ex.Message}", "Erro", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    public bool DecrementarQuantidade(int idProduto, int quantidade)
    {
        using (MySqlConnection conexao = conexaoBD.ObterConexao())
        {
            try
            {
                conexao.Open();
                string verificaQuery = "SELECT quantidade FROM produto WHERE id_produto = @id";
                using (MySqlCommand verificaCmd = new MySqlCommand(verificaQuery, conexao))
                {
                    verificaCmd.Parameters.AddWithValue("@id", idProduto);
                    var estoqueAtual = verificaCmd.ExecuteScalar();
                    
                    if (estoqueAtual != null && Convert.ToInt32(estoqueAtual) >= quantidade)
                    {
                        string query = "UPDATE produto SET quantidade = quantidade - @quantidade WHERE id_produto = @id";
                        using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                        {
                            cmd.Parameters.AddWithValue("@quantidade", quantidade);
                            cmd.Parameters.AddWithValue("@id", idProduto);
                            
                            int result = cmd.ExecuteNonQuery();
                            return result > 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Quantidade insuficiente em estoque!", "Aviso", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao decrementar quantidade: {ex.Message}", "Erro", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    public List<Produto> ListarProdutos()
    {
        List<Produto> produtos = new List<Produto>();
        
        using (MySqlConnection conexao = conexaoBD.ObterConexao())
        {
            try
            {
                conexao.Open();
                string query = "SELECT * FROM produto ORDER BY id_produto";
                
                using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        produtos.Add(new Produto
                        {
                            IdProduto = reader.GetInt32("id_produto"),
                            Nome = reader.GetString("nome"),
                            Preco = reader.GetDecimal("preco"),
                            Quantidade = reader.GetInt32("quantidade")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao listar produtos: {ex.Message}", "Erro", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        return produtos;
    }

    public Produto BuscarProdutoPorId(int idProduto)
    {
        using (MySqlConnection conexao = conexaoBD.ObterConexao())
        {
            try
            {
                conexao.Open();
                string query = "SELECT * FROM produto WHERE id_produto = @id";
                
                using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", idProduto);
                    
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Produto
                            {
                                IdProduto = reader.GetInt32("id_produto"),
                                Nome = reader.GetString("nome"),
                                Preco = reader.GetDecimal("preco"),
                                Quantidade = reader.GetInt32("quantidade")
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao buscar produto: {ex.Message}", "Erro", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        return null;
    }
}