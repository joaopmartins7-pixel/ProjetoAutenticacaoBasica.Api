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
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
           var novoUsuario = new Usuario
           {
               Nome = registerDto.Nome,
               Username = registerDto.Username,
               Password = registerDto.Password,
               CreatedAt = DateTime.UtcNow
           };
            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();
            return Ok("Usuário criado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.Password == loginDto.Password);
            if (usuario == null )
            {
                return NotFound("Credenciais inválidas.");
            }

            return Ok(new { Nome = usuario.Nome });
        }
    }
}
