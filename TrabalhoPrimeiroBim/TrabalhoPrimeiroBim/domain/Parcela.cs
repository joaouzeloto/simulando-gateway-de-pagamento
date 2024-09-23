namespace TrabalhoPrimeiroBim.domain
{
    public class Parcela
    {
        /// <summary>
        /// Quantidade de parcelas escolhidas para se dividir
        /// </summary>
        public int nParcela { get; set; }

        /// <summary>
        /// Valor total da compra
        /// </summary>
        public double valor { get; set; }


        public Parcela (int nParcela, double valor)
        {
            this.nParcela = nParcela;
            this.valor = valor; 
        }
    }
}
