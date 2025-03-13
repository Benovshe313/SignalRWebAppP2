namespace SignalRWebApp237.Services
{
    public interface IFileService
    {
        Task<double> Read();
        Task Write(double data);
        Task<double> Read(string room);
        Task Write(string room,double data);
    }
}
