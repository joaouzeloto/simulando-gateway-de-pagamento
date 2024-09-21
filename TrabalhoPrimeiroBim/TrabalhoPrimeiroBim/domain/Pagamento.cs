namespace TrabalhoPrimeiroBim.domain
{
    public class Pagamento
    {
        public double valor { get; set; }
        public string cartao { get; set; }
        public int cvv { get; set; }
        public int qtdeParcelas { get; set; }
        public Situacao situacao { get; set; }

        public enum Situacao
        {
            PENDENTE = 1, //1
            CONFIRMADO = 2,//2
            CANCELADO = 3 //3
        }

        public string converte(string valor)
        {
            int n = int.Parse(valor);
            Situacao situacao = (Situacao) n;
            return situacao.ToString();
        }
    }
}
