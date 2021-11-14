using System.Collections.Generic;

namespace NubankAuthorizer.DBInterfaces
{
    public interface IDatabase<TItemType>
    {
        public void Save(TItemType data);                                                     
        public TItemType FindOne();
        public List<TItemType> GetAll();
    }
}