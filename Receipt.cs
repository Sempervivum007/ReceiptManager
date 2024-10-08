﻿using System;

namespace ReceiptManager
{
    public class Receipt
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public ExpenseType Type { get; set; }
    }
}