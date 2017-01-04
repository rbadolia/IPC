using System.Collections.Generic;
using System.Collections.Specialized;

namespace AutomationAnywhere.Ipc.Common
{
    public interface IObservableList<T> : IList<T>, INotifyCollectionChanged
    {
    }
}
