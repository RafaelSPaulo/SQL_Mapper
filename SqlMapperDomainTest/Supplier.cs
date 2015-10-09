using SqlMapperAttributes;

namespace SqlMapperDomainTest
{
    [TableName("Suppliers")]
    public class Supplier
    {
        [PrimaryKey]
        public int SupplierID { set; get; }
        public string CompanyName { set; get; }
        public string ContactName { set; get; }

        //[PrimaryKey]
        //public int SupplierID;
        //public string CompanyName;
        //public string ContactName;

        public override string ToString() { 
            return string.Format(
                "SupplierID: {0}, " +
                "CompanyName: {1}, " +
                "ContactName: {2}",
                SupplierID, CompanyName, ContactName);
        }
    }
}
