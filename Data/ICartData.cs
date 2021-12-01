using DAW_MP2.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAW_MP2.Data
{
    public interface ICartData
    {
        Task<List<CartRow>> GetCart(Guid id);

        void CreateCart(Guid id);

        void AddToCart(Guid itemId, Guid userId);

        void RemoveFromCart(Guid itemId,  Guid userId);

        void RemoveAllFromCart(Guid itemId, Guid userId);

        void FinishTransaction(Guid userId, Cart cart);

        Cart Payment(Guid userId);
    }

}
