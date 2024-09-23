namespace TrabalhoPrimeiroBim.domain
{
    public class Pagamento
    {
        /// <summary>
        /// Valor total da compra
        /// </summary>
        public double valor { get; set; }
        /// <summary>
        /// Numero do cartão de crédito
        /// </summary>
        public string cartao { get; set; }
        /// <summary>
        /// Código de segurança do cartão
        /// </summary>
        public int cvv { get; set; }
        /// <summary>
        /// Quantidade de parcelas escolhida
        /// </summary>
        public int qtdeParcelas { get; set; }
        /// <summary>
        /// Status do pagamento
        /// </summary>
        public Situacao situacao { get; set; }

        public enum Situacao
        {
            PENDENTE = 1, //1
            CONFIRMADO = 2,//2
            CANCELADO = 3 //3
        }

        /// <summary>
        /// Função usada para converter valores do BD
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public string converte(string valor)
        {
            int n = int.Parse(valor);
            Situacao situacao = (Situacao) n;
            return situacao.ToString();
        }
    }
}
