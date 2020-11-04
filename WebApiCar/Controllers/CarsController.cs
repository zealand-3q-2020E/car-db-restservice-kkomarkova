using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Clauses;
using WebApiCar.Model;

namespace WebApiCar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {

        string SelectAllCars = "select * from car";
        static string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CarDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static List<Car> carList = new List<Car>()
        {
            new Car(){Id = 1,Model="x3",Vendor="tesla", Price=400000},
            new Car(){Id = 2,Model="x2",Vendor="tesla", Price=600000},
            new Car(){Id = 3,Model="x1",Vendor="tesla", Price=800000},
            new Car(){Id = 4,Model="x0",Vendor="tesla", Price=1400000},
        };

        /// <summary>
        /// Method for get all the cars from the static list
        /// </summary>
        /// <returns>List of cars</returns>
        // GET: api/Cars
        [HttpGet]
        public IEnumerable<Car> Get()
        {
            List<Car> listofcars = new List<Car>();
            string selectAll = "select id, vendor, model, price from Car";
            using (SqlConnection databaseConnection = new SqlConnection(conn))
            {
                //SQL Command
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectAll, databaseConnection))
                {
                    //SQL DataReader
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string vendor = reader.GetString(1);
                            string model = reader.GetString(2);
                            int price = reader.GetInt32(3);

                            Car newCar = new Car(id,vendor,model,price);
                            listofcars.Add(newCar);
                        }
                    }
                }
            }
            return listofcars;
        }

        // GET: api/Cars/5
        [HttpGet("{id}", Name = "Get")]
        public Car Get(int id)
        {
            Car newcar;
            string selectcarbyid = "select id,vendor,model,price from car where id = @id";
            using (SqlConnection databaseconnection = new SqlConnection(conn))
            {
                
                using (SqlCommand selectCommand = new SqlCommand(selectcarbyid, databaseconnection))
                {
                    selectCommand.Parameters.AddWithValue("@id", id);
                    databaseconnection.Open();
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id2 = reader.GetInt32(0);
                            string vendor = reader.GetString(1);
                            string model = reader.GetString(2);
                            int price = reader.GetInt32(3);

                            newcar = new Car(id2,vendor,model,price);
                            return newcar;
                        }
                    }
                }

            }
            return null;
        }

        /// <summary>
        /// Post a new car to the static list
        /// </summary>
        /// <param name="value"></param>
        // POST: api/Cars
        [HttpPost]
        public void Post([FromBody] Car value)
        {
            string insertSql = "insert into car(id,model,vendor,price) values (@id,@model,@vendor,@price)";
            using (SqlConnection databaseconnection = new SqlConnection(conn))
            {
                databaseconnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertSql, databaseconnection))
                {
                    insertCommand.Parameters.AddWithValue("@id", value.Id);
                    insertCommand.Parameters.AddWithValue("@model", value.Model);
                    insertCommand.Parameters.AddWithValue("@vendor", value.Vendor);
                    insertCommand.Parameters.AddWithValue("@price", value.Price);
                    int rowsaffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine($"rowsaffected : {rowsaffected}");
                }

            }
    Car newcar = new Car() { Id = GetId(), Model = value.Model, Vendor = value.Vendor, Price = value.Price };
            carList.Add(newcar);
        }

        // PUT: api/Cars/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Car value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            carList.Remove(Get(id));
        }

       int GetId()
        {
            int max = carList.Max(x => x.Id);
            return max+1;
        }

    }
}
