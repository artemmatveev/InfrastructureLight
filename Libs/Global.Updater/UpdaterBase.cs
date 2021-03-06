﻿using System;
using System.Diagnostics;
using System.IO;

namespace Global.Updater
{
    using Properties;

    public abstract class UpdaterBase : IUpdater
    {
        private readonly string _targetDirectory;
        private readonly string _sourceDirectory;

        protected UpdaterBase(string appName, string sourceDirectory, string CompanyName)
        {
            _targetDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            _targetDirectory = Path.Combine(_targetDirectory, CompanyName, "Updater", "_data", appName);
            _sourceDirectory = sourceDirectory;
        }

        #region Fields

        /// <summary>
        ///     Путь к директории назначения
        /// </summary>
        public string TargetDirectory
            => _targetDirectory;

        /// <summary>
        ///     Путь к источнику файлов
        /// </summary>
        public string SourceDirectory
            => _sourceDirectory;

        #endregion

        #region Methods

        /// <summary>
        ///     Выполняет обновление программы
        /// </summary>
        public void Run()
        {
            Trace.WriteLine(Resources.traceStartMessage, Resources.traceCategory);
            Trace.WriteLine($"{Resources.traceTargetDirectoryMesage} {_targetDirectory}", Resources.traceCategory);
            DirectoryCopy(_sourceDirectory, _targetDirectory, true);
            Trace.WriteLine(Resources.traceEndMessage, Resources.traceCategory);
        }

        protected virtual void DirectoryCopy(string sourceDirName, string targetDirName, bool copySubDirs) { }

        #endregion
    }
}
