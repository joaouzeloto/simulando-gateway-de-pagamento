
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using XXXXX;

namespace TrabalhoPrimeiroBim.controller
{
    [XXXXX.Authorize("APIAuth")]
    [Route("cartoes/")]
    [ApiController]
    public class CartaoController : ControllerBase
    {

        private readonly services.CartaoService _cartaoService;

        public CartaoController(services.CartaoService cartaoService)
        {
            _cartaoService = cartaoService;
        }

        /// <summary>
        /// devolve a bandeira do cartão
        /// </summary>
        /// <param name="cartao"></param>
        /// <returns></returns>
        [HttpGet ("{cartao}/obter-bandeira")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult obterBandeira(string cartao)
        {
            cartao.Replace("-", "");
            bool flag = true;
            string bandeira = "";
            for(int i = 0; i < 3;i++)
                if (cartao[i] != cartao[i+1])
                    flag = false;
            if (flag && cartao[0] == cartao[8])
            {
                switch (cartao[0])
                {
                    case '1':
                        bandeira = "VISA";
                        break;
                    case '2':
                        bandeira = "MASTERCARD";
                        break;
                    case '3':
                        bandeira = "ELO";
                        break;
                    default:
                        return BadRequest("bandeira não reconhecida!");
                        break;
                }
                return Ok(bandeira);
            }
            return BadRequest("Cartão não é válido");
            
        }


        /// <summary>
        /// confere se o cartão está válido
        /// </summary>
        /// <param name="cartao"></param>
        /// <returns></returns>
        [HttpGet("{cartao}/valido")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public bool conferirValidade(string cartao)
        {
            return _cartaoService.validarCartao(cartao);
        }
    }

}
