namespace TrabalhoPrimeiroBim.viewModel
{
    public class TransacaoViewModel
    {
        /// <summary>
        /// Valor total da transação
        /// </summary>
        public  double valorTotal { get; set; }
        /// <summary>
        /// Taxa de juros da compra
        /// </summary>
        public double taxaJuros { get; set; }
        /// <summary>
        /// Quantiddade de parcelas escolhidas
        /// </summary>
        public int qtdeParcelas { get; set; }
    }
}
