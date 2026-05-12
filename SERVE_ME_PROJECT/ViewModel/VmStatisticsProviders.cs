namespace SERVE_ME_PROJECT.ViewModel
{
    public class VmStatisticsProviders
    {
        public int TotalOrders { get; set; } = 0;  // إجمالي الطلبات، القيمة الافتراضية هي 0
        public int TotalCustomers { get; set; } = 0;  // إجمالي العملاء، القيمة الافتراضية هي 0
        public int TotalProviders { get; set; } = 0;  // إجمالي مقدمي الخدمات، القيمة الافتراضية هي 0
        public int TotalServices { get; set; } = 0;  // إجمالي الخدمات، القيمة الافتراضية هي 0
        public decimal TotalOrderValue { get; set; } = 0;  // إجمالي قيمة الطلبات، القيمة الافتراضية هي 0
    }
}

