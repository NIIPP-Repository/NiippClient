#define CLIENT
#undef SERVER

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

#pragma warning disable 618

// Пространство имен в котором реализованы классы для отправки файлов на сервер и получения файлов с сервера
// Эти классы можно также использовать на стороне сервера
namespace NIIPP.DatabaseClient.NetworkFileManager
{
    /// <summary>
    /// Класс позволяет принимать файлы по сети
    /// </summary>
    class NetworkReciever
    {
        private TcpListener _tcpListener;

        // данные для хранения логов
        public string PathToLogFile { get; set; }
        private List<string> _logMas = new List<string>();

        // насильно устанавливаемый IP (если установлен не null, то выбирается не автоматически)
        public string ForceMyIp { get; set; }

        // папка куда будут сохраняться полученные по сети файлы (по умолчанию - корень)
        public string PathToSaveFolder { get; set; }

        // если true то после получения файла от клиента запуститься новое прослушивание
        public bool NeedToLongTimeRecieve { get; set; }

        // порт по которому будет вестись прослушивание
        public string Port { get; set; }

        // количество байт в одном сетевом пакете
        public int CountOfBytesInBuffer { get; set; }

        // максимальное количество ожидающих клиентов
        public int CountOfClient { get; set; }

        // нужно ли писать лог в консоль
        public bool NeedToWriteToConsole { get; set; }

        // нужно ли писать лог в файл
        public bool NeedToWriteLog { get; set; }

        public string FileStorageData { get; set; }

        /// <summary>
        /// Инициализирует объект для приема сетевых файлов
        /// </summary>
        public NetworkReciever()
        {
            FileStorageData = "";
            CountOfBytesInBuffer = 1024;
            Port = "3333";
            NeedToLongTimeRecieve = true;
            PathToSaveFolder = "";
            ForceMyIp = null;
            CountOfClient = 100;
            NeedToWriteLog = true;
        }

        /// <summary>
        /// Возвращает IP адрес данного компьютера
        /// </summary>
        /// <returns></returns>
        private string GetCurrentMachineIP()
        {
            String host = Dns.GetHostName();
            IPAddress ip = Dns.GetHostByName(host).AddressList[0];
            return ip.ToString();
        }

        /// <summary>
        /// Выбирает собственный IP если не задан насильно, то вычисляет IP самостоятельно
        /// </summary>
        /// <returns></returns>
        private string SelectMyIp()
        {
            if (ForceMyIp != null)
                return ForceMyIp;
            else
                return GetCurrentMachineIP();
        }

        /// <summary>
        /// Записывает события в лог
        /// </summary>
        /// <param name="statusMessage">Строка с сообщением</param>
        private void UpdateStatus(string statusMessage)
        {
            if (NeedToWriteToConsole)
                Console.WriteLine(statusMessage);
            _logMas.Add(statusMessage);
        }

        /// <summary>
        /// Запускает прослушивание
        /// </summary>
        public void StartReceiving()
        {
            NetworkStream networkStream = null;
            TcpClient tcpClient = null;
            bool isError = false;
            string myIp = SelectMyIp();

            try
            {
                // если сервер для прослушивания не инициализирован, то инициализируем
                if (_tcpListener == null)
                    _tcpListener = new TcpListener(IPAddress.Parse(myIp), Int32.Parse(Port));

                UpdateStatus("Starting the server...");
                _tcpListener.Start(CountOfClient);

                UpdateStatus("The server has started");
                UpdateStatus("Please connect the client to " + myIp);

                tcpClient = _tcpListener.AcceptTcpClient();
                UpdateStatus("The server has accepted the client");

                networkStream = tcpClient.GetStream();
                UpdateStatus("The server has received the stream");

                // подождем пока придут пакеты (даем фору по времени клиенту)
                Thread.Sleep(300);

                string command = GetCommandFromNetPackage(networkStream);
                string[] metaInfo = GetMetaInfoFromNetPackage(networkStream);

                switch (command)
                {
                    case "command_load_file":
                        LoadFileFromNet(networkStream, metaInfo[0], metaInfo[1]);
                        break;
                    case "command_file_request":
                        SendFileToClient(metaInfo[0], metaInfo[1]);
                        break;
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Erorr!!!");
                UpdateStatus(ex.Message);
                isError = true;
            }
            finally
            {
                if (!isError)
                    UpdateStatus("The file was received");
                else
                    UpdateStatus("The file was NOT received");

                // безопасно закрываем соединение
                CloseCurrentConnection(networkStream, tcpClient);
                UpdateStatus("Streams are now closed \n");
                // записываем лог если есть путь и разрешение для записи и чистим промежуточный лист
                if (PathToLogFile != null && NeedToWriteLog)
                    SaveLog();

                // заново запускаем прослушивание если необходимо
                if (NeedToLongTimeRecieve)
                    StartReceiving();
            }
        }

        /// <summary>
        /// Останавливает запущенное прослушивание
        /// </summary>
        public void StopReceiving()
        {
            _tcpListener.Stop();
            _tcpListener = null;
        }

        private void SaveLog()
        {
            File.WriteAllLines(PathToLogFile, _logMas);
            _logMas.Clear();
        }

        private static void CloseCurrentConnection(NetworkStream networkStream, TcpClient tcpClient)
        {
            if (networkStream != null)
            {
                networkStream.Flush();
                networkStream.Close();
            }
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }

        private void SendFileToClient(string fileName, string clientIp)
        {
            NetworkSender ns = new NetworkSender(clientIp);
            ns.SendFile(FileStorageData + "/" + fileName, null);
        }

        private void LoadFileFromNet(NetworkStream networkStream, string fileName, string strSize)
        {
            FileStream fileStream = null;
            try
            {
                // получаем размер файла
                long fileSize = Convert.ToInt64(strSize);
                UpdateStatus("Receiving file '" + fileName + "' (" + fileSize + " bytes)");
                // готовим буфер для чтения
                int bytesSize;
                byte[] downBuffer = new byte[CountOfBytesInBuffer];
                // зная имя файла создаем его и готовим для записи содержимого
                fileStream = new FileStream(PathToSaveFolder + "/" + fileName, FileMode.Create);
                // считываем содержимое файла по пакетам и записываем его в локальный файл
                while ((bytesSize = networkStream.Read(downBuffer, 0, CountOfBytesInBuffer)) > 0)
                    fileStream.Write(downBuffer, 0, bytesSize);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
        }

        private string[] GetMetaInfoFromNetPackage(NetworkStream networkStream)
        {
            byte[] downBuffer = new byte[CountOfBytesInBuffer * 2];
            networkStream.Read(downBuffer, 0, CountOfBytesInBuffer * 2);
            string str = Encoding.UTF8.GetString(downBuffer, 0, CountOfBytesInBuffer * 2);
            str = str.Substring(0, str.IndexOf('\n'));

            return str.Split(new char[] { '\t' });
        }

        private string GetCommandFromNetPackage(NetworkStream networkStream)
        {
            byte[] downBuffer = new byte[CountOfBytesInBuffer * 2];
            networkStream.Read(downBuffer, 0, CountOfBytesInBuffer * 2);
            string str = Encoding.UTF8.GetString(downBuffer, 0, CountOfBytesInBuffer * 2);
            str = str.Substring(0, str.IndexOf('\n'));

            return str;
        }
    }

    /// <summary>
    /// Класс позволяет отправлять файлы по сети
    /// </summary>
    class NetworkSender
    {
        /// <summary>
        /// Текущее подключение по TCP протоколу
        /// </summary>
        private TcpClient _tcpClient = null;

        // данные для хранения логов
        List<string> _logMas = new List<string>();
        public string PathToLogFile { get; set; }

        public bool NeedToWriteLog { get; set; }

        // количество байт в одном сетевом пакете
        public int CountOfBytesInBuffer { get; set; }

        // порт клиента, по которому он ведет прослушивание
        public int ClientPort { get; set; }

        // нужно ли писать лог в консоль
        public bool NeedToWriteToConsole { get; set; }

        // IP адрес клиента
        private readonly string _clientIp = null;

        /// <summary>
        /// Инициализирует объект для отправки файлов по сети
        /// </summary>
        public NetworkSender(string clientIp)
        {
            PathToLogFile = null;
            NeedToWriteLog = true;
            NeedToWriteToConsole = false;
            CountOfBytesInBuffer = 1024;
            ClientPort = 3333;
            _clientIp = clientIp;
        }

        /// <summary>
        /// Устанавливает соединение с сервером
        /// </summary>
        /// <param name="clientIP">IP адрес клиента, которому посылаем файл</param>
        private bool ConnectToServer()
        {
            _tcpClient = new TcpClient();
            try
            {
                _tcpClient.Connect(_clientIp, ClientPort);
                UpdateStatus("Successfully connected to server");
                return true;
            }
            catch (Exception exMessage)
            {
                UpdateStatus(exMessage.Message);
                return false;
            }
        }

        /// <summary>
        /// Записывает события в лог
        /// </summary>
        /// <param name="statusMessage">Строка с сообщением</param>
        private void UpdateStatus(string statusMessage)
        {
            if (NeedToWriteToConsole)
                Console.WriteLine(statusMessage);
            _logMas.Add(statusMessage);
        }

        /// <summary>
        /// Возвращает IP адрес данного компьютера
        /// </summary>
        /// <returns></returns>
        private string GetCurrentMachineIP()
        {
            String host = Dns.GetHostName();
            IPAddress ip = Dns.GetHostByName(host).AddressList[0];
            return ip.ToString();
        }

        public void SendRequestToGiveFile(string nameOfFile)
        {
            // если не удалось подключиться - выходим
            if (!ConnectToServer())
                return;

            bool isError = false;
            NetworkStream networkStream = null;
            try
            {
                // получаем сетевой поток для записи в него отправляемого файла
                networkStream = _tcpClient.GetStream();
                // передаем по сети команду c запросом выслать нам файл (1 пакет)
                SendStringInNetPackage(networkStream, "command_file_request");
                // передаем по сети название файла и собственное IP 
                SendStringInNetPackage(networkStream, nameOfFile + "\t" + GetCurrentMachineIP());
            }
            catch (Exception ex)
            {
                UpdateStatus("Erorr!!!");
                UpdateStatus(ex.Message);
                isError = true;
            }
            finally
            {
                if (!isError)
                    UpdateStatus("Request sent");
                else
                    UpdateStatus("Request does NOT sent");

                // безопасно завершаем соединение
                CloseConnection(networkStream);
                UpdateStatus("Streams and connections are now closed \n");
                // записываем лог если это нужно
                if (PathToLogFile != null && NeedToWriteLog)
                    SaveLogFile();
            }
        }

        private void SaveLogFile()
        {
            File.WriteAllLines(PathToLogFile, _logMas);
            _logMas.Clear();
        }

        /// <summary>
        /// Отправляет файл по сети выбранному клиенту
        /// </summary>
        /// <param name="pathToFile">Путь к файлу который необходимо отправить</param>
        /// <param name="newName">Новое имя файла для сохранения на сервере (если параметр null, то берется настоящее имя файла)</param>
        public void SendFile(string pathToFile, string newName)
        {
            // если не удалось подключиться - выходим
            if (!ConnectToServer())
                return;

            bool isError = false;
            NetworkStream networkStream = null;
            FileStream fileStream = null;
            try
            {
                // получаем сетевой поток для записи в него отправляемого файла
                UpdateStatus("Sending file information");
                networkStream = _tcpClient.GetStream();

                // объект для получения информации о передаваемом файле
                FileInfo fInfo = new FileInfo(pathToFile);
                // открываем отправляемый файл
                fileStream = new FileStream(pathToFile, FileMode.Open, FileAccess.Read);

                // передаем команду серверу
                SendStringInNetPackage(networkStream, "command_load_file");
                // передаем мета-данные файла (имя и размер), берем либо настоящее либо новое имя
                SendStringInNetPackage(networkStream, (newName ?? fInfo.Name) + "\t" + fInfo.Length.ToString());

                // готовим буферный массив
                int bytesSize = 0;
                byte[] downBuffer = new byte[CountOfBytesInBuffer];
                // передаем содержание файла по сети в виде последовательности сетевых пакетов
                UpdateStatus("Sending the file '" + fInfo.Name + "'");
                while ((bytesSize = fileStream.Read(downBuffer, 0, CountOfBytesInBuffer)) > 0)
                    networkStream.Write(downBuffer, 0, bytesSize);
            }
            catch (Exception ex)
            {
                UpdateStatus("Erorr!!!");
                UpdateStatus(ex.Message);
                isError = true;
            }
            finally
            {
                if (!isError)
                    UpdateStatus("File sent");
                else
                    UpdateStatus("File does NOT sent");

                // безопасно завершаем соединение
                CloseConnection(networkStream);
                // закрываем файл
                CloseFile(fileStream);
                UpdateStatus("Streams and connections are now closed \n");
                // записываем лог если это нужно
                if (PathToLogFile != null && NeedToWriteLog)
                    SaveLogFile();
            }
        }

        private static void CloseFile(FileStream fileStream)
        {
            if (fileStream != null)
            {
                fileStream.Flush();
                fileStream.Close();
            }
        }

        private void CloseConnection(NetworkStream networkStream)
        {
            if (_tcpClient != null)
            {
                _tcpClient.Close();
            }
            if (networkStream != null)
            {
                networkStream.Flush();
                networkStream.Close();
            }
        }

        private void SendStringInNetPackage(NetworkStream networkStream, string str)
        {
            byte[] byteFileName = Encoding.UTF8.GetBytes((str + "\n").ToCharArray());
            byte[] toWriteName = new byte[CountOfBytesInBuffer * 2];
            byteFileName.CopyTo(toWriteName, 0);
            networkStream.Write(toWriteName, 0, CountOfBytesInBuffer * 2);
        }
    }

    /// <summary>
    /// Оболочка для классов NetworkSender и NetworkReciever
    /// </summary>
    public static class Network
    {
        private static string
            _serverIp = "172.16.1.24",
            _pathToSaveFolder = null,
            _pathToLogFileRec = null,
            _pathToLogFileSender = null;
        public static string ServerIp
        {
            get { return _serverIp; }
            set { _serverIp = value; }
        }

        public static string PathToSaveFolder
        {
            get { return _pathToSaveFolder; }
            set { _pathToSaveFolder = value; }
        }

        public static string PathToLogFileRec
        {
            get { return _pathToLogFileRec; }
            set { _pathToLogFileRec = value; }
        }

        public static string PathToLogFileSender
        {
            get { return _pathToLogFileSender; }
            set { _pathToLogFileSender = value; }
        }

        private static void StartContListen()
        {
            NetworkReciever nr = new NetworkReciever();
            nr.NeedToLongTimeRecieve = true;
            if (_pathToSaveFolder != null)
                nr.PathToSaveFolder = _pathToSaveFolder;
            if (_pathToLogFileRec != null)
                nr.PathToLogFile = _pathToLogFileRec;

            nr.StartReceiving();
        }

        private static void StartSingleListen()
        {
            NetworkReciever nr = new NetworkReciever {NeedToLongTimeRecieve = false};
            if (_pathToSaveFolder != null)
                nr.PathToSaveFolder = _pathToSaveFolder;
            if (_pathToLogFileRec != null)
                nr.PathToLogFile = _pathToLogFileRec;
            nr.StartReceiving();
            nr.StopReceiving();
        }

        public static Thread StartContReceiving()
        {
            Thread threadForListen = new Thread(StartContListen);
            threadForListen.Start();

            return threadForListen;
        }

        public static Thread StartSingleReceiving()
        {
            Thread threadForListen = new Thread(StartSingleListen);
            threadForListen.Start();

            return threadForListen;
        }

        public static void DownloadFile(string nameOfFile)
        {
            // включаем фоновое прослушивание чтобы скачать файл
            Thread threadForListen = StartSingleReceiving();

            // отправляем запрос серверу с просьбой выслать нам файл
            NetworkSender ns = new NetworkSender(_serverIp);
            if (_pathToLogFileSender != null)
                ns.PathToLogFile = _pathToLogFileSender;
            ns.SendRequestToGiveFile(nameOfFile);

            // ждем пока файл не скачается
            threadForListen.Join();
        }

        public static void UploadFile(string pathToFile, string newName)
        {
            NetworkSender ns = new NetworkSender(_serverIp);
            if (_pathToLogFileSender != null)
                ns.PathToLogFile = _pathToLogFileSender;
            ns.SendFile(pathToFile, newName);
        }
    }
}
