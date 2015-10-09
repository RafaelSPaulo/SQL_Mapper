using System;
using SqlMapperAttributes;

namespace SqlMapperDomainTest
{
    [TableName("Orders")]
    public class Orders
    {
        [PrimaryKey]
        public int OrderID { set; get; }
        public int CustomerID { set; get; }
        [PrimaryKey]
        public int EmployeeID { set; get; }
        public DateTime OrderDate { set; get; }
        public DateTime RequiresDate { set; get; }
        public DateTime ShippedDate { set; get; }
        public string ShipVia { set; get; }
        public int Freight { set; get; }
        public string ShipName { set; get; }
        public string ShipAddress { set; get; }
        public string ShipCity { set; get; }
        public string ShipRegion { set; get; }
        public string ShipPostalCode { set; get; }
        public string ShipCountry { set; get; }

        public override string ToString()
        {
            return string.Format(
                "OrderID: {0}, " +
                "CustomerID: {1}, " +
                "EmployeeID: {2}",
                OrderID, CustomerID, EmployeeID);
        }
    }
}
