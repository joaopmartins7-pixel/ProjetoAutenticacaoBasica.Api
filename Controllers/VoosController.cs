using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoAutenticacaoBasica.Api.Data;
using ProjetoAutenticacaoBasica.Api.Models;
using ProjetoAutenticacaoBasica.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ProjetoAutenticacaoBasica.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VoosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CriarVoo(CriarVooDto dto)
        {
            var novoVoo = new Voo
            {
                Codigo = dto.Codigo,
                Origem = dto.Origem,
                Destino = dto.Destino
            };

            _context.Voos.Add(novoVoo);
            await _context.SaveChangesAsync();
            return Ok("Voo criado com sucesso!");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResumoVooDto>>> GetVoos()
        {
            var voos = await _context.Voos
                .AsNoTracking()
                .OrderBy(v => v.Codigo)
                .Select(v => new ResumoVooDto
                {
                    Id = v.Id,
                    Codigo = v.Codigo,
                    Trajeto = v.Origem + " -> " + v.Destino
                })
                .ToListAsync();

            return Ok(voos);
        }

        [HttpGet("codigo/{codigo}")]
        public async Task<ActionResult<VooManifestoDto>> GetVooPorCodigo(int codigo)
        {
            var manifesto = await _context.Voos
                .AsNoTracking()
                .Where(v => v.Codigo == codigo)
                .Select(v => new VooManifestoDto
                {
                    Id = v.Id,
                    Codigo = v.Codigo,
                    Trajeto = v.Origem + " -> " + v.Destino,
                    Passageiro = v.Passagens .Select(p => new PassageiroNoVooDto
                    {
                        Nome = p.Passageiro.Nome,
                        Assento = p.Assento
                    })
                        .ToList()
                }).FirstOrDefaultAsync();

            if (manifesto is null)
            {
                return NotFound("Voo não encontrado");
            }
            return Ok(manifesto);
        }
    }
}
