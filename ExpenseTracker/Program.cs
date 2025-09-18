using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreManagement
{
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Food,
        Books,
        Sports
    }

    public class Product
    {
        public string Code { get; private set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InStock => Quantity > 0;
        public ProductCategory Category { get; set; }

        public Product(string name, decimal price, int quantity, ProductCategory category)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название товара не может быть пустым");
            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной");
            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным");

            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        public void SetCode(string code)
        {
            Code = code;
        }

        public override string ToString()
        {
            return $"Код: {Code}, Название: {Name}, Цена: {Price:C}, Количество: {Quantity}, " +
                   $"В наличии: {(InStock ? "Да" : "Нет")}, Категория: {Category}";
        }
    }
    public class Sale
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime SaleDate { get; set; }

        public Sale(string productCode, string productName, int quantity, decimal totalAmount)
        {
            ProductCode = productCode;
            ProductName = productName;
            Quantity = quantity;
            TotalAmount = totalAmount;
            SaleDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Дата: {SaleDate:yyyy-MM-dd HH:mm}, Код: {ProductCode}, Товар: {ProductName}, " +
                   $"Количество: {Quantity}, Сумма: {TotalAmount:C}";
        }
    }

    