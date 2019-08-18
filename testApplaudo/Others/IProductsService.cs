using System;
using System.Collections.Generic;
using testApplaudo.Models;

namespace testApplaudo.Others
{
    public interface IProductsService
    {
        List<Products> GetProducts(string sortingParams);
    }
}