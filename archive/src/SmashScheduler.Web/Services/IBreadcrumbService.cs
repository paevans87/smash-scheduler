using MudBlazor;

namespace SmashScheduler.Web.Services;

public interface IBreadcrumbService
{
    List<BreadcrumbItem> Items { get; }
    public event Action? OnChange;
    void SetBreadcrumbs(List<BreadcrumbItem> items);
}

public class BreadcrumbService : IBreadcrumbService
{
    public event Action? OnChange;
    public List<BreadcrumbItem> Items { get; private set; } = new();

    public void SetBreadcrumbs(List<BreadcrumbItem> items)
    {
        Items = items;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}