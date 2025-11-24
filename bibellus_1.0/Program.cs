using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;

namespace bibellus
{
    class Program
    {
        static string conexaoString = "server=localhost;uid=root;pwd=;database=bibellus";

        static void Main(string[] args)
        {
            decimal aux = 0.00m;
            string opcao;
            decimal preco = 0.00m;
            string simounao;
            string pagamento;

        menu:
            Console.Clear();
            Console.WriteLine("Boas vindas ao sistema Bibellus!");
            Console.WriteLine("1 - Criar venda");
            Console.WriteLine("2 - Histórico de vendas");
            Console.WriteLine("3 - Estoque");
            Console.WriteLine("4 - Adicionar ao estoque");
            Console.WriteLine("5 - Remover do estoque");
            Console.WriteLine("q - sair");
            Console.Write("Digite uma opção: ");
            opcao = Console.ReadLine().ToLower();

            if (opcao == "1" || opcao == "criar venda" || opcao == "criar" || opcao == "criarvenda")
            {
                preco = 0.00m;

            opcao1:
                Console.Clear();
                Console.WriteLine("Qual o tamanho do item?\n(Casquinha, Pequeno, Médio, Grande, Gigante)");
                Console.Write(": ");
                string tamanho_pedido = Console.ReadLine().ToLower();

                if (tamanho_pedido == "casquinha" || tamanho_pedido == "1")
                {
                    preco += 4.50m;
                    Console.WriteLine("Valor: " + preco);
                }
                else if (tamanho_pedido == "pequeno" || tamanho_pedido == "2")
                {
                    preco += 13.00m;
                    Console.WriteLine("Valor: " + preco);
                }
                else if (tamanho_pedido == "medio" || tamanho_pedido == "médio" || tamanho_pedido == "3")
                {
                    preco += 18.00m;
                    Console.WriteLine("Valor: " + preco);
                }
                else if (tamanho_pedido == "grande" || tamanho_pedido == "4")
                {
                    preco += 23.00m;
                    Console.WriteLine("Valor: " + preco);
                }
                else if (tamanho_pedido == "gigante" || tamanho_pedido == "5")
                {
                    preco += 28.00m;
                    Console.WriteLine("Valor: " + preco);
                }
                else
                {
                    Console.WriteLine("ERRO DE DIGITAÇÃO");
                    Console.ReadKey();
                    goto opcao1;
                }

            additem:
                Console.WriteLine("Deseja adicionar mais algum item? | Sim | Não |");
                simounao = Console.ReadLine().ToLower();

                if (simounao == "s" || simounao == "sim")
                {
                    goto opcao1;
                }
                else if (simounao == "n" || simounao == "nao")
                {
                    Console.Clear();
                    Console.WriteLine("Ok. Indo ao tipo de pagamento...");
                    Console.ReadKey();
                    goto tipopagamento;
                }
                else
                {
                    Console.WriteLine("ERRO DE DIGITAÇÃO");
                    goto additem;
                }

            tipopagamento:
                Console.Clear();
                Console.WriteLine("Preço total: " + preco);
                Console.WriteLine("Escolha a forma de pagamento: Pix, Credito, Debito");
                pagamento = Console.ReadLine().ToLower();

                if (pagamento != "pix" && pagamento != "credito" && pagamento != "debito")
                {
                    Console.WriteLine("ERRO DE DIGITAÇÃO");
                    goto tipopagamento;
                }

                try
                {
                    using (var con = new MySqlConnection(conexaoString))
                    {
                        con.Open();
                        string sql = "INSERT INTO vendas (data, hora, pagamento, valor, observacao) VALUES (CURDATE(), CURTIME(), @p, @v, '')";
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@p", pagamento);
                        cmd.Parameters.AddWithValue("@v", preco);
                        cmd.ExecuteNonQuery();
                    }

                    Console.WriteLine("VENDA REGISTRADA COM SUCESSO!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERRO AO SALVAR VENDA: " + e.Message);
                }

                Console.ReadKey();
                goto menu;
            }

            if (opcao == "2")
            {
                Console.Clear();
                Console.WriteLine("HISTÓRICO DE VENDAS");

                try
                {
                    using (var con = new MySqlConnection(conexaoString))
                    {
                        con.Open();
                        string sql = "SELECT id, data, pagamento, valor FROM vendas ORDER BY id DESC";
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        MySqlDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            Console.WriteLine(
                                $"ID {r["id"]} | Data: {r["data"]} | Pagamento: {r["pagamento"]} | Valor: {r["valor"]}"
                            );
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERRO AO CARREGAR HISTÓRICO: " + e.Message);
                }

                Console.ReadKey();
                goto menu;
            }

            if (opcao == "3")
            {
                Console.Clear();
                Console.WriteLine("ESTOQUE");

                try
                {
                    using (var con = new MySqlConnection(conexaoString))
                    {
                        con.Open();
                        string sql =
@"SELECT produto.id AS idProduto, produto.descricao AS prod, unidade.descricao AS un, quantidade
FROM estoque
JOIN produto ON produto.id = estoque.id_produto_fk
JOIN unidade ON unidade.id = estoque.id_unidade_fk";

                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        var r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            Console.WriteLine($"ID {r["idProduto"]}: {r["prod"]} ({r["un"]}) -> {r["quantidade"]}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERRO AO MOSTRAR ESTOQUE: " + e.Message);
                }

                Console.ReadKey();
                goto menu;
            }

            if (opcao == "4")
            {
                Console.Clear();
                Console.WriteLine("ADICIONAR AO ESTOQUE");
                Console.WriteLine("\nLISTA DE PRODUTOS (ID - Descrição):\n");

                try
                {
                    using (var con = new MySqlConnection(conexaoString))
                    {
                        con.Open();
                        string sql = "SELECT id, descricao FROM produto ORDER BY id";
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        var r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            Console.WriteLine($"{r["id"]} - {r["descricao"]}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERRO AO CARREGAR PRODUTOS: " + e.Message);
                }

                Console.WriteLine("\n");

                Console.Write("ID do produto: ");
                int idp = int.Parse(Console.ReadLine());

                Console.Write("Quantidade a adicionar: ");
                int qtd = int.Parse(Console.ReadLine());

                try
                {
                    using (var con = new MySqlConnection(conexaoString))
                    {
                        con.Open();
                        string sql = "UPDATE estoque SET quantidade = quantidade + @q, atualizacao = CURDATE() WHERE id_produto_fk = @id";
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@q", qtd);
                        cmd.Parameters.AddWithValue("@id", idp);

                        int linhas = cmd.ExecuteNonQuery();

                        if (linhas > 0)
                            Console.WriteLine("ESTOQUE ATUALIZADO!");
                        else
                            Console.WriteLine("ID não encontrado.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERRO AO ATUALIZAR ESTOQUE: " + e.Message);
                }

                Console.ReadKey();
                goto menu;
            }

            if (opcao == "5")
            {
                Console.Clear();
                Console.WriteLine("REMOVER DO ESTOQUE");

                try
                {
                    using (var con = new MySqlConnection(conexaoString))
                    {
                        con.Open();
                        string sql =
            @"SELECT produto.id AS idProduto, produto.descricao AS prod, unidade.descricao AS un, quantidade
FROM estoque
JOIN produto ON produto.id = estoque.id_produto_fk
JOIN unidade ON unidade.id = estoque.id_unidade_fk";

                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        var r = cmd.ExecuteReader();

                        Console.WriteLine("ID | Produto | Unidade | Quantidade\n");

                        while (r.Read())
                        {
                            Console.WriteLine(
                                $"ID {r["idProduto"]}: {r["prod"]} ({r["un"]}) -> {r["quantidade"]}"
                            );
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERRO AO MOSTRAR ESTOQUE: " + e.Message);
                }

                Console.WriteLine();
                Console.Write("ID do produto que deseja remover: ");
                int idpRemove = int.Parse(Console.ReadLine());

                Console.Write("Quantidade a remover: ");
                int qtdRemove = int.Parse(Console.ReadLine());

                try
                {
                    using (var con = new MySqlConnection(conexaoString))
                    {
                        con.Open();

                        string busca = "SELECT quantidade FROM estoque WHERE id_produto_fk = @id";
                        MySqlCommand cmdBusca = new MySqlCommand(busca, con);
                        cmdBusca.Parameters.AddWithValue("@id", idpRemove);

                        object result = cmdBusca.ExecuteScalar();

                        if (result == null)
                        {
                            Console.WriteLine("ID não encontrado!");
                            Console.ReadKey();
                            goto menu;
                        }

                        int quantidadeAtual = Convert.ToInt32(result);

                        if (quantidadeAtual < qtdRemove)
                        {
                            Console.WriteLine("ERRO: Não é possível remover mais itens do que o estoque possui!");
                            Console.ReadKey();
                            goto menu;
                        }

                        string sql =
                            "UPDATE estoque SET quantidade = quantidade - @q, atualizacao = CURDATE() WHERE id_produto_fk = @id";

                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@q", qtdRemove);
                        cmd.Parameters.AddWithValue("@id", idpRemove);

                        cmd.ExecuteNonQuery();

                        Console.WriteLine("ITEM REMOVIDO COM SUCESSO!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERRO AO REMOVER DO ESTOQUE: " + e.Message);
                }

                Console.ReadKey();
                goto menu;
            }

            if (opcao == "q" || opcao == "sair")
            {
                goto fim;
            }

            Console.WriteLine("Opção inválida!");
            Console.ReadKey();
            goto menu;

        fim:
            Console.WriteLine("Encerrando...");
        }
    }
}
