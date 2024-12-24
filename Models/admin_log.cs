using System.ComponentModel.DataAnnotations;
using System;

public class admin_log
{
    [Key]
    public int log_id { get; set; }
    public int admin_id { get; set; }
    public string action { get; set; }
    public DateTime change_date { get; set; }
}
