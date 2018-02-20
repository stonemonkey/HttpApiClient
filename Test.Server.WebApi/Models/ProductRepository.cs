// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Test.Server.WebApi.Models
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products = new List<Product>();
        private int _nextId = 1;

        public ProductRepository()
        {
            Add(new Product { Name = "Lenovo IdeaPad", Category = "Electronics", Price = 19500M });
            Add(new Product { Name = "Knife", Category = "Kitchen", Price = 2.25M });
            Add(new Product { Name = "Screwdriver", Category = "Tools", Price = 12.49M });
            Add(new Product { Name = "iPhone 7S", Category = "Electronics", Price = 7950M });
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product Get(int id)
        {
            return _products.Find(p => p.Id == id);
        }

        public Product Add(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            item.Id = _nextId++;
            _products.Add(item);
            return item;
        }

        public void Remove(int id)
        {
            _products.RemoveAll(p => p.Id == id);
        }

        public bool Update(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int index = _products.FindIndex(p => p.Id == item.Id);
            if (index == -1)
            {
                return false;
            }
            _products.RemoveAt(index);
            _products.Add(item);

            return true;
        }
    }
}