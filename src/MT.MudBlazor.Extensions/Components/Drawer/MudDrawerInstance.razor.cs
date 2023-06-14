using Microsoft.AspNetCore.Components;
using MudBlazor.Services;

namespace MudBlazor;

public partial class MudDrawerInstance : MudComponentBase, IDisposable
{
    private bool _disposedValue;
    private string _breakpointWidth;
    private DrawerOptions _options = new();
    private readonly string _elementId = string.Concat("drawer_", Guid.NewGuid().ToString().AsSpan(0, 8));

    [CascadingParameter] private MudDrawerProvider Parent { get; set; }
    [CascadingParameter] private DrawerOptions GlobalDrawerOptions { get; set; } = new();
    
    [Inject] private IBreakpointService BreakpointService { get; set; }

    [Parameter]
    [Category(CategoryTypes.Drawer.Behavior)]  // Behavior and Appearance
    public DrawerOptions Options
    {
        get => _options ??= new DrawerOptions();
        set => _options = value;
    }

    [Parameter]
    [Category(CategoryTypes.Dialog.Behavior)]
    public string Title { get; set; }

    [Parameter]
    [Category(CategoryTypes.Drawer.Behavior)]
    public RenderFragment Content { get; set; }

    [Parameter]
    [Category(CategoryTypes.Drawer.Behavior)]
    public Guid Id { get; set; }

    private Anchor Anchor => Options.Anchor ?? GlobalDrawerOptions.Anchor ?? Anchor.Right;
    
    private Color Color => Options.Color ?? GlobalDrawerOptions.Color ?? Color.Default;

    private bool DisableBackdropClick => Options.DisableBackdropClick ?? GlobalDrawerOptions.DisableBackdropClick ?? false;

    private bool DisableOverlay => Options.DisableOverlay ?? GlobalDrawerOptions.DisableOverlay ?? false;

    private bool DrawerOpen { get; set; }
    
    private int Elevation => Options.Elevation ?? GlobalDrawerOptions.Elevation ?? 1;

    private string Width => _breakpointWidth ?? Options.Width ?? GlobalDrawerOptions.Width;

    private string Height => Options.Height ?? GlobalDrawerOptions.Height;

    private DrawerClipMode ClipMode => Options.ClipMode ?? GlobalDrawerOptions.ClipMode ?? DrawerClipMode.Never;

    public void SetOptions(DrawerOptions options)
    {
        Options = options;
        StateHasChanged();
    }

    public void SetTitle(string title)
    {
        Title = title;
        StateHasChanged();
    }

    /// <summary>
    /// Close and return null. 
    /// 
    /// This is a shorthand of Close(DrawerResult.Ok((object)null));
    /// </summary>
    public void Close()
    {
        Close(DrawerResult.Ok<object>(null));
    }

    /// <summary>
    /// Close with dialog result.
    /// 
    /// Usage: Close(DrawerResult.Ok(returnValue))
    /// </summary>
    public void Close(DrawerResult dialogResult)
    {
        DrawerOpen = false;
        
        StateHasChanged();

        InvokeAsync(async () =>
        {
            await Task.Delay(100);
            
            Parent.DismissInstance(Id, dialogResult);
        });
    }

    /// <summary>
    /// Close and directly pass a return value. 
    /// 
    /// This is a shorthand for Close(DrawerResult.Ok(returnValue))
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="returnValue"></param>
    public void Close<T>(T returnValue)
    {
        Close(DrawerResult.Ok(returnValue));
    }

    /// <summary>
    /// Cancel the dialog. DrawerResult.Cancelled will be set to true
    /// </summary>
    public void Cancel()
    {
        Close(DrawerResult.Cancel());
    }

    /// <summary>
    /// Cancels all dialogs in dialog provider collection.
    /// </summary>
    public void CancelAll()
    {
        DrawerOpen = false;
        
        StateHasChanged();

        InvokeAsync(async () =>
        {
            await Task.Delay(100);
        
            Parent?.DismissAll();
        });
    }

    public void ForceRender()
    {
        StateHasChanged();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            _disposedValue = true;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var result = await BreakpointService.Subscribe((breakpoint) =>
            {
                _breakpointWidth = breakpoint == Breakpoint.Xs ? "100%" : null;

                StateHasChanged();
            });
            
            _breakpointWidth = result.Breakpoint == Breakpoint.Xs ? "100%" : null;
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}