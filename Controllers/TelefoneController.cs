using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SportsX.Controllers
{
    [ApiController]
    [Route("Telefone")]
    public class TelefoneController : ControllerBase
    {
        
        private string sqlCon = @"Data Source=(LocalDB)\MSSQLLocalDB;
                        AttachDbFilename=" + System.IO.Directory.GetCurrentDirectory() + @"\Properties\localDB.mdf;
                        Integrated Security=True;Connect Timeout=30";
        private string insertTelefone = "INSERT INTO Telefones (idUsuario,telefone) VALUES (@idUsuario, @telefone);";
        private string selectTelefones = "SELECT idUsuario, telefone FROM Telefones WHERE idUsuario = @idUsuario;";
        private string selectTodosTelefones = "SELECT idUsuario, telefone FROM Telefones;";
        private string deleteTelefone = "DELETE FROM Telefones WHERE idUsuario =@idUsuario and telefone = @telefone";
        private readonly ILogger<TelefoneController> _logger;

        public TelefoneController(ILogger<TelefoneController> logger)
        {
            _logger = logger;
        }

        [HttpGet ("recuperarTelefonesUsuario")]
        public List<Telefone> recuperarTelefonesUsuario(Telefone tel)
        {
            List<Telefone> telefones = new List<Telefone>();
            SqlConnection con = new SqlConnection(sqlCon);

            SqlCommand cmd = new SqlCommand(selectTelefones, con);
            cmd.Parameters.Add("@idUsuario", System.Data.SqlDbType.Int);
            cmd.Parameters["@idUsuario"].Value = tel.idUsuario;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Telefone telAux = new Telefone();
                    telAux.idUsuario = reader.GetInt32(0);
                    telAux.telefone = reader.GetInt32(1);
                    telefones.Add(telAux);
                }
            }
            return telefones;
        }

        [HttpGet("recuperarTodosTelefones")]
        public List<Telefone> recuperarTodosTelefones()
        {
            List<Telefone> telefones = new List<Telefone>();
            SqlConnection con = new SqlConnection(sqlCon);

            SqlCommand cmd = new SqlCommand(selectTodosTelefones, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Telefone telAux = new Telefone();
                    telAux.idUsuario = reader.GetInt32(0);
                    telAux.telefone = reader.GetInt32(1);
                    telefones.Add(telAux);
                }
            }
            return telefones;
        }

        [HttpPost ("cadastraTelefoneUsuario")]
        public void cadastraTelefoneUsuario([FromBody]Telefone tel)
        {
            SqlConnection con = new SqlConnection(sqlCon);

            SqlCommand cmd = new SqlCommand(insertTelefone, con);
            cmd.Parameters.Add("@idUsuario", System.Data.SqlDbType.Int);
            cmd.Parameters["@idUsuario"].Value = tel.idUsuario;

            cmd.Parameters.Add("@telefone", System.Data.SqlDbType.Int);
            cmd.Parameters["@telefone"].Value = tel.telefone;

            con.Open();
            cmd.ExecuteNonQuery();

        }

        [HttpPost("apagarTelefone")]
        public void apagarTelefone([FromBody]Telefone tel)
        {
            SqlConnection con = new SqlConnection(sqlCon);

            SqlCommand cmd = new SqlCommand(deleteTelefone, con);
            cmd.Parameters.Add("@idPessoa", System.Data.SqlDbType.Int);
            cmd.Parameters["@idPessoa"].Value = tel.idUsuario;

            cmd.Parameters.Add("@telefone", System.Data.SqlDbType.Int);
            cmd.Parameters["@telefone"].Value = tel.telefone;

            con.Open();
            cmd.ExecuteNonQuery();
        }

    }
}
