using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using customerlistcapstone.models;

namespace customerlistcapstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class customerlistController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public customerlistController(IConfiguration configuration)

        {
            _configuration = configuration;
        }



        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                           select name,password,email from
                           dbo.customerlist";


            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("customerlistAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);

        }
        [HttpPost]

        public JsonResult Post(customerlist dep)
        {
            string query = @"
                           insert into dbo.customerlist
                           values (@name,@password,@email) 
                            ";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("customerlistAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new(sqlDataSource))
            {
                {
                    myCon.Open();
                    using SqlCommand myCommand = new(query, myCon);
                   
                    myCommand.Parameters.AddWithValue("@name", dep.name);
                    myCommand.Parameters.AddWithValue("@password", dep.password);
                    myCommand.Parameters.AddWithValue("@email", dep.email);



                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");

        }

    }
}
