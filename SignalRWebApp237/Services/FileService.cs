
namespace SignalRWebApp237.Services
{
    public class FileService : IFileService
    {
        public async Task<double> Read()
        {
            var data = await File.ReadAllTextAsync("data.txt");
            return double.Parse(data);
        }

        public async Task<double> Read(string room)
        {
            var data = await File.ReadAllTextAsync($"{room}.txt");
            return double.Parse(data);
        }

        public async Task Write(double data)
        {
            await File.WriteAllTextAsync("data.txt", data.ToString());
        }

        public async Task Write(string room, double data)
        {
            await File.WriteAllTextAsync($"{room}.txt",data.ToString());
        }
    }
}
