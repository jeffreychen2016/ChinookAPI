using ChinookAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ChinookAPI.DataAccess
{
    public class Chinook
    {
        private const string ConnectionString = "server=(local);Database=Chinook;Trusted_Connection=true";

        public List<Invoice> GetInvoicesBySalesAgentID(int id)
        {
            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                dbConnection.Open();

                var command = dbConnection.CreateCommand();
                command.CommandText = @"SELECT 
                                        FullName = Customer.FirstName + ' ' + Customer.LastName,
                                        Invoice.*
                                        FROM Invoice
                                        INNER JOIN Customer
                                            ON Invoice.CustomerId = Customer.CustomerId
                                        INNER JOIN Employee
                                            ON Employee.EmployeeId = Customer.SupportRepId
                                        WHERE Customer.SupportRepId = @id";

                command.Parameters.AddWithValue("@id",id);

                var result = command.ExecuteReader();

                var Invoices = new List<Invoice>();
                while (result.Read())
                {
                    var invoice = new Invoice
                    {
                        InvoiceId = (int)result["InvoiceId"],
                        ClientFullName = result["FullName"].ToString(),
                        InvoiceDate = (DateTime)result["InvoiceDate"], 
                        BillingAddress = result["BillingAddress"].ToString(),
                        BillingCity = result["BillingCity"].ToString(),
                        BillingState = result["BillingState"].ToString(),
                        BillingCountry = result["BillingState"].ToString(),
                        BillingPostalCode = result["BillingPostalCode"].ToString(),
                        Total = (decimal)result["Total"]
                    };

                    Invoices.Add(invoice);
                }

                return Invoices;
            };
        }

        public List<Invoice> GetAllInvoices()
        {
            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                dbConnection.Open();

                var command = dbConnection.CreateCommand();
                command.CommandText = @"SELECT 
                                            Invoice.Total
	                                        ,CustomerName = Customer.FirstName + ' ' + Customer.LastName
	                                        ,Invoice.BillingCountry
	                                        ,SaleAgent = Employee.FirstName + ' ' + Employee.LastName
                                        FROM Invoice
                                        INNER JOIN Customer
                                            ON Invoice.CustomerId = Customer.CustomerId
                                        INNER JOIN Employee
                                            ON Customer.SupportRepId = Employee.EmployeeId";

                var result = command.ExecuteReader();
                var invoices = new List<Invoice>();

                while (result.Read())
                {
                    var invoice = new Invoice
                    {
                        Total = (decimal)result["Total"],
                        ClientFullName = result["CustomerName"].ToString(),
                        BillingCountry = result["BillingCountry"].ToString(),
                        SalesAgentFullName = result["SaleAgent"].ToString()
                    };

                    invoices.Add(invoice);
                }

                return invoices;
            };
        }

        public int GetCountOfItemsByInvoiceID(int id)
        {
            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                dbConnection.Open();

                var command = dbConnection.CreateCommand();
                command.CommandText = @"SELECT 
                                            Counts = COUNT(*)
                                        FROM InvoiceLine
                                        WHERE InvoiceId = @id";

                command.Parameters.AddWithValue("@id",id);
                var result = (int)command.ExecuteScalar();

                return result;
            }
        }

        public bool AddNewInvoice(Invoice invoice)
        {

            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                dbConnection.Open();

                var command = dbConnection.CreateCommand();
                command.CommandText = @"INSERT INTO 
                                            Invoice (CustomerId,InvoiceDate,BillingAddress,BillingCity,BillingState,BillingCountry,BillingPostalCode,Total)
                                        VALUES (@CustomerId,@InvoiceDate,@BillingAddress,@BillingCity,@BillingState,@BillingCountry,@BillingPostalCode,@Total)";

                command.Parameters.AddWithValue("@CustomerId",invoice.CustomerId);
                command.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);
                command.Parameters.AddWithValue("@BillingAddress", invoice.BillingAddress);
                command.Parameters.AddWithValue("@BillingCity", invoice.BillingCity);
                command.Parameters.AddWithValue("@BillingState", invoice.BillingState);
                command.Parameters.AddWithValue("@BillingCountry", invoice.BillingCountry);
                command.Parameters.AddWithValue("@BillingPostalCode", invoice.BillingPostalCode);
                command.Parameters.AddWithValue("@Total", invoice.Total);


                var result = command.ExecuteNonQuery();

                return result == 1;
            }
        }
    };
}
