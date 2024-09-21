using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace TrabalhoPrimeiroBim.services
{
    public class CartaoService
    {
        private readonly ILogger<CartaoService> _logger;
        private readonly TrabalhoPrimeiroBim.BD _bd;

        public CartaoService(ILogger<CartaoService> logger, BD bd)
        {
            _logger = logger;
            _bd = bd;
        }

        public bool validarCartao(string numero)
        {

           
            MySqlConnection conexao = _bd.CriarConexao();

            conexao.Open();

            MySqlCommand cmd = conexao.CreateCommand();

            cmd.CommandText = @$"select * 
                                 from Cartao 
                                 where numero = {numero}";

            var dr = cmd.ExecuteReader();

            if(dr.HasRows)
            {
                DateTime validadeCartao;
                dr.Read();
                if (DateTime.TryParse(dr["validade"].ToString(), out validadeCartao))
                {
                    if (validadeCartao > DateTime.Now)
                    {
                        conexao.Close();
                        return true; 
                    }
                }
            }

            conexao.Close();
            return false;
           

        }
    }
}
