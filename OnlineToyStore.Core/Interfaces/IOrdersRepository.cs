using OnlineToyStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineToyStore.Core.Interfaces
{
    public interface IOrdersRepository<T> : IRepository<T> where T: Order
    {

    }
}
