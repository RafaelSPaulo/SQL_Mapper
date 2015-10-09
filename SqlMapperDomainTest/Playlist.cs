using System;
using SqlMapperAttributes;

namespace SqlMapperDomainTest
{
    [TableName("PlaylistTable")]
    public class Playlist
    {
        [PrimaryKey]
        public string name { set; get; }
        public string ownerName { set; get; }
        public DateTime creationDate { set; get; }
        public string description { set; get; }

        public override string ToString()
        {
            return string.Format(
                "Name: {0}, " +
                "Owner: {1}, " +
                "CreationDate: {2}" +
                "Description: {3}",
                name, ownerName, creationDate, description);
        }
    }
}
