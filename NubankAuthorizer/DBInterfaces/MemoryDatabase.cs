using System.Collections.Generic;
using System.Linq;

namespace NubankAuthorizer.DBInterfaces
{
    public class MemoryDatabase<TItemType> : IDatabase<TItemType>
    {
        private readonly List<TItemType> databaseItem;
        
        public MemoryDatabase()
        {
            databaseItem = new List<TItemType>();
        }

        public void Save(TItemType data)
        {
            databaseItem.Add(data);
        }

        public TItemType FindOne()
        {
            return databaseItem.FirstOrDefault();
        }

        public List<TItemType> GetAll()
        {
            return databaseItem;
        }
    }
}