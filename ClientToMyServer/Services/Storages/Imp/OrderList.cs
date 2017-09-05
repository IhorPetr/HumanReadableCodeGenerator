using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClientToMyServer.Domain;
using ClientToMyServer.Services.ServerConfigs;

namespace ClientToMyServer.Services.Storages.Imp
{
    public class OrderList : IOrderList
    {
        private List<Order> _orderlist;
        private readonly IMapper _map;
        private readonly IHttpCodeWriter _httpwriter;
        public OrderList(IMapper _map, IHttpCodeWriter _httpwriter)
        {
            this._map = _map;
            this._httpwriter = _httpwriter;
            _orderlist = new List<Order>
            {
                new Order
                {
                    CreateDate=DateTime.Now,
                    CreatorId="Budy@mail.com",
                    Id=1,
                    IsDeleted=false,
                    OrderDetails="2 statues of Sonic",
                    Status=OrderStatus.Promoted,
                },
                new Order
                {
                    CreateDate=DateTime.Now,
                    CreatorId="Budy@mail.com",
                    Id=2,
                    IsDeleted=false,
                    OrderDetails="A Cup",
                    Status=OrderStatus.Promoted,
                }
            };
        }
        public async Task<IList<Order>> GetAll()
        {
            await _httpwriter.Authentificate();
            foreach (var t in _orderlist)
            {
                t.Code = await _httpwriter.GetCodeById(t.Id);
            }
            return _orderlist;
        }
        public Order Get(int id)
        {
            return _orderlist.FirstOrDefault(r => r.Id == id);
        }
        public bool Create(Order order)
        {
           // var g = _map.Map<Order>(order);
            try
            {
                _orderlist.Add(order);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Update(int id, Order value)
        {
            try
            {
                //var g = _map.Map<Order>(value);
                var res = _orderlist.FirstOrDefault(r => r.Id == id);
                if (res != null)
                {
                    _orderlist.RemoveAt(id);
                    value.Id = id;
                    _orderlist.Add(value);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var res = _orderlist.FirstOrDefault(r => r.Id == id);
                _orderlist.RemoveAt(res.Id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
