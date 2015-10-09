using SqlMapperAttributes;

namespace SqlMapperDomainTest
{
    [TableName("Customers")]
    public class Customers
    {
        [PrimaryKey]
        public int CustomerID { set; get; }
        public string CompanyName { set; get; }
        public string ContactName { set; get; }
        public string ContactTitle { set; get; }
        public string Address { set; get; }
        public string City { set; get; }
        public string Region { set; get; }
        public string PostalCode { set; get; }
        public string Country { set; get; }
        public int Phone { set; get; }

        public override string ToString()
        {
            return string.Format(
                "CustomerID: {0}, " +
                "CompanyName: {1}, " +
                "ContactName: {2}",
                CustomerID, CompanyName, ContactName);
        }
    }
}
