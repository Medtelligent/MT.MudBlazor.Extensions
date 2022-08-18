using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MudBlazor;

public partial class MudDrawerProvider : IDisposable
{
    [Inject] private IDrawerService DrawerService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    
    [Parameter] [Category(CategoryTypes.Drawer.Behavior)] public Anchor? Anchor { get; set; }

    [Parameter] [Category(CategoryTypes.Drawer.Appearance)] public Color? Color { get; set; }
    
    [Parameter] [Category(CategoryTypes.Dialog.Behavior)] public bool? DisableBackdropClick { get; set; }

    [Parameter] [Category(CategoryTypes.Drawer.Behavior)] public bool? DisableOverlay { get; set; }
    
    [Parameter] [Category(CategoryTypes.Drawer.Appearance)] public int? Elevation { get; set; }

    [Parameter] [Category(CategoryTypes.Drawer.Appearance)] public string Width { get; set; }

    [Parameter] [Category(CategoryTypes.Drawer.Appearance)] public string Height { get; set; }
    
    private readonly Collection<IDrawerReference> _drawers = new();
    private readonly DrawerOptions _globalDrawerOptions = new();

    protected override void OnInitialized()
    {
        DrawerService.OnDrawerInstanceAdded += AddInstance;
        DrawerService.OnDrawerCloseRequested += DismissInstance;
        NavigationManager.LocationChanged += LocationChanged;

        _globalDrawerOptions.Anchor = Anchor;
        _globalDrawerOptions.Color = Color;
        _globalDrawerOptions.DisableBackdropClick = DisableBackdropClick;
        _globalDrawerOptions.DisableOverlay = DisableOverlay;
        _globalDrawerOptions.Elevation = Elevation;
        _globalDrawerOptions.Width = Width;
        _globalDrawerOptions.Height = Height;
    }

    internal void DismissInstance(Guid id, DrawerResult result)
    {
        var reference = GetDrawerReference(id);
        if (reference != null)
        {
            DismissInstance(reference, result);
        }
    }
    
    private void AddInstance(IDrawerReference drawer)
    {
        _drawers.Add(drawer);
        StateHasChanged();
    }
    
    public void DismissAll()
    {
        _drawers.ToList().ForEach(r => DismissInstance(r, DrawerResult.Cancel()));
        StateHasChanged();
    }
    
    private void DismissInstance(IDrawerReference drawer, DrawerResult result)
    {
        if (!drawer.Dismiss(result))
        {
            return;
        }
    
        _drawers.Remove(drawer);
        StateHasChanged();
    }
    
    private IDrawerReference GetDrawerReference(Guid id)
    {
        return _drawers.SingleOrDefault(x => x.Id == id);
    }
    
    private void LocationChanged(object sender, LocationChangedEventArgs args)
    {
        DismissAll();
    }
    
    public void Dispose()
    {
        if (NavigationManager != null)
        {
            NavigationManager.LocationChanged -= LocationChanged;
        }
    
        if (DrawerService != null)
        {
            DrawerService.OnDrawerInstanceAdded -= AddInstance;
            DrawerService.OnDrawerCloseRequested -= DismissInstance;
        }
    }
}