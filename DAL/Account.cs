using System;
using System.Collections.Generic;

namespace CSV_Reader.DAL;

public partial class Account
{
    public Account(int costumerNumber, string costumerName, int phone, string system, int outerNumber, string? fsa, string? vip, int? userId)
    {
        CostumerNumber = costumerNumber;
        CostumerName = costumerName;
        Phone = phone;
        System = system;
        OuterNumber = outerNumber;
        Fsa = fsa;
        Vip = vip;
        UserId = userId;
    }

    public int CostumerNumber { get; set; }

    public string CostumerName { get; set; } = null!;

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
            if(a.CostumerNumber == CostumerNumber)
            {
                return true;
            }
        }

        return false;
    }

}
