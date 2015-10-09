using SqlMapperAttributes;

namespace SqlMapperDomainTest
{
    [TableName("Products")]
    public class Product
    {
        [PrimaryKey]
        public int ProductID { set; get; }
        public string ProductName { set; get; }
        public string QuantityPerUnit { set; get; }
        public Supplier SupplierID { set; get; }
        public Category CategoryID { set; get; }

        //[PrimaryKey]
        //public int ProductID;
        //public string ProductName;
        //public string QuantityPerUnit;
        //public Supplier SupplierID;
        //public Category CategoryID;

        public override string ToString() { 
            return string.Format(
                "ProductID: {0}, " + 
                "\nProductName: {1}, " +
                "\nQuantityPerUnit: {2}, " +
                "\n\tSupplier: {3}, " +
                "\n\tCategory: {4}\n\n",
                ProductID, ProductName, QuantityPerUnit, SupplierID, CategoryID);
        }
    }
}
