using System.Net.NetworkInformation;

public class NetworkClient
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public string? Ip { get; set; }

  public string? Mac { get; set; }
  public bool IsActive { get; set; }	

  public async Task<PingReply> PingAndProcessAsync()
  {
    Ping pingSender = new Ping();

    if (null != Ip)
    {
      return await pingSender.SendPingAsync(Ip, 2000);
    }
    else
    {
      throw new ArgumentNullException("IP address is null.");
    }
  }

  public async Task<bool> PowerOnAsync()
  {
    PingReply? pingReply = await PingAndProcessAsync();
    IsActive = pingReply.Status == IPStatus.Success;
    return IsActive;
  }
}
