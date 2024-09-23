namespace TrabalhoPrimeiroBim.viewModel
{
    public class PagamentoViewModel
    {
        /// <summary>
        /// Valor total do pagamento
        /// </summary>
        public double valor { get; set; }
        /// <summary>
        /// Número do cartão de crédito
        /// </summary>
        public string cartao { get; set; }
        /// <summary>
        /// Número de segurança do cartão
        /// </summary>
        public int cvv { get; set; }
        /// <summary>
        /// Quantidades de parcelas do pagamento
        /// </summary>
        public int qtdeParcelas { get; set; }
        
        


    }
}
