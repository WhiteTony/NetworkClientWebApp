using Microsoft.EntityFrameworkCore;

class NetworkClientDb : DbContext
{
    public NetworkClientDb(DbContextOptions<NetworkClientDb> options)
        : base(options) { }

    public DbSet<NetworkClient> NetworkClients => Set<NetworkClient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<NetworkClient>()
            .HasData(
            new NetworkClient { Id = 1, Name = "Apple", Ip = "192.168.0.11" , IsActive = false},
            new NetworkClient { Id = 2, Name = "Sons computer", Ip = "192.168.0.21", IsActive = true},
            new NetworkClient { Id = 3, Name = "Dogs Steam Deck", Ip = "192.168.0.34", IsActive = false}
            );  
    }

    // public IResult pingNetworkClient(int id)
    // {
    //   NetworkClient? networkCLient = await db.NetworkClients.FindAsync(id);

    //   if (networkCLient == null)
    //   {
    //     return Results.NotFound();
    //   }
    //   if (networkCLient.Ip == null)
    //   {
    //     return Results.BadRequest("IP address is null.");
    //   }

    //   PingReply? pingReply = await networkCLient.PingAndProcessAsync();
    //   networkCLient.IsActive = pingReply.Status == IPStatus.Success;

    //   return Results.Ok(new {NetworkClient = networkCLient});
    // }
}