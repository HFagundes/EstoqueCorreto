using MySql.Data.MySqlClient;

public class ConexaoBD
{
    private string connectionString = "Server=localhost;Database=estoque_db;Uid=root;Pwd=123;";

    public MySqlConnection ObterConexao()
    {
        return new MySqlConnection(connectionString);
    }
}
