namespace NIIPP.DatabaseClient.DataStorage
{
    /// <summary>
    /// Класс содержит данные для подключения SQL службы к серверу
    /// </summary>
    public static class ConnectionSettings
    {
        /// <summary>
        /// IP адрес сервера
        /// </summary>
        public static string ServerIp = "";

        /// <summary>
        /// Название пользователя SQL 
        /// </summary>
        public static string UserId = "";

        /// <summary>
        /// Пароль пользователя SQL
        /// </summary>
        public static string Password = "";

        /// <summary>
        /// Название базы данных
        /// </summary>
        public static string DatabaseName = "";
    }

    /// <summary>
    /// Класс содержит сведения относительно таблицы "tb_materials",
    /// которая хранит информацию об исходном материале - новых пластинах
    /// </summary>
    public static class TbMaterials
    {
        /// <summary>
        /// Название таблицы
        /// </summary>
        public const string Name = "tb_materials";

        /// <summary>
        /// Идентификатор записи 
        /// </summary>
        public const string MaterialId = "id";

        /// <summary>
        /// Название партии пластин
        /// </summary>
        public const string NumberOfParcel = "parcel_number";

        /// <summary>
        /// Порядковый номер пластины в партии
        /// </summary>
        public const string NumberOfWafer = "wafer_number";

        /// <summary>
        /// Дата создания записи
        /// </summary>
        public const string CreationDate = "time_material";

        /// <summary>
        /// Автор записи
        /// </summary>
        public const string AuthorOfRecord = "author_material";

        /// <summary>
        /// Комментарий для данной пластины
        /// </summary>
        public const string Comment = "comment";

        /// <summary>
        /// Технология для которой предназначается пластина (pin и т. д.)
        /// </summary>
        public const string Technology = "technology";

        /// <summary>
        /// Техпроцесс (12, 20, и т. д.) - толщина
        /// </summary>
        public const string TechProc = "tech_proc";

        /// <summary>
        /// Производитель структуры на пластине
        /// </summary>
        public const string StructureManufacturer = "structure_manufacturer";

        /// <summary>
        /// Производитель пластины (AXT, WT, и т. д.)
        /// </summary>
        public const string WaferManufacturer = "wafer_manufacturer";

        /// <summary>
        /// Идентификатор слитка
        /// </summary>
        public const string Ingot = "ingot";

        /// <summary>
        /// Пластина соответствует...
        /// </summary>
        public const string Corresponds = "correspond";

        /// <summary>
        /// Была ли данная запись подтверждена пользователем
        /// </summary>
        public const string VerificationStatus = "verification_status";

        /// <summary>
        /// Статус - в запуске или не запущена
        /// </summary>
        public const string LaunchedStatus = "launched_status";

        /// <summary>
        /// Название таблицы связанной с этой записью и содержащей информацию о слоях эпитаксиальной структуры
        /// </summary>
        public const string NameOfTableWithEpitStructure = "link_to_epit_struct";

        /// <summary>
        /// Концетрация легирующей примеси в подложке
        /// </summary>
        public const string ConcentrationInWafer = "wafer_conc";

        /// <summary>
        /// Тип подложки по концентрации и составу легирующей примеси (SI, n+, p+, n, p)
        /// </summary>
        public const string TypeOfWafer = "wafer_type";

        /// <summary>
        /// Толщина подложки в мкм
        /// </summary>
        public const string ThicknessOfWafer = "wafer_thick";

        /// <summary>
        /// Диаметр подложки в дюймах
        /// </summary>
        public const string WaferDiameter = "wafer_diameter";

        /// <summary>
        /// Сопротивление подложки в омах
        /// </summary>
        public const string WaferResistance = "wafer_resistance";

        /// <summary>
        /// Материал из которго изготовлена подложка
        /// </summary>
        public const string MaterialOfWafer = "wafer_material";

        /// <summary>
        /// Значение поля ID записи о файле со сканом паспорта эпитаксиальных структур, 
        /// в таблице "tb_files_pool", содержащей реестр файлов на сервере
        /// </summary>
        public const string RecordIdOfEpitStructureFile = "link_to_file_pass";

        /// <summary>
        /// Значение поля ID записи о файле со сканом паспорта эпитаксиальных структур, 
        /// в таблице "tb_files_pool", содержащей реестр файлов на сервере
        /// </summary>
        public const string RecordIdOfAttachmentFile = "link_to_file_some";
    }

    /// <summary>
    /// Класс содержит сведения относительно таблицы "tb_files_pool",
    /// которая хранит реестр файлов на сервере
    /// </summary>
    public static class TbFilesPool
    {
        /// <summary>
        /// Название таблицы
        /// </summary>
        public const string Name = "tb_files_pool";

        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public const string FileID = "id";

        /// <summary>
        /// Название файла
        /// </summary>
        public const string NameOfFile = "name_of_file";

        /// <summary>
        /// Дата и время загрузки файла на сервер
        /// </summary>
        public const string DateOfUpload = "date_of_upload";

        /// <summary>
        /// Размер файла в мегабайтах
        /// </summary>
        public const string SizeInMb = "size_in_Mb";
    }

    /// <summary>
    /// Класс содержит сведения относительно группы таблицы "tb_z_epit || 'materialId'",
    /// которая хранит слои эпитаксиальной структуры подложки
    /// </summary>
    public static class TbEpitStructure
    {
        public const string NamePrefix = "tb_z_epit || ";
        /// <summary>
        /// Название таблицы
        /// </summary>
        public static string Name(string materialId)
        {
            return NamePrefix + materialId;
        }

        /// <summary>
        /// Идентификатор записи и порядковый номер слоя (от подложки). 0 слой - подложка
        /// </summary>
        public const string Number = "number";

        /// <summary>
        /// Тип слоя (n+, p+, SI и т. д.)
        /// </summary>
        public const string TypeOfLayer = "name";

        /// <summary>
        /// Материал из которого состоит слой (GaAs, GaN и т. д.)
        /// </summary>
        public const string Material = "material";

        /// <summary>
        /// Толщина слоя (в мкм)
        /// </summary>
        public const string Thickness = "thick";

        /// <summary>
        /// Концентрация примеси в слое (1 / см^3)
        /// </summary>
        public const string Concentration = "conc";
    }

    /// <summary>
    /// Класс содержит сведения относительно таблицы "tb_epit_host",
    /// которая хранит реестр таблиц со слоями эпитаксиальной структуры
    /// </summary>
    public static class TbEpitHost
    {
        /// <summary>
        /// Название таблицы
        /// </summary>
        public const string Name = "tb_epit_host";

        /// <summary>
        /// Название связанной таблицы в которой содержится информация о слоях эпитаксиальной структуры
        /// и одновременно идентификатор записи
        /// </summary>
        public const string NameOfEpitStructureTable = "name_of_table";

        /// <summary>
        /// Пользовательское название таблицы со слоями эпитаксиальной структуры
        /// </summary>
        public const string UserName = "name_for_user";

        /// <summary>
        /// Дата и время создания таблицы со слоями эпитаксиальной структуры
        /// </summary>
        public const string TimeOfCreation = "time_of_creation";
    }

    /// <summary>
    /// Класс содержит сведения относительно таблицы "tb_route_lists",
    /// которая хранит информацию о сопроводных листах
    /// </summary>
    public static class TbRouteLists
    {
        /// <summary>
        /// Название таблицы
        /// </summary>
        public const string Name = "tb_route_lists";

        /// <summary>
        /// Название сопроводного листа
        /// </summary>
        public const string NameOfList = "name_of_list";

        /// <summary>
        /// Количество пластин связанных с этим сопроводным листом
        /// </summary>
        public const string CountOfMaterials = "count_of_materials";

        /// <summary>
        /// Название комплекта фотошаблонов
        /// </summary>
        public const string SetOfMasks = "set_of_masks";
        
        /// <summary>
        /// Фамилия и имя назначенного технолога
        /// </summary>
        public const string Technologist = "technologist";

        /// <summary>
        /// Название пластины либо пластин которые связаны с этим сопроводным листом
        /// </summary>
        public const string Materials = "materials";
        
        /// <summary>
        /// Номер заказа
        /// </summary>
        public const string NumberOfOrder = "number_of_order";

        /// <summary>
        /// Дата создания сопроводного листа
        /// </summary>
        public const string DateOfCreation = "date_of_creation";

    }

    /// <summary>
    /// Класс содержит сведения относительно таблицы "tb_set_of_masks",
    /// которая хранит информацию о комплектах фотошаблонов
    /// </summary>
    public static class TbSetOfMasks
    {
        /// <summary>
        /// Название таблицы
        /// </summary>
        public const string Name = "tb_set_of_masks";

        /// <summary>
        /// Название комплекта фотошаблонов и одновременно идентификатор данного комплекта фотошаблонов
        /// </summary>
        public const string NameOfSetOfMasksId = "name";

        /// <summary>
        /// Описание комплекта фотошаблонов
        /// </summary>
        public const string Description = "description";

        /// <summary>
        /// Технология которую обеспечивает набор фотошаблонов (pHEMT, mHEMT, GaN, ДБШ, pin, Пассивные МИС)
        /// </summary>
        public const string Technology = "technology";

        /// <summary>
        /// Техпроцесс (12, 20, и т. д.) - толщина
        /// </summary>
        public const string TechProc = "tech_proc";

        /// <summary>
        /// Дата и время создания записи
        /// </summary>
        public const string TimeOfRecordCreation = "time_of_record_creation";

        /// <summary>
        /// Дата и время создания комплекта ФШ
        /// </summary>
        public const string TimeOfSetOfMasksCreation = "time_of_set_of_masks_creation";

        /// <summary>
        /// Разработчик комплекта
        /// </summary>
        public const string Developer = "developer";

        /// <summary>
        /// Автор записи
        /// </summary>
        public const string Author = "author";

        /// <summary>
        /// Название связанной таблицы, которая хранит информацию о фотошаблонах
        /// </summary>
        public const string NameOfTableWithMasks = "name_of_table";

        /// <summary>
        /// Значение поля ID записи о файле с техническими требованиями в формате word, 
        /// в таблице "tb_files_pool", содержащей реестр файлов на сервере
        /// </summary>
        public const string RecordIdofTrWordFile = "link_to_file_tt_word";

        /// <summary>
        /// Значение поля ID записи о файле с техническими требованиями в в отсканированном формате, 
        /// в таблице "tb_files_pool", содержащей реестр файлов на сервере
        /// </summary>
        public const string RecordIdofTrScanFile = "link_to_file_tt_scan";

        /// <summary>
        /// Значение поля ID записи о файле с картой раскроя, 
        /// в таблице "tb_files_pool", содержащей реестр файлов на сервере
        /// </summary>
        public const string RecordIdOfMapFile = "link_to_file_map";

        /// <summary>
        /// Значение поля ID записи о файле с архивом папки, хранящей файлы фотошаблонов, 
        /// в таблице "tb_files_pool", содержащей реестр файлов на сервере
        /// </summary>
        public const string RecordIdOfFolderWithMasks = "link_to_folder_with_masks";

        /// <summary>
        /// Значение поля ID записи о прикрепленном файле, 
        /// в таблице "tb_files_pool", содержащей реестр файлов на сервере
        /// </summary>
        public const string RecordIdOfAttachedFile = "link_to_attached_file";
    }

    /// <summary>
    /// Класс содержит сведения относительно группы таблиц, каждая из которых содержит описания фотошаблонов,
    /// названия таблиц начинаются с приставки "tb_x_masks_" + "название комплекта фотошаблонов (идентификатор записи в таблице 'TbSetOfMasks')"
    /// </summary>
    public static class TbMasks
    {
        /// <summary>
        /// Позволяет получить имя таблицы связанной с конкретным комплектом фотошаблонов
        /// </summary>
        /// <param name="nameOfSetOfMasks">Название комплекта фотошаблонов</param>
        /// <returns>Имя таблицы</returns>
        public static string GetName(string nameOfSetOfMasks)
        {
            return "tb_x_masks || " + nameOfSetOfMasks;
        }

        /// <summary>
        /// Идентификатор данного слоя, а также порядковый номер слоя
        /// </summary>
        public const string Number = "number";

        /// <summary>
        /// Метка слоя
        /// </summary>
        public const string Mark = "mark";

        /// <summary>
        /// Назначение слоя
        /// </summary>
        public const string Purpose = "purpose";

        /// <summary>
        /// Дата создания слоя
        /// </summary>
        public const string TimeOfCreation = "time";

        /// <summary>
        /// Коэффициент покрытия слоя золотом (0..1)
        /// </summary>
        public const string Coeff = "coeff";

        /// <summary>
        /// Наличие золота (YES или NO)
        /// </summary>
        public const string Aurum = "aurum";

        /// <summary>
        /// Комментарий к слою
        /// </summary>
        public const string Comment = "comment";
    }

    /// <summary>
    /// Класс содержит сведения относительно МАС-адресов сотрудников 41 лаборатории ОАО "НИИПП"
    /// </summary>
    public static class TbPeopleHardwareAddress
    {
        /// <summary>
        /// Название таблицы
        /// </summary>
        public static string Name = "tb_people_hardware_address";

        /// <summary>
        /// Фамилия и инициалы сотрудника (пример: Петров И. В.)
        /// </summary>
        public static string SurnameAndName = "name";

        /// <summary>
        /// Последовательность всех MAC-адресов конкретного сотрудника
        /// </summary>
        public static string MacAddress = "mac_address";
    }
}
