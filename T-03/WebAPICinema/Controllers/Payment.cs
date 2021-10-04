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
    public class Payment
    {

        private readonly IConfiguration _configuration;

        public Payment(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("ProximoIdPagamento/")]
        public JsonResult Post()
        {
            string query = @"INSERT INTO IdPagamentoUsuario VALUES (DEFAULT)
                             SELECT Scope_Identity() IdPagamentoUsuario";
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
                    return new JsonResult(table);
                }
            }

            //   return new JsonResult("Adicionado com sucesso!");
        }

        [HttpPost("CadastraPagamento/")]

        public JsonResult Post(Pagamento P)
        {
            string query = @" INSERT INTO T_Pagamento (IdPagamentoUsuario, IdFormaPagamento, NumeroCartao, DataExpiracao, CodigoSeguranca) VALUES
                            (
                              " + P.IdPagamentoUsuario + @",
                              " + P.IdPagamento + @",
                              " + P.NumeroCartao + @",
                              '" + P.DataExpiracao + @"',
                              " + P.CodigoSeguranca + @"
                            )";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WebApiCinemaCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    if (P.IdPagamentoUsuario == 0 || P.IdPagamento == 0)
                    {
                        myCon.Close();
                        return new JsonResult("Preencha os campos corretamente") { StatusCode = (int)404 }; ;
                    }
                    else
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader); ;
                        myReader.Close();
                        myCon.Close();
                        return new JsonResult("Adicionado com sucesso!");
                    }
                }
            }

            //   return new JsonResult("Adicionado com sucesso!");
        }
    }
}
