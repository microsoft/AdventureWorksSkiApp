using System;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;

namespace SkiResort.XamarinApp.Views
{
    public class BindablePicker : Picker
    {
        public List<object> TypedItems;

        public BindablePicker()
        {
            TypedItems = new List<object>();

            this.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create<BindablePicker, IEnumerable>(o => o.ItemsSource, default(IEnumerable), propertyChanged: OnItemsSourceChanged);

        public static BindableProperty SelectedItemProperty =
            BindableProperty.Create<BindablePicker, object>(o => o.SelectedItem, default(object), propertyChanged: OnSelectedItemChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            var picker = bindable as BindablePicker;
            picker.Items.Clear();
            picker.TypedItems.Clear();
            if (newvalue != null)
            {
                //now it works like "subscribe once" but you can improve
                foreach (var item in newvalue)
                {
                    picker.Items.Add(item.ToString());
                    picker.TypedItems.Add(item);
                }
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = TypedItems[SelectedIndex];
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as BindablePicker;
            if (newvalue != null)
            {
                picker.SelectedIndex = picker.TypedItems.IndexOf(newvalue);
            }
        }
    }
}
