using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XXXXX;

namespace TrabalhoPrimeiroBim.controller
{
    [XXXXX.Authorize("APIAuth")]
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

        /// <summary>
        /// calcula as parcelas de um pagamento
        /// </summary>
        /// <param name="transacaoInfo"></param>
        /// <returns></returns>
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

        /// <summary>
        /// cria um novo pagamento
        /// </summary>
        /// <param name="pagamentoView"></param>
        /// <returns></returns>
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
                long i = _pagamentoService.criarTransacao(transacao);
                if (i>1)
                    return Ok(i);
            }
            return BadRequest();
        }

        /// <summary>
        /// devolve a situacao de determinado pagamento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/{id}/situacao")]
        public IActionResult situacaoPagamento(int id)
        {
            return Ok(_pagamentoService.situacaoPagamento(id));
        }

        /// <summary>
        /// atualiza a situacao de determinado pagamento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("/{id}/confirmar")]
        public IActionResult confirmarPagamento(int id)
        {
            if (_pagamentoService.confirmarPagamento(id))
                return Ok("Pagamento confirmado!");
            return BadRequest("Pagamento não confirmado!");
        }

        /// <summary>
        /// atualiza a situacao de determinado pagamento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("/{id}/cancelar")]
        public IActionResult cancelarrPagamento(int id)
        {
            if (_pagamentoService.cancelarPagamento(id))
                return Ok("Pagamento cancelado!");
            return BadRequest("Pagamento não cancelado!");
        }

    }
}
