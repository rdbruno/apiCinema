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
    public class Ticket : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public Ticket(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("ProximoId/")]

        public JsonResult Post()
        {
            string query = @"insert into IdIngressoUsuario VALUES (DEFAULT) 
                             SELECT Scope_Identity() IdIngressoUsuario";
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

        [HttpPost("CadastraIngresso/")]

        public JsonResult Post(Ingresso I)
        {
            string query = @" EXEC [SP_InsereIngresso] 
                            " + I.IdIngressoUsuario + @",
                              " + I.IdUsuario + @",
                              " + I.IdSala + @",
                              " + I.Lugar + @",
                              " + I.IdPagamentoUsuario + @",
                              " + I.RgCpf + @",
                              " + I.ValorTotal + @"";
            DataTable table = new DataTable();
            Console.WriteLine(query);
            string query1 = "SELECT I.IdIngresso, I.IdIngressoUsuario, I.IdPagamentoUsuario, FP.FormaPagamento, " +
                "I.IdUsuario, L.Nome, L.Email,  SF.NomeSala, F.NomeFilme, " +
                "F.PrecoIngresso, I.Lugar " +
                "FROM T_Ingresso I " +
                "LEFT JOIN T_Login L ON I.IdUsuario = L.Id " +
                "LEFT JOIN T_SalaFilme SF ON I.IdSala = SF.IdSala " +
                "LEFT JOIN T_Filmes F ON SF.IdFilme = F.IdFilme " +
                "LEFT JOIN T_Pagamento P ON I.IdPagamentoUsuario = P.IdPagamentoUsuario " +
                "LEFT JOIN T_IdFormaPagamento FP ON P.IdFormaPagamento = FP.IdFormaPagamento " +
                "WHERE I.BitAtivo = 1 AND I.IdIngressoUsuario=" + I.IdIngressoUsuario + @"";
            string sqlDataSource = _configuration.GetConnectionString("WebApiCinemaCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    myReader.Close();

                    using (SqlCommand myCommand1 = new SqlCommand(query1, myCon))
                        myReader = myCommand1.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();

                    return new JsonResult(table);
                }
            }

            //   return new JsonResult("Adicionado com sucesso!");
        }

        [HttpPut]

        public JsonResult Put(Ingresso I)
        {
            string query = @" UPDATE T_Ingresso SET RgCpf = " + I.RgCpf + @" WHERE IdIngresso =  " + I.IdIngresso + @"";
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
                    return new JsonResult(table);
                }
            }

            //   return new JsonResult("Adicionado com sucesso!");
        }

        [HttpPost("DevolveIngresso/")]

        public JsonResult DevolveIngresso(Ingresso Ingresso)
        {
            string query = @" EXEC [SP_DevolveIngresso] 
                              " + Ingresso.IdIngresso + @"";
            DataTable table = new DataTable();
            Console.WriteLine(query);
            string sqlDataSource = _configuration.GetConnectionString("WebApiCinemaCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    if (Ingresso.IdIngresso == 0)
                    {
                        myCon.Close();
                        return new JsonResult("Preencha os campos corretamente") { StatusCode = (int)404 }; ;
                    }

                    myReader = myCommand.ExecuteReader();
                    myReader.Close();
                    myCon.Close();
                    return new JsonResult("Ingresso devolvido");
                }
            }

            //   return new JsonResult("Adicionado com sucesso!");
        }

    }
}
