﻿using System;
using System.Windows;

namespace InfrastructureLight.Wpf.Dialogs
{
    using Message;
    using ViewModels;

    public interface IDialogVisualizer
    {
        /// <summary>
        ///     Отобразить окно для ViewModel
        /// </summary>        
        void Show<T>(T viewModel, IDialogSettings dialogSettings = null, Action<T> callback = null) where T : ViewModelBase;

        /// <summary>
        ///     Отобразить окно для ViewModel
        ///     и задать владельца
        /// </summary>        
        void Show<T, TOwner>(T viewModel, TOwner owner, IDialogSettings dialogSettings = null, Action<T> callback = null) where T : ViewModelBase where TOwner : ViewModelBase;

        /// <summary>
        ///     Отобразить ДИАЛОГОВОЕ окно для ViewModel
        /// </summary>        
        bool? ShowDialog<T>(T viewModel, IDialogSettings dialogSettings = null, Action<T> callback = null) where T : ViewModelBase;

        /// <summary>
        ///     Отобразить ДИАЛОГОВОЕ окно для ViewModel
        ///     и задать владельца
        /// </summary>        
        bool? ShowDialog<T, TOwner>(T viewModel, TOwner owner, IDialogSettings dialogSettings = null, Action<T> callback = null) where T : ViewModelBase where TOwner : ViewModelBase;

        /// <summary>
        ///     Отобразить диалоговое окно в стиле сообщения
        /// </summary>
        bool? ShowDialog(string title, string content, ResizeMode resizeMode = ResizeMode.NoResize, MessageBoxImage messageBoxImage = MessageBoxImage.None, params DialogButton[] buttons);

        /// <summary>
        ///     Закрывает окно, которое сопоставлено заданной модели представления
        /// </summary>
        /// <param name="viewModel">Модель представления</param>
        /// <typeparam name="T">Тип модели представления</typeparam>
        void Close<T>(T viewModel) where T : ViewModelBase;
    }
}
