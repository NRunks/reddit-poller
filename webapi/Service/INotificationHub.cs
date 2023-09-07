namespace webapi.Service
{
    public interface INotificationHub
    {
        public Task SendMessage(string notification);
    }
}
