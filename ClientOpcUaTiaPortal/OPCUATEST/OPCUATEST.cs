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
        public OPCUATEST()
        {


        }
        [Fact]
        public void CheckAddDB()
        {

            //given
            _listObjectType = new listObjectType();
            _dbBlocks = new List<dbBlock>();
            var ob = new dbBlock(_listObjectType._list) { NameDb = "DB1", ind = _dbBlocks.Count };
            //when
            _dbBlocks.Add(ob);
            //then
            _dbBlocks.Should().Contain(a=>a.NameDb=="DB1" );
        }
    }
}