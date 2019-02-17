namespace InfrastructureLight.Wpf.Dialogs
{
    public interface IDialogSettings
    {
        /// <summary>
        ///     Заголовок диалога
        /// </summary>
        string Title { get; set; }

        /// <summary>
        ///     Ширина диалога
        /// </summary>
        double DialogWidth { get; set; }

        /// <summary>
        ///     Высота диалога
        /// </summary>
        double DialogHeight { get; set; }

        /// <summary>
        ///     Цвет диалогового окна
        /// </summary>
        string Color { get; set; }
    }
}
