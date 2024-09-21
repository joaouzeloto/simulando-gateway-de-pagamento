using Microsoft.AspNetCore.Mvc;

namespace TrabalhoPrimeiroBim.controller
{
    [Route("pagamentos/")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        private readonly services.CartaoService _cartaoService;
        private readonly services.PagamentoService _pagamentoService;


        public PagamentosController(services.CartaoService cartaoService, services.PagamentoService pagamentoService)
        {
            _cartaoService = cartaoService;
            _pagamentoService = pagamentoService;
        }

        [HttpPost ("calcular-parcelas")]
        public List<domain.Parcela> calcularParcelas(viewModel.TransacaoViewModel transacaoInfo)
        {
            List<domain.Parcela> parcelas = new List<domain.Parcela>(); 
            double valorParcelas = transacaoInfo.valorTotal/transacaoInfo.qtdeParcelas;
            for(int i = 1; i <= transacaoInfo.qtdeParcelas; i++)
            {
                if (i > 1)
                    parcelas.Add(new domain.Parcela(i, valorParcelas + transacaoInfo.valorTotal * transacaoInfo.taxaJuros));
                else
                    parcelas.Add(new domain.Parcela(i, valorParcelas));
            }

            if(parcelas.Count > 0) 
                return parcelas;
            return null;
            
        }

        [HttpPost ("")]
        public IActionResult cadastrarPagamento(viewModel.PagamentoViewModel pagamentoView)
        {
            if(_cartaoService.validarCartao(pagamentoView.cartao))
            {
                domain.Pagamento transacao = new domain.Pagamento();
                transacao.cartao = pagamentoView.cartao;    
                transacao.cvv = pagamentoView.cvv;
                transacao.valor = pagamentoView.valor;
                transacao.qtdeParcelas = pagamentoView.qtdeParcelas;
                transacao.situacao = domain.Pagamento.Situacao.PENDENTE; 
                if(_pagamentoService.criarTransacao(transacao)>1)
                    return Ok(_pagamentoService.criarTransacao(transacao));
            }
                return BadRequest();
        }

        [HttpPost("/{id}/situacao")]
        public IActionResult situacaoPagamento(int id)
        {
            return Ok(_pagamentoService.situacaoPagamento(id));
        }

        [HttpPut("/{id}/confirmar")]
        public IActionResult confirmarPagamento(int id)
        {
            if (_pagamentoService.confirmarPagamento(id))
                return Ok("Pagamento confirmado!");
            return BadRequest("Pagamento não confirmado!");
        }

        [HttpPut("/{id}/cancelar")]
        public IActionResult cancelarrPagamento(int id)
        {
            if (_pagamentoService.cancelarPagamento(id))
                return Ok("Pagamento cancelado!");
            return BadRequest("Pagamento não cancelado!");
        }

    }
}
