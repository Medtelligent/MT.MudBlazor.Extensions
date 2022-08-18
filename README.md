# MT.MudBlazor.Extensions
Extensions to MudBlazor component library

### Using

Add the nuget Package `MT.MudBlazor.Extensions` to your blazor project.

The extensions have been built within the MudBlazor namespace so as long as you're importing that in your `_Imports.razor` file you're good to go.

In your **`_Imports.razor`**
```csharp
@using MudBlazor
```

## DrawerService

A `MudDrawer` is a panel that is overlaid on top of a page and slides in from the side.  This provides another great option for rendering forms or just informational content.

Generally, drawers are used for navigation but it's just another container for any type of content just like the `MudDialog`.  
Unlike the `MudDialog`, however, the drawer can currently only be inline, embedded in the page from where it is to be triggered.

This poses a problem when you want to abstract the content of a drawer for say a form in it's own component and show ad-hoc when a button is clicked the way dialogs work with the [IDialogService](https://mudblazor.com/components/dialog#usage)

`IDrawerService` provides `IDialogService` like functionality but for drawers!

### Register DrawerService in ServiceProvider

In your **`Program.cs`** or wherever you configure your services
```csharp
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = await WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddMudServices()
    .AddMudBlazorDrawerService();
```

### Add MudProvider component to layout

In your **`Layout.razor`**
```html
<MudLayout>
    <MudDrawerProvider Width="600px" />
</MudLayout>
```

A subset of original Drawer options can be configured globally as default options for all drawers that are opened with the `IDrawerService`


### Inject IDrawerService in your component
`@inject IDialogService DialogService`

### Create component to render as content in drawer

In **`DrawerPane.razor`**
```csharp
<MudContainer>
    This is some content in the pane.  The message passed here is <strong>@Message</strong>
</MudContainer>

<MudDivider Class="my-5" />

<MudContainer>
    <MudStack Row="true" Justify="Justify.SpaceBetween">
        <MudButton OnClick="Close" Variant="Variant.Filled">Close</MudButton>
        <MudButton Color="Color.Primary" OnClick="Submit" Variant="Variant.Filled">Submit</MudButton>
    </MudStack>
</MudContainer>

@code {
    [CascadingParameter] 
    private MudDrawerInstance DrawerPane { get; set; }
    
    [Parameter] 
    public string Message { get; set; }

    private void Submit()
    {
        DrawerPane.Close();
    }

    private void Close()
    {
        DrawerPane.Cancel();
    }
}
```

### Show the drawer from a page

In your **`DrawerTestPage.razor`**
```csharp
@page "/DrawerTest"

<MudPaper Height="1200px">
    <MudStack>
        <MudPaper Class="pa-3">
            Click <MudButton Variant="Variant.Outlined" OnClick="OpenServiceDrawer">here</MudButton> to trigger a test DrawerService drawer.
        </MudPaper>
        <MudPaper Class="pa-3">
            Click <MudButton Variant="Variant.Outlined" OnClick="OpenInlineDrawer">here</MudButton> to trigger a test inline drawer on this page.
        </MudPaper>
    </MudStack>

    <MudDrawer Open="@DrawerOpen" OpenChanged="v => DrawerOpen = v" Anchor="Anchor.Right" ClipMode="DrawerClipMode.Never" Width="600px" Variant="DrawerVariant.Temporary">
        <MudDrawerHeader>
            <MudText Typo="Typo.h6">Inline Test Drawer</MudText>
        </MudDrawerHeader>
        <MudContainer>
            This is an inline temporary drawer to demonstrate difference with the one with drawer service
        </MudContainer>
    </MudDrawer>
</MudPaper>

@code {
    private bool DrawerOpen { get; set; }
    
    private async Task OpenServiceDrawer()
    {
        var drawer = DrawerService.Show<DrawerContent>("Test Drawer", new DrawerParameters().Add("Message", "Hello world!"), new DrawerOptions());

        var result = await drawer.Result;

        if (!result.Cancelled)
        {
            SnackBar.Add("Drawer was closed on action", Severity.Success);
        }
    }

    private void OpenInlineDrawer()
    {
        DrawerOpen = !DrawerOpen;
    }
}
```


## Change Log
- 0.0.1 Initial commit

## Links
[Github Repository](https://github.com/Medtelligent/MT.MudBlazor.Extensions) |
[Nuget Package](https://www.nuget.org/packages/MT.MudBlazor.Extensions/)