﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senai.SpMedicalGroup.WebApi.Domains;
using Senai.SpMedicalGroup.WebApi.Interfaces;
using Senai.SpMedicalGroup.WebApi.Repositorios;

namespace Senai.SpMedicalGroup.WebApi.Controllers
{
    [Authorize(Roles = "1")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProntuariosController : ControllerBase
    {
        private IProntuariosRepositorio ProntuariosRepositorio { get; set; }

        public ProntuariosController()
        {
            ProntuariosRepositorio = new ProntuariosRepositorio();
        }

        // Lista todos os Prontuarios
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(ProntuariosRepositorio.Listar());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // Listar Prontuarios com usuarios
        [HttpGet("ProntuariosInclude")]
        public IActionResult GetProntuariosInclude()
        {
            try
            {
                return Ok(ProntuariosRepositorio.ListarProntuariosInclude());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // Lista todos os Prontuarios somente com id o nome
        [HttpGet("SelectProntuarios")]
        public IActionResult GetProntuarios()
        {
            try
            {
                using (SpMedicalGroupContext ctx = new SpMedicalGroupContext())
                {
                    return Ok(ctx.Prontuarios.Select(x => new { x.Id, x.Nome }).ToList());
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // retorna a quantidade de Prontuarios cadastradas
        [HttpGet("Count")]
        public IActionResult GetCount()
        {
            try
            {
                List<Prontuarios> prontuarios = ProntuariosRepositorio.Listar();

                return Ok(prontuarios.Count);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // Busca um unico Prontuario
        [HttpGet("{prontuarioId}")]
        public IActionResult Get(int prontuarioId)
        {
            try
            {
                Prontuarios prontuarioBuscado = ProntuariosRepositorio.BuscarProntuario(prontuarioId);

                if (prontuarioBuscado == null)
                {
                    return NotFound(new { mensagem = "Prontuario não encontrado!" });
                }

                return Ok(prontuarioBuscado);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // Cadastra um Prontuario
        [HttpPost]
        public IActionResult Post(Prontuarios prontuarioRecebido)
        {
            try
            {
                ProntuariosRepositorio.Cadastrar(prontuarioRecebido);

                return Ok(prontuarioRecebido);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // Atualiza um Prontuario
        [HttpPut]
        public IActionResult Put(Prontuarios prontuarioRecebido)
        {
            try
            {
                Prontuarios prontuarioBuscado = ProntuariosRepositorio.BuscarProntuario(prontuarioRecebido.Id);

                if (prontuarioBuscado == null)
                {
                    return NotFound(new { mensagem = "Prontuario não encontrado!"});
                }

                ProntuariosRepositorio.Alterar(prontuarioRecebido);

                return Ok(prontuarioRecebido);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // Deleta um Prontuario
        [HttpDelete("{prontuarioId}")]
        public IActionResult Delete(int prontuarioId)
        {
            try
            {
                Prontuarios prontuarioBuscado = ProntuariosRepositorio.BuscarProntuario(prontuarioId);

                if (prontuarioBuscado == null)
                {
                    return NotFound(new { mensagem = "Prontuario não encontrada!" });
                }

                ProntuariosRepositorio.Deletar(prontuarioBuscado);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}