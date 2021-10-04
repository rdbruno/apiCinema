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
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult Get()
        {
            string query = @"select Nome, Telefone, Email, RgCpf, Senha from T_Login";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WebApiCinemaCon");
            SqlDataReader myReader;
            using(SqlConnection myCon=new SqlConnection(sqlDataSource))
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

        [HttpPost("GetLogin/")]

        public JsonResult PostLogin(Login log)
        {
            string query = @"select Id, Nome, Telefone, Email, RgCpf, Senha from T_Login 
                            WHERE Email = '" + log.Email + @"' AND Senha ='" + log.Senha + @"'";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WebApiCinemaCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();


                    if (myReader.HasRows)
                    {   
                        /*
                         ----MODO ANTIGO----
                        Console.WriteLine("Login Realizado com Sucesso");
                        table.Load(myReader); ;
                        myReader.Close();
                        myCon.Close();
                        return new JsonResult(table); 
                        */
                        //Console.WriteLine("Login Realizado com Sucesso");
                        var result = "Login Realizado com Sucesso";
                        table.Load(myReader); ;
                        var Login = table;
                        myReader.Close();
                        myCon.Close();
                        return new JsonResult(new { result, Login }); 
                    }
                    else if(log.Email == null || log.Email == "" || log.Senha == null || log.Senha == "")
                    {
                        return new JsonResult("Paramêtros incorretos: Digite Email e Senha") { StatusCode = (int)400 };
                    }
                    else
                    {
                        //Console.WriteLine("Senha Incorreta");
                        myReader.Close();
                        myCon.Close();
                        //return new JsonResult("Senha Incorreta");
                        return new JsonResult("Senha Incorreta") { StatusCode = (int)404 };
                    }

                }

            }


        }

        [HttpPost]

        public JsonResult Post(Login log)
        {
            string query = @"INSERT INTO T_Login (Nome, Telefone, Email, RgCpf, Senha) VALUES
                            (
                            '"+log.Nome+ @"'
                            ,'" + log.Telefone + @"'
                            ,'" + log.Email + @"'
                            ,'" + log.RgCpf + @"'
                            ,'" + log.Senha + @"'
                            )";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WebApiCinemaCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {

                    /*  
                     ----MODO ANTIGO ---
                      myReader = myCommand.ExecuteReader();
                      table.Load(myReader); ;

                      myReader.Close();
                      myCon.Close(); 
                    */

                    if (string.IsNullOrEmpty(log.Nome) || string.IsNullOrEmpty(log.Senha) || string.IsNullOrEmpty(log.Email))
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
