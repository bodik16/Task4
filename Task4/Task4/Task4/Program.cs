using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Task4.Properties.Settings.NorthwindConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            Console.WriteLine("Show the list of first and last names of the employees from London");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT FirstName, LastName FROM Employees WHERE City='London';";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("{0}\t{1}", reader["FirstName"], reader["LastName"]);
            }
            reader.Close();

            Console.WriteLine("\nCalculate the count of employees from London:");
            command.CommandText = "SELECT COUNT(*) AS EmployeeQuantity FROM Employees WHERE City='London';";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader["EmployeeQuantity"]);
            }
            reader.Close();

            Console.WriteLine("\nShow first and last names of the employees as well as the count of orders each of them have received during the year 1997 (use left join)");
            command.CommandText = "SELECT e.FirstName, e.LastName, COUNT(o.EmployeeID) AS OrdersQuantity FROM Employees AS e LEFT JOIN Orders AS o ON o.EmployeeID=e.EmployeeID WHERE o.OrderDate>='1997-01-01' AND o.OrderDate<='1997-12-31' GROUP BY e.FirstName, e.LastName;";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("{0,-10}{1,-10}{2}", reader["FirstName"], reader["LastName"], reader["OrdersQuantity"]);
            }
            reader.Close();

            Console.WriteLine("\nShow the list of french customers’ names who have made more than one order (use grouping)");
            command.CommandText = "SELECT c.ContactName FROM Customers AS c, Orders AS o WHERE c.Country='France' GROUP BY c.ContactName HAVING COUNT(o.CustomerID)>1;";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader["ContactName"]);
            }
            reader.Close();

            Console.WriteLine("\nShow the list of french customers’ names who used to order french products");
            command.CommandText = "SELECT c.ContactName FROM Customers AS c, Orders AS o WHERE c.CustomerID=o.CustomerID AND c.Country='France' AND o.ShipCountry='France';";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader["ContactName"]);
            }
            reader.Close();
            
            Console.WriteLine("\nShow the total ordering sum calculated for each country of customer");
            command.CommandText = "SELECT c.Country, SUM(od.UnitPrice) AS TotalPrice FROM Customers AS c, Orders AS o, [Order Details] AS od WHERE o.CustomerID=c.CustomerID AND o.OrderID=od.OrderID GROUP BY c.Country;";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("{0,-10}{1}", reader["Country"], reader["TotalPrice"]);
            }
            reader.Close();
            Console.ReadKey();
        }
    }
}
