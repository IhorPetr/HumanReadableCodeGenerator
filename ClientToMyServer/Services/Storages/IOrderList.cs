using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientToMyServer.Domain;

namespace ClientToMyServer.Services.Storages
{
    public interface IOrderList
    {
        Task<IList<Order>> GetAll();
        Order Get(int id);
        bool Create(Order order);
        bool Update(int id, Order value);
        bool Delete(int id);
    }
}
