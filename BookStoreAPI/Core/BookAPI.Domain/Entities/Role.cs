﻿namespace BookAPI.Domain.Entites
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Member> Members { get; set; }
    }
}
