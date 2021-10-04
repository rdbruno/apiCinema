using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebAPICinema.Models;

namespace WebAPICinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Movies : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public Movies(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult Get()
        {
            //string query = @"select IdFilme, NomeFilme, UrlCartaz, PrecoIngresso FROM T_Filmes WHERE BitAtivo = 1";
            string query = @"EXEC [SP_GetFilmes]";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WebApiCinemaCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpGet("IdUsuario={IdUsuario}")]

        public JsonResult Get(int IdUsuario)
        {
            //string query = @"select IdFilme, NomeFilme, UrlCartaz, PrecoIngresso FROM T_Filmes WHERE BitAtivo = 1";
            string query = @"SELECT I.IdIngresso, I.IdIngressoUsuario, I.IdUsuario, "+
                "L.Nome, L.RgCpf, "+
                "SF.IdSala, SF.NomeSala, SF.QuantidadeLugares, SF.HorarioSecao, "+
                "F.IdFilme, F.NomeFilme, F.PrecoIngresso, F.UrlCartaz " +
                "FROM T_Ingresso I " +
                "INNER JOIN T_Login L ON L.Id = I.IdUsuario " +
                "INNER JOIN T_SalaFilme SF ON I.IdSala = SF.IdSala " +
                "INNER JOIN T_Filmes F ON SF.IdFilme = F.IdFilme "+
                "WHERE I.BitAtivo = 1 AND I.IdUsuario ='" + IdUsuario + @"'";
            DataTable table = new DataTable();
            Console.WriteLine(query);
            string sqlDataSource = _configuration.GetConnectionString("WebApiCinemaCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }
    }
}
