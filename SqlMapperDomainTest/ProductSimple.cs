using SqlMapperAttributes;

namespace SqlMapperDomainTest
{
    [TableName("Products")]
    public class ProductSimple
    {
        [PrimaryKey]
        public int ProductID { set; get; }
        public string ProductName { set; get; }
        public string QuantityPerUnit { set; get; }
        public int SupplierID { set; get; }
        public int CategoryID { set; get; }

        public override string ToString() { 
            return string.Format(
                "ProductID: {0}, " + 
                "ProductName: {1}, " + 
                "QuantityPerUnit: {2}, " +
                "SupplierID: {3}, " +
                "CategoryID: {4}",
                ProductID, ProductName, QuantityPerUnit, SupplierID, CategoryID);
        }
    }
}
