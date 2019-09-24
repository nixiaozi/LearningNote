using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADONetTest
{
    public static class ADONetTestHelper
    {
        private readonly static string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=EntityFrameworkTest.Context.EFDbContext;Integrated Security=True;MultipleActiveResultSets=True";

        public static void ADONetTestHelperDoing()
        {
            // AddContent();
            CustomExecConnectString();
        }

        /// <summary>
        /// 连接到数据源
        /// </summary>
        public static void AddContent()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Do work here.  
            }
        }

        /// <summary>
        /// 使用自定义生成连接字符串生成连接
        /// </summary>
        public static void CustomExecConnectString()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder =
                    new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder["Data Source"] = "(localdb)\\mssqllocaldb";
            builder["integrated Security"] = true;
            builder["Initial Catalog"] = "EntityFrameworkTest.Context.EFDbContext";
            builder["MultipleActiveResultSets"] = true;
            // builder["NewValue"] = "ForeTheNew"; 添加此项会报错：不支持的关键字

            var customerString = builder.ConnectionString;

            using (SqlConnection connection = new SqlConnection(customerString))
            {
                connection.Open();
                // Do work here.  
            }
        }

        #region 开放式并发示例
        //保守式并发涉及到锁定数据源中的行，以防止其他用户因修改数据而影响当前用户。 
        //在保守式模型中，当用户执行会应用锁的操作时，其他用户将无法执行可能与锁发生冲突的操作，直到锁所有者释放锁为止。 
        //此模型主要用于以下环境：对数据存在激烈争用，使得用锁保护数据的成本少于在发生并发冲突时回滚事务的成本。
        public static void OpenAsyncExample()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Do work here.  

                // Assumes connection is a valid SqlConnection.  
                SqlDataAdapter adapter = new SqlDataAdapter(
                  "SELECT CustomerID, CompanyName FROM Customers ORDER BY CustomerID",
                  connection);

                // The Update command checks for optimistic concurrency violations  
                // in the WHERE clause.  
                adapter.UpdateCommand = new SqlCommand("UPDATE Customers Set CustomerID = @CustomerID, CompanyName = @CompanyName " +
                   "WHERE CustomerID = @oldCustomerID AND CompanyName = @oldCompanyName", connection);
                adapter.UpdateCommand.Parameters.Add(
                  "@CustomerID", SqlDbType.NChar, 5, "CustomerID");
                adapter.UpdateCommand.Parameters.Add(
                  "@CompanyName", SqlDbType.NVarChar, 30, "CompanyName");

                // Pass the original values to the WHERE clause parameters.  
                SqlParameter parameter = adapter.UpdateCommand.Parameters.Add(
                  "@oldCustomerID", SqlDbType.NChar, 5, "CustomerID");
                parameter.SourceVersion = DataRowVersion.Original;
                parameter = adapter.UpdateCommand.Parameters.Add(
                  "@oldCompanyName", SqlDbType.NVarChar, 30, "CompanyName");
                parameter.SourceVersion = DataRowVersion.Original;

                // Add the RowUpdated event handler.  
                adapter.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "Customers");

                // Modify the DataSet contents.  

                adapter.Update(dataSet, "Customers");

                foreach (DataRow dataRow in dataSet.Tables["Customers"].Rows)
                {
                    if (dataRow.HasErrors)
                        Console.WriteLine(dataRow[0] + "\n" + dataRow.RowError);
                }
            }
        }


        private static void OnRowUpdated(object sender, SqlRowUpdatedEventArgs args)
        {
            if (args.RecordsAffected == 0)
            {
                args.Row.RowError = "Optimistic Concurrency Violation Encountered";
                args.Status = UpdateStatus.SkipCurrentRow;
            }
        }
        #endregion




    }
}
