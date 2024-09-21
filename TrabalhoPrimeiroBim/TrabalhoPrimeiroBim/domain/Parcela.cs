namespace TrabalhoPrimeiroBim.domain
{
    public class Parcela
    {
        public int nParcela { get; set; }

        public double valor { get; set; }

        public Parcela (int nParcela, double valor)
        {
            this.nParcela = nParcela;
            this.valor = valor; 
        }
    }
}
