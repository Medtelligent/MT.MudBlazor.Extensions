using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace MudBlazor;

public class DrawerService : IDrawerService
{
    public event Action<IDrawerReference> OnDrawerInstanceAdded;
    public event Action<IDrawerReference, DrawerResult> OnDrawerCloseRequested;

    public IDrawerReference Show<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>() where T : ComponentBase
    {
        return Show<T>(null, new DrawerParameters(), new DrawerOptions());
    }
    
    public IDrawerReference Show<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(string title) where T : ComponentBase
    {
        return Show<T>(title, new DrawerParameters(), new DrawerOptions());
    }

    public IDrawerReference Show<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(string title, DrawerOptions options) where T : ComponentBase
    {
        return Show<T>(title, new DrawerParameters(), options);
    }

    public IDrawerReference Show<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(string title, DrawerParameters parameters) where T : ComponentBase
    {
        return Show<T>(title, parameters, new DrawerOptions());
    }

    public IDrawerReference Show<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(string title, DrawerParameters parameters, DrawerOptions options) where T : ComponentBase
    {
        return Show(typeof(T), title, parameters, options);
    }

    public IDrawerReference Show([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type contentComponent)
    {
        return Show(contentComponent, null, new DrawerParameters(), new DrawerOptions());
    }

    public IDrawerReference Show([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type contentComponent, string title)
    {
        return Show(contentComponent, title, new DrawerParameters(), new DrawerOptions());
    }

    public IDrawerReference Show([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type contentComponent, string title, DrawerOptions options)
    {
        return Show(contentComponent, title, new DrawerParameters(), options);
    }

    public IDrawerReference Show([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type contentComponent, string title, DrawerParameters parameters)
    {
        return Show(contentComponent, title, parameters, new DrawerOptions());
    }

    public IDrawerReference Show([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type contentComponent, string title, DrawerParameters parameters, DrawerOptions options)
    {
        if (!typeof(ComponentBase).IsAssignableFrom(contentComponent))
        {
            throw new ArgumentException($"{contentComponent?.FullName} must be a Blazor Component");
        }
        
        var drawerReference = CreateReference();

        var drawerContent = new RenderFragment(builder =>
        {
            var i = 0;
            builder.OpenComponent(i++, contentComponent);

            if (!drawerReference.AreParametersRendered)
            {
                foreach (var parameter in parameters)
                {
                    builder.AddAttribute(i++, parameter.Key, parameter.Value);
                }

                drawerReference.AreParametersRendered = true;
            }
            else
            {
                i += parameters.Count;
            }

            builder.AddComponentReferenceCapture(i++, inst => { drawerReference.InjectDrawer(inst); });
            builder.CloseComponent();
        });
        
        var drawerInstance = new RenderFragment(builder =>
        {
            builder.OpenComponent<MudDrawerInstance>(0);
            builder.SetKey(drawerReference.Id);
            builder.AddAttribute(1, "Options", options);
            builder.AddAttribute(2, "Title", title);
            builder.AddAttribute(3, "Content", drawerContent);
            builder.AddAttribute(4, "Id", drawerReference.Id);
            builder.CloseComponent();
        });
        
        drawerReference.InjectRenderFragment(drawerInstance);
        OnDrawerInstanceAdded?.Invoke(drawerReference);

        return drawerReference;
    }

    public void Close(DrawerReference drawer)
    {
        Close(drawer, DrawerResult.Ok<object>(null));
    }

    public virtual void Close(DrawerReference drawer, DrawerResult result)
    {
        OnDrawerCloseRequested?.Invoke(drawer, result);
    }

    public virtual IDrawerReference CreateReference()
    {
        return new DrawerReference(Guid.NewGuid(), this);
    }
}