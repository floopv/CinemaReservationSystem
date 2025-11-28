namespace CinemaReservationSystem.Configurations
{
    public static class MapsterConfiguration
    {
        public static void RegisterMappings(this IServiceCollection services)
        {
            TypeAdapterConfig<ApplicationUser, ApplicationUserVM>.NewConfig()
                .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
        }
    }
}
