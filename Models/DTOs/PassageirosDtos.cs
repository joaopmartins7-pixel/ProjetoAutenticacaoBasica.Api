namespace ProjetoAutenticacaoBasica.Api.Models.DTOs
{

    public class CriarPassageiroDto
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
    }

    public class PassageiroInfoDto
    {         
        public int Id { get; set; }
        public string Nome { get; set; }
    }

}
