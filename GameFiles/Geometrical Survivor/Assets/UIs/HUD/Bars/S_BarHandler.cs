using System;

public static class S_BarHandler
{
    public enum BarTypes
    {
        Health,
        Nanomachine
    }

    /// <summary>
    /// Will be invoke when a type of bar needs to be updated.
    /// 
    /// <para> <b> Parameters : </b> </para>
    /// 
    /// <para> - BarTypes | The bar type to update. </para>
    /// <para> - float | The bar new value. </para>
    /// <para> - float | The bar new max value. </para>
    /// </summary>
    public static event Action<BarTypes, float, float> _OnBarValueUpdateEvent;

    /// <summary>
    /// Will invoke the '<see cref = "_OnBarValueUpdateEvent"/>' event Action. That will update the bar of the BarType given.
    /// 
    /// <para> <b> Parameters : </b> </para>
    /// 
    /// <para> - BarTypes | The bar type to update. </para>
    /// <para> - float | The bar new value. </para>
    /// <para> - float | The bar new max value. </para>
    /// </summary>
    public static void NotifyBarValueChange(BarTypes p_barTypes, float p_newValue, float p_maxValue)
    {
        _OnBarValueUpdateEvent?.Invoke(p_barTypes, p_newValue, p_maxValue);
    }
}