using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ChickenSoftware.BusinessRules.ObjectOriented
{
    public class CustomerInSystemHandler: Handler
    {
        Handler _successor = null;
        Customer _customer = null;

        public CustomerInSystemHandler(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }
            _customer = customer;
        }

        public override void Process()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var commandText = "usp_verifyInvoince";
                using (var command = new SqlCommand(commandText))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(_customer.Id);
                    try
                    {
                        connection.Open();
                        var isInSystem = (bool)command.ExecuteScalar();
                        if(isInSystem)
                        {
                            _successor.Process();
                        }
                        else
                        {
                            Logger.Write("Something Went Wrong");
                        }


                    }
                    catch (SqlException ex)
                    {
                        Logger.Write("Something Went Wrong");
                        throw new BuisnessRulesException("CustomerInSystemHandler", ex);
                    }
                }
            }
        }

        public override void SetSuccessor(Handler handler)
        {
            _successor = handler;
        }
    }
}
