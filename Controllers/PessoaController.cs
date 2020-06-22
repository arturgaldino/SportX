using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SportsX.Controllers
{
    [ApiController]
    [Route("Pessoa")]
    public class PessoaController : ControllerBase
    {
        
        private string sqlCon = @"Data Source=(LocalDB)\MSSQLLocalDB;
                        AttachDbFilename=" + System.IO.Directory.GetCurrentDirectory() + @"\Properties\localDB.mdf;
                        Integrated Security=True;Connect Timeout=30";
        private string insertPessoaFisica = "INSERT INTO Pessoa (nome,email,classificacao,cep,cpf) VALUES (@nome, @email, @classificacao, @cep, @cpf);";
        private string insertPessoaJuridica = "INSERT INTO Pessoa (nome,razaosocial,email,classificacao,cep,cnpj) VALUES (@nome,@razaosocial, @email, @classificacao, @cep, @cnpj);";
        private string selectAllPessoaFisica = "SELECT id,nome,email,classificacao,cep,cpf FROM Pessoa WHERE cpf is not null;";
        private string selectAllPessoaJuridica = "SELECT id,nome,email,classificacao,razaoSocial,cep,cnpj FROM Pessoa WHERE cnpj is not null;";
        private string deletePessoa = "DELETE FROM PESSOA WHERE id =@id";
        private readonly ILogger<PessoaController> _logger;

        public PessoaController(ILogger<PessoaController> logger)
        {
            _logger = logger;
        }

        [HttpGet ("recuperaPessoasFisica")]
        public List<Pessoa> recuperaPessoasFisica()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            SqlConnection con = new SqlConnection(sqlCon);

            SqlCommand cmd = new SqlCommand(selectAllPessoaFisica, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Pessoa pessoaAux = new Pessoa();
                    pessoaAux.id = reader.GetInt32(0);
                    pessoaAux.nome = reader.IsDBNull(1) ? null : reader.GetString(1).Trim();
                    pessoaAux.email = reader.IsDBNull(2) ? null : reader.GetString(2).Trim();
                    pessoaAux.classificacao = reader.IsDBNull(3) ? null : reader.GetString(3).Trim();
                    if (!reader.IsDBNull(4)){
                        pessoaAux.cep = (int) reader.GetDecimal(4);
                    }
                    if(!reader.IsDBNull(5)){
                        pessoaAux.cpf = (long) reader.GetDecimal(5);
                    }  
                    pessoas.Add(pessoaAux);
                }
            }
            return pessoas;
        }

        [HttpGet ("recuperaPessoasJuridica")]
        public List<Pessoa> recuperaPessoasJuridica()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            SqlConnection con = new SqlConnection(sqlCon);

            SqlCommand cmd = new SqlCommand(selectAllPessoaJuridica, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Pessoa pessoaAux = new Pessoa();
                    pessoaAux.id = reader.GetInt32(0);
                    pessoaAux.nome = reader.IsDBNull(1) ? null : reader.GetString(1).Trim();
                    pessoaAux.email = reader.IsDBNull(2) ? null : reader.GetString(2).Trim();
                    pessoaAux.classificacao = reader.IsDBNull(3) ? null : reader.GetString(3).Trim();
                    pessoaAux.razaoSocial = reader.IsDBNull(4) ? null : reader.GetString(4).Trim();
                    if (!reader.IsDBNull(5)){
                        pessoaAux.cep = (int)reader.GetDecimal(5);
                    }
                    if (!reader.IsDBNull(6)){
                        pessoaAux.cnpj = (long)reader.GetDecimal(6);
                    }
                    pessoas.Add(pessoaAux);
                }
            }
            return pessoas;
        }

        [HttpPost ("cadastraPessoaFisica")]
        public void cadastraPessoaFisica([FromBody]Pessoa pessoa)
        {
            SqlConnection con = new SqlConnection(sqlCon);

            SqlCommand cmd = new SqlCommand(insertPessoaFisica, con);
            cmd.Parameters.Add("@nome", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@nome"].Value = pessoa.nome;

            cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@email"].Value = pessoa.email;

            cmd.Parameters.Add("@classificacao", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@classificacao"].Value = pessoa.classificacao;

            cmd.Parameters.Add("@cep", System.Data.SqlDbType.Decimal);
            cmd.Parameters["@cep"].Value = pessoa.cep;

            cmd.Parameters.Add("@cpf", System.Data.SqlDbType.Decimal);
            cmd.Parameters["@cpf"].Value = pessoa.cpf;

            con.Open();
            cmd.ExecuteNonQuery();

        }

        [HttpPost("cadastraPessoaJuridica")]
        public void cadastraPessoaJuridica([FromBody]Pessoa pessoa)
        {
            SqlConnection con = new SqlConnection(sqlCon);

            SqlCommand cmd = new SqlCommand(insertPessoaJuridica, con);
            cmd.Parameters.Add("@nome", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@nome"].Value = pessoa.nome;

            cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@email"].Value = pessoa.email;

            cmd.Parameters.Add("@classificacao", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@classificacao"].Value = pessoa.classificacao;

            cmd.Parameters.Add("@razaoSocial", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@razaoSocial"].Value = pessoa.razaoSocial;

            cmd.Parameters.Add("@cep", System.Data.SqlDbType.Decimal);
            cmd.Parameters["@cep"].Value = pessoa.cep;

            cmd.Parameters.Add("@cnpj", System.Data.SqlDbType.Decimal);
            cmd.Parameters["@cnpj"].Value = pessoa.cnpj;

            con.Open();
            cmd.ExecuteNonQuery();

        }

        [HttpPost("apagarPessoa")]
        public void apagarPessoa([FromBody]Pessoa pessoa)
        {
            SqlConnection con = new SqlConnection(sqlCon);

            SqlCommand cmd = new SqlCommand(deletePessoa, con);
            cmd.Parameters.Add("@id", System.Data.SqlDbType.Int);
            cmd.Parameters["@id"].Value = pessoa.id;

            con.Open();
            cmd.ExecuteNonQuery();
        }

    }
}
