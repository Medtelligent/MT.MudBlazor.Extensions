// Copyright (c) 2019 - Blazored
// Copyright (c) 2020 - Adaptations by Jonny Larsson and Meinrad Recheis

using System.Collections;

namespace MudBlazor;

public class DrawerParameters : IEnumerable<KeyValuePair<string, object>>
{
    private Dictionary<string, object> _parameters;

    public DrawerParameters()
    {
        _parameters = new Dictionary<string, object>();
    }

    public DrawerParameters Add(string parameterName, object value)
    {
        _parameters[parameterName] = value;

        return this;
    }

    public T Get<T>(string parameterName)
    {
        if (_parameters.TryGetValue(parameterName, out var value))
        {
            return (T)value;
        }

        throw new KeyNotFoundException($"{parameterName} does not exist in Drawer parameters");
    }

    public T TryGet<T>(string parameterName)
    {
        if (_parameters.TryGetValue(parameterName, out var value))
        {
            return (T)value;
        }

        return default;
    }

    public int Count =>
        _parameters.Count;

    public object this[string parameterName]
    {
        get => Get<object>(parameterName);
        set => _parameters[parameterName] = value;
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return _parameters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _parameters.GetEnumerator();
    }
}