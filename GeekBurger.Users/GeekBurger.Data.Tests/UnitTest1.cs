using System;
using Xunit;

namespace GeekBurger.Data.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void InsertInterfaceTest()
        {
            var repo = new UserRepository();
            repo.InsertFace("hdfhdsafpuf8sdap9fup3jfç398fuU9OPJF93JUHÇOJH23ÇJ93JU");
        }
    }
}
