using DeliveryApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Ui;

public class Settings
{
    public string CONNECTION_STRING { get; set; }
    public string RABBIT_MQ_HOST { get; set; }
}
