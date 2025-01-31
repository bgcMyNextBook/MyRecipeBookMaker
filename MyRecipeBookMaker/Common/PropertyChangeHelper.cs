
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyRecipeBookMaker.Common
{
    public static class PropertyChangeHelper
    {
        public static void SubscribeRecursive(this ObservableObject source,
            Action<string, object, PropertyChangedEventArgs> handler)
        {
            var handledObjects = new HashSet<object>();

            void Subscribe(ObservableObject obj)
            {
                if (handledObjects.Contains(obj)) return;

                handledObjects.Add(obj);

                obj.PropertyChanged += (sender, args) =>
                {
                    handler(args.PropertyName, sender, args);

                    if (args.PropertyName != null &&
                        typeof(ObservableCollection<object>).IsAssignableFrom(
                            obj.GetType().GetProperty(args.PropertyName)?.PropertyType))
                    {
                        var collection = obj.GetType()
                            .GetProperty(args.PropertyName)?
                            .GetValue(obj);

                        if (collection is ObservableCollection<object> observableCollection)
                        {
                            foreach (var item in observableCollection)
                            {
                                if (item is ObservableObject observableItem)
                                    Subscribe(observableItem);
                            }

                            observableCollection.CollectionChanged += (sender, args) =>
                            {
                                if (args.NewItems != null)
                                {
                                    foreach (var newItem in args.NewItems)
                                    {
                                        if (newItem is ObservableObject observableNewItem)
                                            Subscribe(observableNewItem);
                                    }
                                }
                            };
                        }
                    }
                };
            }

            Subscribe(source);
        }
    }
}
