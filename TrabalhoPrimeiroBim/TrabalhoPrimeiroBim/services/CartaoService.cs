using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace TrabalhoPrimeiroBim.services
{
    public class CartaoService
    {
        public readonly ILogger<CartaoService> _logger;
        public readonly TrabalhoPrimeiroBim.BD _bd;

        public CartaoService(ILogger<CartaoService> logger, BD bd)
        {
            _logger = logger;
            _bd = bd;
        }

        public bool validarCartao(string numero)
        {
            _logger.LogInformation("iniciando o modulo de validação do cartão");
            
            MySqlConnection conexao = _bd.CriarConexao();
            try
            {
                conexao.Open();

                MySqlCommand cmd = conexao.CreateCommand();

                cmd.CommandText = @$"select * 
                                 from Cartao 
                                 where numero = {numero}";

                var dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    DateTime validadeCartao;
                    dr.Read();
                    if (DateTime.TryParse(dr["validade"].ToString(), out validadeCartao))
                    {
                        if (validadeCartao > DateTime.Now)
                        {
                            _logger.LogInformation("cartão validado - pronto para uso");
                            conexao.Close();
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro a transação.");
                throw new Exception(ex.Message);
            }
            _logger.LogInformation("cartão validado - não pode ser usado");
            conexao.Close();
            return false;
           

        }
    }
}
