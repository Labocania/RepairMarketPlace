﻿using RepairMarketPlace.ApplicationCore.Entities;
using System;
using System.Threading.Tasks;

namespace RepairMarketPlace.ApplicationCore.Interfaces
{
    public interface IShopService
    {
        public Task CreateShopAsync(Guid userId, string name, string address, string email, string phoneNumber, string webSite);
        public Task<Shop> GetShopAsync(Guid userId);
        public Task UpdateShopAsync(Shop shop);
    }
}
