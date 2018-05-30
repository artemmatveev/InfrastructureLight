using System;
using System.Collections.Generic;
using InfrastructureLight.Wpf.ViewModels;

namespace InfrastructureLight.Wpf.AutoSave.Interfaces
{    
    public interface IAutoRowSaver
    {
        /// <summary>
        ///     Добавляет элемент в коллекцию ItemsSource
        /// </summary>
        /// <typeparam name="T">Тип модели представления строки</typeparam>
        /// <param name="item">Добавляемый элемент</param>
        /// <param name="itemsSource">Коллекция-источник</param>
        void AddItem<T>(T item, ICollection<T> itemsSource) where T : ViewModelBase;
        /// <summary>
        ///     Показывает, есть ли ошибки валидации
        /// </summary>
        bool GridHasErrors();
        /// <summary>
        ///     Удаляет элемент, и пытается сохранить те, кторые были с ошибками
        /// </summary>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <param name="item">Элемент</param>
        /// <param name="itemsSource">Коллекция-источник</param>
        /// <param name="saveToRepositoryMethod">Метод для сохранения в хранилище</param>
        void Remove<T>(T item, ICollection<T> itemsSource, Action<T> saveToRepositoryMethod) where T : ViewModelBase, IRaisingPropertyChanged;
        /// <summary>
        ///     Устанавливает текущий элемент как отредактированный (для срабатывания сохранения)
        /// </summary>
        void SetCurrentAsEdited();
        /// <summary>
        ///     Пытается сохранить элемент, и за одно, все, которые были с ошибками
        /// </summary>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <param name="item">Элемент</param>
        /// <param name="saveToRepositoryMethod">Метод для сохранения в хранилище</param>
        /// <returns></returns>
        bool TrySave<T>(T item, Action<T> saveToRepositoryMethod) where T : ViewModelBase;
    }
}