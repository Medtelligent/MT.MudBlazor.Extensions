using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace MudBlazor;

public interface IDrawerReference
{
    Guid Id { get; }
    RenderFragment RenderFragment { get; set; }

    bool AreParametersRendered { get; set; }

    Task<DrawerResult> Result { get; }

    void Close();
    void Close(DrawerResult result);

    bool Dismiss(DrawerResult result);

    object Drawer { get; }

    void InjectRenderFragment(RenderFragment rf);

    void InjectDrawer(object inst);

    Task<T> GetReturnValueAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>();
}