using System;
using SqlMapper3;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;
using SqlMapperDomainTest;

namespace SqlMapper3Test
{
    public class SqlMapperManualTest
    {
        public static Type SqlDataMapperType = typeof(SqlDataMapper<>);
        public static Object[] DataMapperParams = new Object[] { SqlMapper3Test.SqlMapperUnitTest.getMySqlConnectionString() };
        public static Type PropertyColumnMapperType = typeof(PropertyColumnMapper);
        public static Type FieldsColumnMapperType = typeof(FieldsColumnMapper);
        public static Type MultipleConnectionPolicyType = typeof(MultipleConnectionPolicy);
        public static Type SingleConnectionPolicyType = typeof(SingleConnectionPolicy);
        public static Type ExplicitConnectionPolicyType = typeof(ExplicitConnectionPolicy);

        public static void Main(String[] args) {
           //m1();
        }

        public static void m1() {
            Builder builder = new Builder(
               SqlDataMapperType,
               DataMapperParams,
               //FieldsColumnMapperType,
               PropertyColumnMapperType,
               MultipleConnectionPolicyType);
            IDataMapper productMapper = builder.Build<Product>();

            SqlEnumerable prods = productMapper.GetAll();
            Product p2 = (Product)productMapper.GetById(1);
            p2.SupplierID.ContactName = "Charlotte";
            productMapper.Update(p2);

            p2.SupplierID.ContactName = "Charlotte Cooper";
            productMapper.Update(p2);

            foreach (Product p in prods)
            {
                Console.WriteLine(p);
            }
        }
    }
}
