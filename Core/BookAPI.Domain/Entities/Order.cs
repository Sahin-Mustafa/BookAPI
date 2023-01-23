﻿namespace BookAPI.Domain.Entites
{
    public class Order
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public DateTime OrderDate { get; set; }= DateTime.Now;
        //navigation properties
        public Address Address { get; set; }
        public List<ShoppingCard> ShoppingCards { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
