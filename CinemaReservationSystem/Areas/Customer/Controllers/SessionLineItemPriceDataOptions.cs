





namespace CinemaReservationSystem.Areas.Customer.Controllers
{
    internal class SessionLineItemPriceDataOptions : Stripe.Checkout.SessionLineItemPriceDataOptions
    {
        public string Currency { get; set; }
        public object ProductData { get; set; }
        public long UnitAmount { get; set; }
    }
}