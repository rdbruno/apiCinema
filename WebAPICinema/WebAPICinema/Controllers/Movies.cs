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
            string query = @"select IdFilme, NomeFilme, PrecoIngresso FROM T_Filmes WHERE BitAtivo = 1";
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
    }
}
