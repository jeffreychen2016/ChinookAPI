﻿using ChinookAPI.Models;
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
    }
}