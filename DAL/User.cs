using System;
using System.Collections.Generic;

namespace CSV_Reader.DAL;

public partial class User
{
    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Id { get; set; }
}
