using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace MudBlazor;

public interface IDrawerService
{
    public event Action<IDrawerReference> OnDrawerInstanceAdded;
    public event Action<IDrawerReference, DrawerResult> OnDrawerCloseRequested;

    IDrawerReference Show<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>() where TComponent : ComponentBase;
    
    IDrawerReference Show<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>(string title) where TComponent : ComponentBase;

    IDrawerReference Show<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>(string title, DrawerOptions options) where TComponent : ComponentBase;
        
    IDrawerReference Show<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>(string title, DrawerParameters parameters) where TComponent : ComponentBase;

    IDrawerReference Show<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>(string title, DrawerParameters parameters, DrawerOptions options) where TComponent : ComponentBase;

    IDrawerReference Show([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type component);
    
    IDrawerReference Show([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type component, string title);

    IDrawerReference Show([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type component, string title, DrawerOptions options);

    IDrawerReference Show([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type component, string title, DrawerParameters parameters);

    IDrawerReference Show([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type component, string title, DrawerParameters parameters, DrawerOptions options);

    IDrawerReference CreateReference();

    void Close(DrawerReference drawer);

    void Close(DrawerReference drawer, DrawerResult result);
}