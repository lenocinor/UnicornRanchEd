using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home Page";

            string connectionString;
            connectionString = @"Data Source=DESKTOP-9L20D2P;Initial Catalog=TestDb;User ID=sa;Password=admin";
            string queryString = "SELECT * from dbo.Unicorn";
            var unicorns = new HashSet<Unicorn>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Unicorn unicorn = new Unicorn
                        {
                            name = reader["Name"].ToString(),
                            color = reader["Color"].ToString(),
                            favoriteFood = reader["Favorite Food"].ToString(),
                            location = reader["location"].ToString()
                        };
                        unicorns.Add(unicorn);// etc
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }

            ViewBag.unicorns = unicorns;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public bool UnicornMove([FromBody]UnicornMove unicornMove)
        {
            string connectionString;
            connectionString = @"Data Source=DESKTOP-9L20D2P;Initial Catalog=TestDb;User ID=sa;Password=admin";
            string queryString = "UPDATE dbo.Unicorn SET Unicorn.Location = '" + unicornMove.newLocation + "' WHERE Unicorn.Name = '" + unicornMove.unicornName + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    dataAdapter.UpdateCommand = command;
                    dataAdapter.UpdateCommand.ExecuteNonQuery();
                    dataAdapter.Dispose();
                }
                finally { }
            }

            return true;
        }
    }

    public class Unicorn
    {
        public string name;
        public string color;
        public string favoriteFood;
        public string location;
    }
}
