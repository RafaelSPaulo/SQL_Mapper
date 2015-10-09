using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlMapper1;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;
using SqlMapperDomainTest;

namespace SqlMapper1Test
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
        public void BuildSqlMapperForProduct()
        {
            // Arrange
            Builder builder = new Builder(
                SqlDataMapperType,
                DataMapperParams,
                PropertyColumnMapperType,
                MultipleConnectionPolicyType);
            try
            {
                // Act
                IDataMapper<ProductSimple> productMapper = builder.Build<ProductSimple>();

                // Assert
                Assert.IsNotNull(productMapper);
            }
            catch(Exception){
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void BuildSqlMapperForOrders()
        {
            // Arrange
            Builder builder = new Builder(
                SqlDataMapperType,
                DataMapperParams,
                PropertyColumnMapperType,
                MultipleConnectionPolicyType);

            // Act
            IDataMapper<Orders> ordersMapper = builder.Build<Orders>();

            // Assert
            Assert.IsNotNull(ordersMapper);
        }

        [TestMethod]
        public void BuildSqlMapperForEmployees()
        {
            // Arrange
            Builder builder = new Builder(
                SqlDataMapperType,
                DataMapperParams,
                PropertyColumnMapperType,
                MultipleConnectionPolicyType);

            // Act
            IDataMapper<Employees> employeesMapper = builder.Build<Employees>();

            // Assert
            Assert.IsNotNull(employeesMapper);
        }

        [TestMethod]
        public void BuildSqlMapperForCustomers()
        {
            // Arrange
            Builder builder = new Builder(
                SqlDataMapperType,
                DataMapperParams,
                PropertyColumnMapperType,
                MultipleConnectionPolicyType);

            // Act
            IDataMapper<Customers> customersMapper = builder.Build<Customers>();

            // Assert
            Assert.IsNotNull(customersMapper);
        }

        [TestMethod]
        public void GetAllProductsWithSuccess()
        {
            // Arrange
            Builder builder = new Builder(
                SqlDataMapperType,
                DataMapperParams,
                PropertyColumnMapperType,
                MultipleConnectionPolicyType);
            IDataMapper<ProductSimple> productMapper = builder.Build<ProductSimple>();

            // Act
            List<ProductSimple> allProducts = productMapper.GetAll().ToList();

            // Assert
            Assert.IsTrue(allProducts.Count > 0);
        }

        [TestMethod]
        public void UpdateProductsWithSuccess()
        {
            // Arrange
            Builder builder = new Builder(
                SqlDataMapperType,
                DataMapperParams,
                PropertyColumnMapperType,
                ExplicitConnectionPolicyType);

            IDataMapper<ProductSimple> productMapper = builder.Build<ProductSimple>();
            IConnectionPolicy explicitConnectionPolicy = productMapper.GetConnectionPolicy();

            // Act
            explicitConnectionPolicy.OpenConnection();
            explicitConnectionPolicy.BeginTransaction();

            ProductSimple p1 = productMapper.GetAll().First();
            
            p1.ProductName = "Potatoes";
            productMapper.Update(p1);
            
            ProductSimple p2 = productMapper.GetAll().First();

            // Assert
            Assert.AreEqual(p1.ProductName,p2.ProductName);
            explicitConnectionPolicy.RollBack();
            explicitConnectionPolicy.Dispose();
        }        
    }
}
