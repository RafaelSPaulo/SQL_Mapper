using System;
using System.Data.SqlClient;
using System.Linq;
using SqlMapper1;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;
using SqlMapperDomainTest;

namespace SqlMapper1Test
{
    public class SqlMapperManualTest
    {
        public static void Main(String[] args) {
            //m1();
            //m2();
            //m3();
        }

        public static void m1() {
            Builder builder = new Builder(
                   typeof(SqlDataMapper<>),
                   new Object[] { SqlMapperUnitTest.getMySqlConnectionString() },
                   typeof(PropertyColumnMapper),
                   typeof(SingleConnectionPolicy));

            IDataMapper<ProductSimple> productMapper = builder.Build<ProductSimple>();
            IConnectionPolicy policy = productMapper.GetConnectionPolicy();

            ProductSimple p = productMapper.GetAll().First();
            p.ProductName = "Potatoes";
            productMapper.Update(p);

            ProductSimple p2 = productMapper.GetAll().First();
            p2.ProductName = "Chai";
            productMapper.Update(p2);

            ProductSimple novoP = new ProductSimple();
            novoP.ProductName = "batatas";
            novoP.QuantityPerUnit = "setenta mil";
            productMapper.Insert(novoP);
        }

        public static void m2() {
            Builder builder = new Builder(
                    typeof(SqlDataMapper<>),
                   new Object[] { SqlMapperUnitTest.getMySqlConnectionString() },
                   typeof(PropertyColumnMapper),
                   typeof(ExplicitConnectionPolicy));

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
            bool result = p1.ProductName == p2.ProductName;
            explicitConnectionPolicy.RollBack();
            explicitConnectionPolicy.Dispose();
        }

        public static void m3()
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.DataSource = @"RoXaSDTD-PC\SQLEXPRESS";
            sqlConnectionStringBuilder.InitialCatalog = "PlaylistManager";
            sqlConnectionStringBuilder.UserID = "Rafael";
            sqlConnectionStringBuilder.Password = "rafael";
            sqlConnectionStringBuilder.IntegratedSecurity = true;
            string connectionString = sqlConnectionStringBuilder.ConnectionString;

            Builder builder = new Builder(
                   typeof(SqlDataMapper<>),
                   new Object[] { connectionString },
                   typeof(PropertyColumnMapper),
                   typeof(MultipleConnectionPolicy));

            IDataMapper<Playlist> playlistMapper = builder.Build<Playlist>();
            IConnectionPolicy policy = playlistMapper.GetConnectionPolicy();

            Playlist p = playlistMapper.GetAll().First();
            p.name = "teste";
            playlistMapper.Update(p);

            Playlist p2 = playlistMapper.GetAll().First();
            p2.name = "JoanaPlaylist";
            playlistMapper.Update(p2);
        }
    }
}
