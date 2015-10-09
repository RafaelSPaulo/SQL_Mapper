using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlMapper2;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;
using SqlMapperDomainTest;

namespace SqlMapper2Test
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
                    catch {}
                }
            }
            // Assert
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void GetAllAndWhereProductsWithSuccess()
        {
            // Arrange
            Builder builder = new Builder(
                SqlDataMapperType,
                DataMapperParams,
                PropertyColumnMapperType,
                MultipleConnectionPolicyType);
            IDataMapper<ProductSimple> productMapper = builder.Build<ProductSimple>();

            // Act
            ISqlEnumerable<ProductSimple> prods = productMapper.GetAll();
            ISqlEnumerable<ProductSimple> prods2 = prods.Where("CategoryID = 7");
            ISqlEnumerable<ProductSimple> prods3 = prods2.Where("UnitsinStock > 30");

            // Assert
            Assert.AreEqual(prods.Count(), 77);
            Assert.AreEqual(prods2.Count(), 5);
            Assert.AreEqual(prods3.Count(), 1);
        } 
    }
}
