
namespace CorporationSyncify.Identity.WebApi.BackgroundJobs
{
    public interface IProcessOutboxMessagesJob
    {
        Task ProcessAsync();
    }
}