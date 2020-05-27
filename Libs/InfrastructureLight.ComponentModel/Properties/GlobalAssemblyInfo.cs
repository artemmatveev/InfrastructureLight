﻿using System.Reflection;
using System.Resources;

[assembly: AssemblyProduct("InfrastructureLight")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyCopyright("Copyright © 2017")]

[assembly: NeutralResourcesLanguage("en-US")]

[assembly: AssemblyVersion("1.0.67.95")]
[assembly: AssemblyFileVersion("1.0.67.95")]

/*
    Нумерация версии ПО: 

    Формат номера версии ("{A}.{B}.{C}.{D}")
    A - главный номер версии (major version number)
    B - вспомогательный номер версии (minor version number)
    C - номер сборки (build number), номер логической итерации по работе над функционалом версии A.B
        Для текущего проекта смотрим на AppVeyor: https://ci.appveyor.com/project/artemmatveev/infrastructurelight/history
    D - Номер ревизии, сквозной номер назначаемый автоматически программным обеспечением хранения версий (SVN).
        Для текущего проекта смотрим номер commita на GitHub
    CиD - можно объединить и указывать номер исправления 

    e - условное обозначение релиза: 
            Pre-alpha (pa) – соответствует этапу начала работ над версией. Характеризуется большими изменениями 
                             в функционале и большим количеством ошибок. Pre-alpha релизы не покидают отдела разработки ПО
            Alpha(a) - соответствует этапу завершения разработки нового функционала. Начиная с alpha версии новый функционал 
                       не разрабатывается, а все заявки на новый функционал уходят в план работ по следующей версии. 
                       Этап характеризуется высокой активностью по тестированию внутри подразделения разработки ПО и устранению ошибок.
            Beta (b) – соответствует этапу публичного тестирования. Это первый релиз, который выходит за пределы 
                       отдела разработки ПО. На этом этапе принимаются замечания от пользователей по интерфейсу 
                       продукта и прочим найденным пользователями ошибкам и неточностям.
            Release Candidate (rc) – весь функционал реализован и полностью оттестирован, 
                                     все найденные на предыдущих этапах ошибки исправлены. На этом этапе могут вноситься изменения 
                                     в документацию и конфигурации продукта.
            Release to manufacturing или Release to marketing (rtm) – служит для индикации того, что ПО соответствует 
                                                                      всем требованиям качества, и готово для массового 
                                                                      распространения. RTM не определяет способа доставки 
                                                                      релиза (сеть или носитель) и служит лишь для индикации 
                                                                      того, что качество достаточно для массового распространения
            General availability (ga) – финальный релиз, соответствующий завершению всех работ по коммерциализации продукта, 
                                        продукт полностью готов к продажам через веб или на физических носителях 
            End of life (eol) – работы по развитию и поддержке продукта завершены  
                 
*/