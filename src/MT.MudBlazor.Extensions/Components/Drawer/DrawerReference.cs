// Copyright (c) 2019 Blazored (https://github.com/Blazored)
// Copyright (c) 2020 Jonny Larsson (https://github.com/MudBlazor/MudBlazor)
// Copyright (c) 2021 improvements by Meinrad Recheis
// See https://github.com/Blazored
// License: MIT

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace MudBlazor;

public class DrawerReference : IDrawerReference
{
    private readonly TaskCompletionSource<DrawerResult> _resultCompletion = new();

    private readonly IDrawerService _dialogService;

    public DrawerReference(Guid dialogInstanceId, IDrawerService dialogService)
    {
        Id = dialogInstanceId;
        _dialogService = dialogService;
    }

    public void Close()
    {
        _dialogService.Close(this);
    }

    public void Close(DrawerResult result)
    {
        _dialogService.Close(this, result);
    }

    public virtual bool Dismiss(DrawerResult result)
    {
        return _resultCompletion.TrySetResult(result);
    }

    public Guid Id { get; }

    public object Drawer { get; private set; }
    public RenderFragment RenderFragment { get; set; }

    public Task<DrawerResult> Result => _resultCompletion.Task;

    public bool AreParametersRendered { get; set; }

    public void InjectDrawer(object inst)
    {
        Drawer = inst;
    }

    public void InjectRenderFragment(RenderFragment rf)
    {
        RenderFragment = rf;
    }

    public async Task<T> GetReturnValueAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>()
    {
        var result = await Result;
        try
        {
            return (T)result.Data;
        }
        catch (InvalidCastException)
        {
            Debug.WriteLine($"Could not cast return value to {typeof(T)}, returning default.");
            return default;
        }
    }

}