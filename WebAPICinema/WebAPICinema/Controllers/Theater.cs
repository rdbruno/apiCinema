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
    public class Theater : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public Theater(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("IdFilme={Id}")]
        public JsonResult Get(int Id)
        {
            string query = @"SELECT IdSala, IdFilme, NomeSala, HorarioSecao, QuantidadeLugares FROM T_SalaFilme where IdFilme ='" + Id + @"'";
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

