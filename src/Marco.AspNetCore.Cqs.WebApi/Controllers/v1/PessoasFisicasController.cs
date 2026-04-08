using arco.AspNetCore.Cqs.Infra.Data.Dapper.CQS.Queries;
using AutoMapper;
using Marco.AspNetCore.Cqs.Application.Commands;
using Marco.AspNetCore.Cqs.Domain.Models;
using Marco.AspNetCore.Cqs.WebApi.Models.v1;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Marco.AspNetCore.Cqs.WebApi.Controllers.v1
{
    [ApiController]
    [AllowAnonymous]
    [Route("")]
    public class PessoasFisicasController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PessoasFisicasController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("ConsultarViaCommand/{cpf}")]
        [ProducesResponseType(typeof(PessoaFisicaGetResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByCpfCommandAsync([FromRoute] string cpf)
        {
            var command = new ConsultarPessoaFisicaPorCpfCommand(cpf);
            var pessoaFisica = await _mediator.Send(command);

            if (pessoaFisica is null)
                return NotFound();

            return Ok(_mapper.Map<PessoaFisicaGetResult>(pessoaFisica));
        }

        [HttpGet("ConsultarViaQuery/{cpf}")]
        [ProducesResponseType(typeof(PessoaFisicaGetResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByCpfQueryAsync([FromRoute] string cpf)
        {
            var query = new ConsultarPessoaFisicaPorCpfQuery(cpf);
            var pessoaFisica = await _mediator.Send(query);

            if (pessoaFisica is null)
                return NotFound();

            return Ok(_mapper.Map<PessoaFisicaGetResult>(pessoaFisica));
        }
    }
}
