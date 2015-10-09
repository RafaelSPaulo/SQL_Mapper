using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlMapper3;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;
using SqlMapperDomainTest;

namespace SqlMapper3Test
{
    [TestClass]
    public class SqlMapperUnitTest
    {
        public Type SqlDataMapperType = typeof(SqlDataMapper<>);
        public Object[] DataMapperParams = new Object[] { getMySqlConnectionString() };
        public Type PropertyColumnMapperType = typeof(PropertyColumnMapper);
        public Type FieldsColumnMapperType = typeof(FieldsColumnMapper);
        public Type MultipleConnectionPolicyType = typeof(MultipleConnectionPolicy);
        public Type SingleConnectionPolicyType = typeof(SingleConnectionPolicy);
        public Type ExplicitConnectionPolicyType = typeof(ExplicitConnectionPolicy);

        public static string getMySqlConnectionString()
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.DataSource = @"RoXaSDTD-PC\SQLEXPRESS";
            sqlConnectionStringBuilder.InitialCatalog = "Northwind";
            sqlConnectionStringBuilder.UserID = "Rafael";
            sqlConnectionStringBuilder.Password = "rafael";
            sqlConnectionStringBuilder.IntegratedSecurity = true;
            return sqlConnectionStringBuilder.ConnectionString;
        }

        [TestMethod]
        public void ConnectedToTestSqlConnection()
        {
            // Arrange
            string connectionString = getMySqlConnectionString();
            bool result = false;
            // Act

            using (SqlConnection conSql = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = conSql.CreateCommand())
                {
                    // Assert
                    try
                    {
                        conSql.Open();
                        result = true;
                    }
                    catch { }
                }
            }
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UpdateSupplierFromProduct()
        {
            // Arrange
            Builder builder = new Builder(
                SqlDataMapperType,
                DataMapperParams,
                PropertyColumnMapperType,
                MultipleConnectionPolicyType);
            IDataMapper productMapper = builder.Build<Product>();
            IConnectionPolicy explicitConnectionPolicy = productMapper.GetConnectionPolicy();
            String fakeName = "teste";
            String trueName = "";
            String updatedName = "";
            // Act
            Product p1 = (Product)productMapper.GetAll().First();
            trueName = p1.SupplierID.CompanyName;
            p1.SupplierID.CompanyName = fakeName;
            productMapper.Update(p1);

            Product p2 = (Product)productMapper.GetAll().First();
            updatedName = p2.SupplierID.CompanyName;

            p2.SupplierID.CompanyName = trueName;
            productMapper.Update(p2);

            // Assert
            Assert.AreEqual(fakeName, updatedName);
        }
    }
}
