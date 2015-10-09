using SqlMapperAttributes;

namespace SqlMapperDomainTest
{
    [TableName("Categories")]
    public class Category
    {
        [PrimaryKey]
        public int CategoryID { set; get; }
        public string CategoryName { set; get; }
        public string Description { set; get; }

        //[PrimaryKey]
        //public int CategoryID;
        //public string CategoryName;
        //public string Description;
        
        public override string ToString() { 
            return string.Format(
                "CategoryID: {0}, " + 
                "CategoryName: {1}, " + 
                "Description: {2}",
                CategoryID, CategoryName, Description);
        }
    }
}
