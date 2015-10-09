using System;
using SqlMapperAttributes;

namespace SqlMapperDomainTest
{
    [TableName("Employees")]
    public class Employees
    {
        [PrimaryKey]
        public int EmployeeID { set; get; }
        public string LastName { set; get; }
        public string FirstName { set; get; }
        public string Title { set; get; }
        public string TitleOfCourtesy { set; get; }
        public DateTime BirthDate { set; get; }
        public DateTime HireDate { set; get; }
        public string Address { set; get; }
        public string City { set; get; }
        public string Region { set; get; }
        public string PostalCode { set; get; }

        public override string ToString()
        {
            return string.Format(
                "EmployeeID: {0}, " +
                "Name: {1} {2}",
                EmployeeID, FirstName, LastName);
        }
    }
}
