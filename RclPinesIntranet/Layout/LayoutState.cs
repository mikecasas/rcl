using Microsoft.AspNetCore.Components;

namespace RclPinesIntranet.Layout;

public class LayoutState
{
    public event Action? OnChange;

    private RenderFragment? _headerContent;

    public RenderFragment? HeaderContent
    {
        get => _headerContent;
        set
        {
            _headerContent = value;
            OnChange?.Invoke();
        }
    }

    public void ClearHeader()
    {
        HeaderContent = null;
    }
}
