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
    public class ShoppingCart
    {
        private readonly IConfiguration _configuration;

        public ShoppingCart(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult Get(int IdUsuario)
        {
            //string query = @"select IdFilme, NomeFilme, UrlCartaz, PrecoIngresso FROM T_Filmes WHERE BitAtivo = 1";
            string query = @"SELECT C.IdCarrinho, C.IdUsuario, C.Lugar, C.ValorTotal, C.ValorTotal, 
                             C.BitAtivo, L.Nome, L.Telefone, SF.IdSala, SF.NomeSala, SF.HorarioSecao, 
                             F.NomeFilme, F.PrecoIngresso  FROM T_Carrinho C 
                             INNER JOIN T_Login L ON C.IdUsuario = L.Id 
                             INNER JOIN T_SalaFilme SF ON C.IdSala = SF.IdSala
                             INNER JOIN T_Filmes F ON SF.IdFilme = F.IdFilme                             
                           WHERE C.BitAtivo = 1 and IdUsuario ='" + IdUsuario + @"'";
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

        [HttpPost]

        public JsonResult Post(Carrinho Car)
        {
            //string query = @"select IdFilme, NomeFilme, UrlCartaz, PrecoIngresso FROM T_Filmes WHERE BitAtivo = 1";
            string query = @"INSERT INTO T_Carrinho (IdUsuario, IdSala, Lugar, ValorTotal) VALUES
                            (
                            " + Car.IdUsuario + @"
                            ," + Car.IdSala + @"
                            ," + Car.Lugar + @"
                            ," + Car.ValorTotal + @"
                            )";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WebApiCinemaCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    /* myReader = myCommand.ExecuteReader();
                     table.Load(myReader); ;

                     myReader.Close();
                     myCon.Close(); */

                    if (Car.IdUsuario == 0 || Car.IdSala == 0 )
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

        }

        [HttpDelete]

        public JsonResult Delete(int IdCarrinho)
        {
            //string query = @"select IdFilme, NomeFilme, UrlCartaz, PrecoIngresso FROM T_Filmes WHERE BitAtivo = 1";
            string query = @"UPDATE T_Carrinho SET BitAtivo = 0 WHERE IdCarrinho ='" + IdCarrinho + @"'";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WebApiCinemaCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    /* myReader = myCommand.ExecuteReader();
                     table.Load(myReader); ;

                     myReader.Close();
                     myCon.Close(); */

                    if (IdCarrinho == 0)
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
                        return new JsonResult("Removido com sucesso");
                    }
                }
            }

        }
    }
}
