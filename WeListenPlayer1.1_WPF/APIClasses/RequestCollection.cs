using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeListenPlayer1._1_WPF.APIClasses
{
    class RequestCollection : ObservableCollection<SongData>
    {
        public void CopyFrom(IEnumerable<SongData> requests)
        {
            this.Items.Clear();
            foreach (var r in requests)
            {

                this.Items.Add(r);
            }

            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        
    }
}
