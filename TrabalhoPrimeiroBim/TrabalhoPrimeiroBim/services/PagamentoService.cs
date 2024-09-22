using MySql.Data.MySqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static TrabalhoPrimeiroBim.domain.Pagamento;

namespace TrabalhoPrimeiroBim.services
{
    public class PagamentoService
    {
        private readonly ILogger<CartaoService> _logger;
        private readonly TrabalhoPrimeiroBim.BD _bd;
        private readonly domain.Pagamento _pagamento;

        public PagamentoService(ILogger<CartaoService> logger, BD bd, domain.Pagamento pagamento)
        {
            _logger = logger;
            _bd = bd;
            _pagamento = pagamento;
        }

        public long criarTransacao(domain.Pagamento pagamento)
        {
            _logger.LogInformation("iniciando o modulo de criação do pagamento");
            MySqlConnection conexao = _bd.CriarConexao();
            long id = 0;
            try
            {
                conexao.Open();
                MySqlCommand cmd = conexao.CreateCommand();
                Console.WriteLine(pagamento.situacao);
                cmd.CommandText = @$"INSERT INTO 
                                      Transacao(Valor, Cartao, CVV, Parcelas, Situacao) VALUES({pagamento.valor},{pagamento.cartao}, 
                                     {pagamento.cvv}, {pagamento.qtdeParcelas},{((uint)pagamento.situacao)});";
                cmd.ExecuteNonQuery();
                id = cmd.LastInsertedId;
                _logger.LogInformation("sucesso na criação do pagamento");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Erro a transação.");
                throw new Exception(ex.Message);
            }
            finally
            {
                conexao.Close();
            }

            return id;
        }

        public string situacaoPagamento(long id)
        {
            _logger.LogInformation("iniciando o modulo de consulta da situacao do pagamento");
            MySqlConnection conexao = _bd.CriarConexao();
            string situacao = "";
            try
            {
                conexao.Open();

                MySqlCommand cmd = conexao.CreateCommand();
                cmd.CommandText = @$"select * 
                                 from Transacao 
                                 where TransacaoId = {id}";

                var dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    situacao = dr["Situacao"].ToString();
                    _logger.LogInformation("sucesso em obter a situação");
                }
                else
                {
                    _logger.LogInformation("falha em obter a situação");
                    return "NÃO EXISTE";
                }
                    
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro a transação.");
                throw new Exception(ex.Message);
            }
            finally
            {
                conexao.Close();
            }

            return _pagamento.converte(situacao);
        }

        public bool confirmarPagamento(long id)
        {
            _logger.LogInformation("iniciando o modulo de update da situacao do pagamento");
            MySqlConnection conexao = _bd.CriarConexao();
            string situacao = "";
            bool flag = true;
            try
            {
                conexao.Open();
                MySqlCommand cmd = conexao.CreateCommand();
                cmd.CommandText = @$"select * 
                                 from Transacao 
                                 where TransacaoId = {id}";
                var dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    situacao = _pagamento.converte(dr["Situacao"].ToString());
                    dr.Close();
                    if (situacao != "CANCELADO")
                    {
                        cmd.CommandText = @$"UPDATE 
                                 Transacao SET Situacao = {2}
                                 WHERE TransacaoId = {id};";
                        dr = cmd.ExecuteReader();
                        _logger.LogInformation("update da situacao do pagamento realizado");
                    }
                    else
                    {
                        _logger.LogInformation("falha no update da situacao do pagamento realizado");
                        flag = false;
                    }
                    
                }

            }
            catch(Exception ex) 
            {
                 conexao.Close();
                _logger.LogError(ex, "Erro a transação.");
                throw new Exception(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
            return flag;
        }

        public bool cancelarPagamento(long id)
        {
            _logger.LogInformation("iniciando o modulo de update da situacao do pagamento");
            MySqlConnection conexao = _bd.CriarConexao();
            string situacao = "";
            bool flag = true;
            try
            {
                conexao.Open();
                MySqlCommand cmd = conexao.CreateCommand();
                cmd.CommandText = @$"select * 
                                 from Transacao 
                                 where TransacaoId = {id}";
                var dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    situacao = _pagamento.converte(dr["Situacao"].ToString());
                    dr.Close();
                    if (situacao != "CONFIRMADO")                 {
                        cmd.CommandText = @$"UPDATE 
                                 Transacao SET Situacao = {3}
                                 WHERE TransacaoId = {id};";
                        dr = cmd.ExecuteReader();
                        _logger.LogInformation("update da situacao do pagamento realizado");
                    }
                    else
                    {
                        _logger.LogInformation("falha no update da situacao do pagamento realizado");
                        flag = false;
                    }

                }

            }
            catch (Exception ex)
            {
                conexao.Close();
                _logger.LogError(ex, "Erro a transação.");
                throw new Exception(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
            return flag;
        }
    }
}
