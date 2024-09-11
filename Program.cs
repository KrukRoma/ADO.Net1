using System;
using Microsoft.Data.SqlClient;
using System.Text;

namespace ADO.Net1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=DESKTOP-GE7UVHJ\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connected to the database.");

                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\nSelect an option:");
                    Console.WriteLine("1. Show all clients");
                    Console.WriteLine("2. Show all sellers");
                    Console.WriteLine("3. Show sales by a specific seller");
                    Console.WriteLine("4. Show sales greater than a specified amount");
                    Console.WriteLine("5. Show most expensive and cheapest sale for a specific client");
                    Console.WriteLine("6. Show first sale of a specific seller");
                    Console.WriteLine("7. Exit");

                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            ShowAllClients(connection);
                            break;
                        case "2":
                            ShowAllSellers(connection);
                            break;
                        case "3":
                            ShowSalesBySeller(connection);
                            break;
                        case "4":
                            ShowSalesGreaterThanAmount(connection);
                            break;
                        case "5":
                            ShowMostExpensiveAndCheapestSaleForClient(connection);
                            break;
                        case "6":
                            ShowFirstSaleBySeller(connection);
                            break;
                        case "7":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Try again.");
                            break;
                    }
                }
            }
        }

        static void ShowAllClients(SqlConnection connection)
        {
            string query = "select * from Clients";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.OutputEncoding = Encoding.UTF8;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($" {reader.GetName(i),-20}");
            }
            Console.WriteLine("\n" + new string('-', 100));

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($" {reader[i],-20} ");
                }
                Console.WriteLine();
            }

            reader.Close();
        }

        static void ShowAllSellers(SqlConnection connection)
        {
            string query = "select * from Employees";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.OutputEncoding = Encoding.UTF8;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($" {reader.GetName(i),-20}");
            }
            Console.WriteLine("\n" + new string('-', 100));

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($" {reader[i],-20} ");
                }
                Console.WriteLine();
            }

            reader.Close();
        }

        static void ShowSalesBySeller(SqlConnection connection)
        {
            Console.WriteLine("Enter seller's full name (e.g., Yaroschuk Ivan Petrovych): ");
            string sellerName = Console.ReadLine();

            string query = @"
                select s.Id, s.Price, s.Quantity, c.FullName as ClientName, p.Name as ProductName
                from Salles s
                join Employees e on s.EmployeeId = e.Id
                join Clients c on s.ClientId = c.Id
                join Products p on s.ProductId = p.Id
                where e.FullName = @SellerName";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SellerName", sellerName);

            SqlDataReader reader = command.ExecuteReader();

            Console.OutputEncoding = Encoding.UTF8;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($" {reader.GetName(i),-20}");
            }
            Console.WriteLine("\n" + new string('-', 100));

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($" {reader[i],-20} ");
                }
                Console.WriteLine();
            }

            reader.Close();
        }

        static void ShowSalesGreaterThanAmount(SqlConnection connection)
        {
            Console.WriteLine("Enter the minimum amount for sales:");
            decimal amount = Convert.ToDecimal(Console.ReadLine());

            string query = @"
                select s.Id, s.Price, s.Quantity, e.FullName as SellerName, c.FullName as ClientName, p.Name as ProductName
                from Salles s
                join Employees e on s.EmployeeId = e.Id
                join Clients c on s.ClientId = c.Id
                join Products p on s.ProductId = p.Id
                where s.Price > @Amount";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Amount", amount);

            SqlDataReader reader = command.ExecuteReader();

            Console.OutputEncoding = Encoding.UTF8;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($" {reader.GetName(i),-20}");
            }
            Console.WriteLine("\n" + new string('-', 100));

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($" {reader[i],-20} ");
                }
                Console.WriteLine();
            }

            reader.Close();
        }

        static void ShowMostExpensiveAndCheapestSaleForClient(SqlConnection connection)
        {
            Console.WriteLine("Enter client's full name (e.g., Petruk Stepan Romanovych): ");
            string clientName = Console.ReadLine();

            string query = @"
                select max(s.Price) as MostExpensive, min(s.Price) as Cheapest
                from Salles s
                join Clients c on s.ClientId = c.Id
                where c.FullName = @ClientName";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClientName", clientName);

            SqlDataReader reader = command.ExecuteReader();

            Console.OutputEncoding = Encoding.UTF8;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($" {reader.GetName(i),-20}");
            }
            Console.WriteLine("\n" + new string('-', 100));

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($" {reader[i],-20} ");
                }
                Console.WriteLine();
            }

            reader.Close();
        }

        static void ShowFirstSaleBySeller(SqlConnection connection)
        {
            Console.WriteLine("Enter seller's full name (e.g., Yaroschuk Ivan Petrovych): ");
            string sellerName = Console.ReadLine();

            string query = @"
                select top 1 s.Id, s.Price, s.Quantity, c.FullName as ClientName, p.Name as ProductName, s.Date
                from Salles s
                join Employees e on s.EmployeeId = e.Id
                join Clients c on s.ClientId = c.Id
                join Products p on s.ProductId = p.Id
                where e.FullName = @SellerName
                order by s.Date asc";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SellerName", sellerName);

            SqlDataReader reader = command.ExecuteReader();

            Console.OutputEncoding = Encoding.UTF8;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($" {reader.GetName(i),-20}");
            }
            Console.WriteLine("\n" + new string('-', 100));

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($" {reader[i],-20} ");
                }
                Console.WriteLine();
            }

            reader.Close();
        }
    }
}
