namespace InfrastructureLight.Wpf.Common.Helpers
{
    using Controls;
    using Dialogs;
    using InfrastructureLight.Common.Extensions;
    using MahApps.Metro.Controls;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Text;

    public class DebugHelper
    {
        /// <summary>
        ///     Отображает окно с таблицей свойств и их значений, имеющих указанный тип <see cref="T"/>
        /// </summary>
        /// <typeparam name="T">Тип свойства</typeparam>
        /// <param name="obj">Объект, для которого ищем свойства</param>
        /// <param name="name">Наименование этого объекта</param>
        public static void ShowPropertiesOfType<T>(object obj, string name)
        {
            if (obj == null) return;

            Stack<TypeInfo> pStack = new Stack<TypeInfo>();

            TypeInfo currentType =
                new TypeInfo(obj.GetType().GetProperties().Where(x => !x.GetIndexParameters().Any()).ToArray(),
                    obj, 0, name);

            List<Exception> exceptions = new List<Exception>();

            bool canContinue = true;

            PropertiesContainer container = new PropertiesContainer(currentType.Properties.Length);

            MetroWindow window = null;

            DispatchHelper.Invoke(() =>
            {
                PropertiesView view = new PropertiesView { DataContext = container };
                window = new MetroWindow
                {
                    Content = view,
                    ResizeMode = ResizeMode.CanResizeWithGrip,
                    Title = "Значения свойств типа " + typeof(T),
                    Width = 300,
                    Height = 300,
                    WindowState = WindowState.Maximized
                };
                window.Show();
            });

            while (canContinue)
            {
                try
                {
                    ApplicationHelper.DoEvents();

                    while (currentType.Properties.Length <= currentType.Index)
                    {
                        if (pStack.Any())
                        {
                            currentType.Dispose();
                            currentType = null;
                            GC.Collect();

                            currentType = pStack.Pop();
                        }
                        else
                        {
                            currentType.Dispose();
                            currentType = null;
                            GC.Collect();

                            canContinue = false;

                            break;
                        }
                    }

                    if (!canContinue)
                    {
                        break;
                    }

                    int i = currentType.Index;

                    var target = currentType.Target;

                    PropertyInfo prop = currentType.Properties[i];

                    object val;

                    try
                    {
                        val = prop.GetValue(target);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(new Exception(currentType + "\nParent: " + currentType.ParentPropertyName, ex));
                        currentType.Index = ++i;
                        continue;
                    }

                    if (val == null || val.GetType() == currentType.Target.GetType() ||
                        pStack.Any(x => x.Target == val))
                    {
                        currentType.Index = ++i;
                        continue;
                    }

                    if (val is T)
                    {
                        var type = currentType;
                        window.Dispatcher.Invoke(() => container.ItemsSource.Add(new Property(type.ParentPropertyName, prop.Name, val)));
                        currentType.Index = ++i;
                        continue;
                    }


                    PropertyInfo[] nextProps = currentType.Properties[i].PropertyType.GetProperties()
                        .Where(x => !x.GetIndexParameters().Any()).ToArray();

                    name = currentType.Properties[i].Name;

                    TypeInfo nextType = new TypeInfo(nextProps, val, 0,
                        currentType.ParentPropertyName + "/" + name);

                    currentType.Index = ++i;

                    if (currentType.Target == obj)
                    {
                        ++container.CurrentPropertyIndex;
                    }

                    pStack.Push(currentType);
                    currentType = nextType;
                }
                catch (Exception)
                {
                    MessageDialogHelper.ShowDialog("Возникло исключение",
                        currentType + "\nParent: " + currentType.ParentPropertyName, 400, 300, true,
                        MessageBoxButton.OK, ResizeMode.CanResizeWithGrip, MessageBoxImage.Error);
                }
            }

            if (exceptions.Any())
            {
                var result = MessageBox.Show("Количество исключений: " + exceptions.Count + "\nПоказать?",
                    "В процессе работы возникли исключения", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Exception exception in exceptions)
                    {
                        sb.Append(exception.GetFullErrorInfo());
                    }

                    MessageDialogHelper.ShowDialog("Возникло исключение", sb.ToString(), 400, 300, true,
                        MessageBoxButton.OK, ResizeMode.CanResizeWithGrip, MessageBoxImage.Error);
                }
            }

        }

        internal class TypeInfo : IDisposable
        {            
            public TypeInfo(PropertyInfo[] properties, object target, int index, string parentPropertyNane)
            {
                Properties = properties;
                Target = target;
                Index = index;
                ParentPropertyName = parentPropertyNane;
            }

            public string ParentPropertyName { get; }

            public PropertyInfo[] Properties { get; private set; }

            public int Index { get; set; }
            public object Target { get; private set; }

            public override string ToString()
            {
                return Target.GetType() + " Index: " + Index;
            }

            #region IDispose

            bool _disposed;
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!this._disposed && disposing)
                {
                    Properties = null;
                    Target = null;
                }

                _disposed = true;
            }

            #endregion
        }

        internal class Property
        {
            public Property(string path, string name, object value)
            {
                Path = path;
                Name = name;
                Value = value;
            }

            public string Path { get; }
            public string Name { get; }
            public object Value { get; }
        }

        internal class PropertiesContainer : INotifyPropertyChanged
        {
            private int _currentPropertyIndex;

            public PropertiesContainer(int propertiesCount)
            {
                ItemsSource = new ObservableCollection<Property>();
                PropertiesCount = propertiesCount;
            }

            public ObservableCollection<Property> ItemsSource { get; }

            public int PropertiesCount { get; }

            public int CurrentPropertyIndex
            {
                get => _currentPropertyIndex;
                set
                {
                    _currentPropertyIndex = value;
                    OnPropertyChanged();
                }
            }


            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}