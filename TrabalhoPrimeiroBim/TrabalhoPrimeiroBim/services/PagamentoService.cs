using MySql.Data.MySqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static TrabalhoPrimeiroBim.domain.Pagamento;

namespace TrabalhoPrimeiroBim.services
{
    public class PagamentoService
    {
        private readonly ILogger<CartaoService> _logger;
        private readonly TrabalhoPrimeiroBim.BD _bd;

        public PagamentoService(ILogger<CartaoService> logger, BD bd)
        {
            _logger = logger;
            _bd = bd;
        }

        public long criarTransacao(domain.Pagamento pagamento)
        {
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
            MySqlConnection conexao = _bd.CriarConexao();
            domain.Pagamento aux = new domain.Pagamento();
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
                }
                else
                    return "NÃO EXISTE";
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

            return aux.converte(situacao);
        }

        public bool confirmarPagamento(long id)
        {
            MySqlConnection conexao = _bd.CriarConexao();
            string situacao = "";
            bool flag = true;
            domain.Pagamento aux = new domain.Pagamento();
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
                    situacao = aux.converte(dr["Situacao"].ToString());
                    dr.Close();
                    if (situacao != "CANCELADO")
                    {
                        cmd.CommandText = @$"UPDATE 
                                 Transacao SET Situacao = {2}
                                 WHERE TransacaoId = {id};";
                        dr = cmd.ExecuteReader();
                    }
                    else
                        flag = false;
                    
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
            MySqlConnection conexao = _bd.CriarConexao();
            string situacao = "";
            bool flag = true;
            domain.Pagamento aux = new domain.Pagamento();
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
                    situacao = aux.converte(dr["Situacao"].ToString());
                    dr.Close();
                    if (situacao != "CONFIRMADO")                 {
                        cmd.CommandText = @$"UPDATE 
                                 Transacao SET Situacao = {3}
                                 WHERE TransacaoId = {id};";
                        dr = cmd.ExecuteReader();
                    }
                    else
                        flag = false;

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
