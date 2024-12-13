using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Minimal_API.Entities;
using Minimal_API.Context;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data;

namespace Minimal_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly ContatoContext _context;

        public ContatoController(ContatoContext context)
        {
            _context = context;
        }


        [HttpPost("criar")]
        public IActionResult CriarContato(Contato contato)
        {

            if (contato.DataDeRegistro == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data de registro não pode ser vazia." });
            }
            
            bool contatoExiste = _context.Contatos.Any(c => c.Nome == contato.Nome || c.Numero == contato.Numero);
            if (contatoExiste)
            {
            return Conflict(new { Erro = "Esse número já existe." });
            }
            
            _context.Add(contato);
            _context.SaveChanges();
            
            return CreatedAtAction(nameof(contato), new { id = contato.Id }, contato);
        }

        [HttpDelete("deletar")]
        public IActionResult DeletarContato(int id, string nome)
        {

            var contatoId = _context.Contatos.FirstOrDefault(c => c.Id == id);

            var contatoNome = _context.Contatos.FirstOrDefault(c => c.Nome == nome && c.Id != id);

            if (contatoId == null && contatoNome == null)
            {
                return NotFound(new { Erro = "Contato não encontrado." });
            }

            if (contatoId != null)
            {
                _context.Contatos.Remove(contatoId);
            }

            if (contatoNome != null)
            {
                _context.Contatos.Remove(contatoNome);
            }

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("atualizar")]
        public IActionResult AtualizarContato(int id, string nome, Contato contato)
        {
            var contatoId = _context.Contatos.FirstOrDefault(c => c.Id == id);

            var contatoNome = _context.Contatos.FirstOrDefault(c => c.Nome == nome && c.Id != id);

            bool contatoExiste = _context.Contatos.Any(c => c.Numero == contato.Numero);

            if (contato.DataDeRegistro == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data de registro não pode ser vazia." });
            }

            if (contatoId == null && contatoNome == null)
            {
                return NotFound(new { Erro = "Contato não encontrado." });
            }

            if(contatoExiste)
            {
                return BadRequest(new { Erro = "Esse número já existe." });
            }

            if (contatoId != null && !contatoExiste)
            {
                contatoId.Nome = contato.Nome;
                contatoId.Numero = contato.Numero;
                contatoId.Status = contato.Status;
                contatoId.DataDeRegistro = contato.DataDeRegistro;

                _context.Contatos.Update(contatoId);
            }

            if (contatoNome != null && !contatoExiste)
            {
                contatoNome.Nome = contato.Nome;
                contatoNome.Numero = contato.Numero;
                contatoNome.Status = contato.Status;
                contatoNome.DataDeRegistro = contato.DataDeRegistro;

                _context.Contatos.Update(contatoNome);
            }

            _context.SaveChanges();
            return CreatedAtAction(nameof(contato), new { id = contato.Id }, contato);
            
        }

        [HttpGet("contatos")]
        public IActionResult BuscarContatos()
        {
            var contatos = _context.Contatos;
            return Ok(contatos);
        }

        [HttpGet("busca")]
        public IActionResult Buscarespecifico(int id, string nome, string numero, EnumStatusContato status)
        {
            var contatoId = _context.Contatos.Find(id);

            var contatoNome = _context.Contatos.FirstOrDefault(c => c.Nome.Contains(nome));

            var contatoNumero = _context.Contatos.FirstOrDefault(c => c.Numero.Contains(numero));
            
            var contatoStatus = _context.Contatos.Where(c => c.Status == status);

            if (contatoId != null)
            {
                return Ok(contatoId);
            }
            if (contatoNome != null)
            {
                return Ok(contatoNome);
            }
            if (contatoNumero != null)
            {
                return Ok(contatoNumero);
            }
            if (contatoStatus != null)
            {
                return Ok(contatoStatus);
            }
            return NotFound(new { Erro = "Contato não encontrado." });
        }
    }
}

 