using System;
using System.Collections.Generic;
using System.Windows.Forms;

public partial class MainForm : Form
{
    private ProdutoRepository repository;
    private DataGridView dataGridViewProdutos;

    public MainForm()
    {
        InitializeComponent();
        repository = new ProdutoRepository();
        CarregarProdutos();
    }

    private void InitializeComponent()
    {
        this.Text = "Sistema de Controle de Estoque";
        this.Size = new System.Drawing.Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

      
        TabControl tabControl = new TabControl();
        tabControl.Dock = DockStyle.Fill;
        tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

     
        TabPage tabListagem = new TabPage("Listagem de Produtos");
        tabListagem.BackColor = SystemColors.Control;
        
    
        TabPage tabCadastro = new TabPage("Cadastrar Produto");
        tabCadastro.BackColor = SystemColors.Control;
        

        TabPage tabEstoque = new TabPage("Gerenciar Estoque");
        tabEstoque.BackColor = SystemColors.Control;

        tabControl.TabPages.Add(tabListagem);
        tabControl.TabPages.Add(tabCadastro);
        tabControl.TabPages.Add(tabEstoque);

  
        ConfigurarAbaListagem(tabListagem);
        
     
        ConfigurarAbaCadastro(tabCadastro);
        
     
        ConfigurarAbaEstoque(tabEstoque);

        this.Controls.Add(tabControl);
    }

    private void ConfigurarAbaListagem(TabPage tabPage)
    {

        Button btnAtualizar = new Button();
        btnAtualizar.Text = "Atualizar Lista";
        btnAtualizar.Location = new System.Drawing.Point(10, 10);
        btnAtualizar.Size = new System.Drawing.Size(100, 30);
        btnAtualizar.Click += (s, e) => CarregarProdutos();

        // DataGridView para exibir produtos
        dataGridViewProdutos = new DataGridView();
        dataGridViewProdutos.Location = new System.Drawing.Point(10, 50);
        dataGridViewProdutos.Size = new System.Drawing.Size(750, 400);
        dataGridViewProdutos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridViewProdutos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridViewProdutos.ReadOnly = true;

        tabPage.Controls.Add(btnAtualizar);
        tabPage.Controls.Add(dataGridViewProdutos);
    }

    private void ConfigurarAbaCadastro(TabPage tabPage)
    {
  
        Label lblNome = new Label() { Text = "Nome:", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(100, 20) };
        Label lblPreco = new Label() { Text = "Preço:", Location = new System.Drawing.Point(20, 60), Size = new System.Drawing.Size(100, 20) };
        Label lblQuantidade = new Label() { Text = "Quantidade:", Location = new System.Drawing.Point(20, 100), Size = new System.Drawing.Size(100, 20) };

 
        TextBox txtNome = new TextBox() { Location = new System.Drawing.Point(120, 20), Size = new System.Drawing.Size(200, 20) };
        TextBox txtPreco = new TextBox() { Location = new System.Drawing.Point(120, 60), Size = new System.Drawing.Size(100, 20) };
        TextBox txtQuantidade = new TextBox() { Location = new System.Drawing.Point(120, 100), Size = new System.Drawing.Size(100, 20) };

 
        Button btnCadastrar = new Button();
        btnCadastrar.Text = "Cadastrar Produto";
        btnCadastrar.Location = new System.Drawing.Point(120, 140);
        btnCadastrar.Size = new System.Drawing.Size(120, 30);
        btnCadastrar.Click += (s, e) =>
        {
            if (ValidarCamposCadastro(txtNome.Text, txtPreco.Text, txtQuantidade.Text))
            {
                Produto novoProduto = new Produto(
                    txtNome.Text,
                    decimal.Parse(txtPreco.Text),
                    int.Parse(txtQuantidade.Text)
                );

                if (repository.CadastrarProduto(novoProduto))
                {
                    MessageBox.Show("Produto cadastrado com sucesso!", "Sucesso", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCamposCadastro(txtNome, txtPreco, txtQuantidade);
                    CarregarProdutos();
                }
            }
        };

        tabPage.Controls.AddRange(new Control[] { lblNome, lblPreco, lblQuantidade, 
                                                txtNome, txtPreco, txtQuantidade, btnCadastrar });
    }

    private void ConfigurarAbaEstoque(TabPage tabPage)
    {

        Label lblId = new Label() { Text = "ID do Produto:", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(100, 20) };
        Label lblQuantidade = new Label() { Text = "Quantidade:", Location = new System.Drawing.Point(20, 60), Size = new System.Drawing.Size(100, 20) };


        TextBox txtId = new TextBox() { Location = new System.Drawing.Point(120, 20), Size = new System.Drawing.Size(100, 20) };
        TextBox txtQuantidade = new TextBox() { Location = new System.Drawing.Point(120, 60), Size = new System.Drawing.Size(100, 20) };


        Button btnIncrementar = new Button();
        btnIncrementar.Text = "Incrementar Estoque";
        btnIncrementar.Location = new System.Drawing.Point(120, 100);
        btnIncrementar.Size = new System.Drawing.Size(120, 30);
        btnIncrementar.Click += (s, e) =>
        {
            if (ValidarCamposEstoque(txtId.Text, txtQuantidade.Text))
            {
                if (repository.IncrementarQuantidade(int.Parse(txtId.Text), int.Parse(txtQuantidade.Text)))
                {
                    MessageBox.Show("Estoque incrementado com sucesso!", "Sucesso", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCamposEstoque(txtId, txtQuantidade);
                    CarregarProdutos();
                }
            }
        };

        Button btnDecrementar = new Button();
        btnDecrementar.Text = "Decrementar Estoque";
        btnDecrementar.Location = new System.Drawing.Point(250, 100);
        btnDecrementar.Size = new System.Drawing.Size(120, 30);
        btnDecrementar.Click += (s, e) =>
        {
            if (ValidarCamposEstoque(txtId.Text, txtQuantidade.Text))
            {
                if (repository.DecrementarQuantidade(int.Parse(txtId.Text), int.Parse(txtQuantidade.Text)))
                {
                    MessageBox.Show("Estoque decrementado com sucesso!", "Sucesso", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCamposEstoque(txtId, txtQuantidade);
                    CarregarProdutos();
                }
            }
        };

  
        Label lblInfo = new Label();
        lblInfo.Text = "Observação: Use a aba de Listagem para ver os IDs dos produtos";
        lblInfo.Location = new System.Drawing.Point(20, 150);
        lblInfo.Size = new System.Drawing.Size(400, 40);
        lblInfo.ForeColor = Color.Blue;

        tabPage.Controls.AddRange(new Control[] { lblId, lblQuantidade, txtId, txtQuantidade, 
                                                btnIncrementar, btnDecrementar, lblInfo });
    }

    private void CarregarProdutos()
    {
        if (dataGridViewProdutos != null)
        {
            var produtos = repository.ListarProdutos();
            dataGridViewProdutos.DataSource = produtos;
            
   
            if (dataGridViewProdutos.Columns.Count > 0)
            {
                dataGridViewProdutos.Columns["IdProduto"].HeaderText = "ID";
                dataGridViewProdutos.Columns["Nome"].HeaderText = "Nome do Produto";
                dataGridViewProdutos.Columns["Preco"].HeaderText = "Preço";
                dataGridViewProdutos.Columns["Preco"].DefaultCellStyle.Format = "C2";
                dataGridViewProdutos.Columns["Quantidade"].HeaderText = "Quantidade em Estoque";
            }
        }
    }

    private bool ValidarCamposCadastro(string nome, string preco, string quantidade)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            MessageBox.Show("Por favor, informe o nome do produto.", "Aviso", 
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!decimal.TryParse(preco, out decimal precoValor) || precoValor <= 0)
        {
            MessageBox.Show("Por favor, informe um preço válido.", "Aviso", 
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(quantidade, out int quantidadeValor) || quantidadeValor < 0)
        {
            MessageBox.Show("Por favor, informe uma quantidade válida.", "Aviso", 
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    private bool ValidarCamposEstoque(string id, string quantidade)
    {
        if (!int.TryParse(id, out int idValor) || idValor <= 0)
        {
            MessageBox.Show("Por favor, informe um ID válido.", "Aviso", 
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(quantidade, out int quantidadeValor) || quantidadeValor <= 0)
        {
            MessageBox.Show("Por favor, informe uma quantidade válida.", "Aviso", 
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    private void LimparCamposCadastro(params TextBox[] textBoxes)
    {
        foreach (var txt in textBoxes)
        {
            txt.Clear();
        }
    }

    private void LimparCamposEstoque(params TextBox[] textBoxes)
    {
        foreach (var txt in textBoxes)
        {
            txt.Clear();
        }
    }

    private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        var tabControl = sender as TabControl;
        if (tabControl != null && tabControl.SelectedIndex == 0) // Aba de Listagem
        {
            CarregarProdutos();
        }
    }
}
