﻿using BookShopping_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppinhg_Project.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository:iRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
    }
}
