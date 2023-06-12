using System;
using System.Collections.Generic;

namespace CSV_Reader.DAL;

public partial class Account
{
    public Account(int customerNumber, string CustomerName, int phone, string system, int outerNumber, string? fsa, string? vip, int? userId)
    {
        CustomerNumber = customerNumber;
        CustomerName = CustomerName;
        Phone = phone;
        System = system;
        OuterNumber = outerNumber;
        Fsa = fsa;
        Vip = vip;
        UserId = userId;
    }

    public int CustomerNumber { get; set; }

    public string CustomerName { get; set; } = null!;

    public int Phone { get; set; }

    public string System { get; set; } = null!;

    public int OuterNumber { get; set; }

    public string? Fsa { get; set; }

    public string? Vip { get; set; }

    public int? UserId { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is Account a)
        {
            if(a.CustomerName == CustomerName)
            {
                return true;
            }
        }

        return false;
    }

}
