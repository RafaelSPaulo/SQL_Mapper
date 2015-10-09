using System;
using SqlMapper2;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;
using SqlMapperDomainTest;

namespace SqlMapper2Test
{
    public class SqlMapperManualTest
    {
        public static Type SqlDataMapperType = typeof(SqlDataMapper<>);
        public static Object[] DataMapperParams = new Object[] { SqlMapper2Test.SqlMapperUnitTest.getMySqlConnectionString() };
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
               PropertyColumnMapperType,
               MultipleConnectionPolicyType);
            IDataMapper<ProductSimple> productMapper = builder.Build<ProductSimple>();

            ISqlEnumerable<ProductSimple> prods = productMapper.GetAll();
            foreach (ProductSimple p in prods)
            {
                Console.WriteLine(p);
            }
            
            Console.WriteLine("-------------");
            ISqlEnumerable<ProductSimple> prods2 = prods.Where("CategoryID = 7");
            foreach (ProductSimple p in prods2)
            {
                Console.WriteLine(p);
            }
            
            Console.WriteLine("-------------"); 
            ISqlEnumerable<ProductSimple> prods3 = prods2.Where("UnitsinStock > 30");
            foreach (ProductSimple p in prods3)
            {
                Console.WriteLine(p);
            }
        }
    }
}
