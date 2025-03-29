using ClientOpcUaTiaPortal;
using ClientOpcUaTiaPortal.Events;
using ClientOpcUaTiaPortal.item;
using ClientOpcUaTiaPortal.ListbBoxMainApplication;
using System.Windows;
using System.Windows.Controls;
using FluentAssertions;
namespace OPCUATEST
{
    public class OPCUATEST
    {
        private List<dbBlock> _dbBlocks;
        listObjectType _listObjectType;
        private CreateDbBlocks _functionDB;
        private EventWithServer _eventWithServer;
        private ItemIntoDbBlocks _itemIntoDbBlocks;
        //_eventWithServer = new EventWithServer(_dbBlocks);
        //_itemIntoDbBlocks = new ItemIntoDbBlocks(_eventWithServer, _listObjectType);
        //_functionDB = new CreateDbBlocks(_dbBlocks, _itemIntoDbBlocks);

        [Theory]
        [MemberData(nameof(StringLists))]

        public void CheckAddDB(List<string> names)
        {
            
            //given
            _listObjectType = new listObjectType();
            _dbBlocks = new List<dbBlock>();
            int id = 0;
            foreach (var name in names)
            {

                var ob = new dbBlock(_listObjectType._list) { NameDb = name, ind = _dbBlocks.Count };
            //when
            _dbBlocks.Add(ob);
            //then
            _dbBlocks.Should().Contain(a=>a.NameDb==name  && a.ind==id);
                id++;
            }
        }



        //List to test
        public static IEnumerable<object[]> StringLists()
        {
            yield return new object[] { new List<string> { "DB1", "DB2", "DB3" } };
        }


    }
}