using SharpGIS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using Windows.Storage;

namespace BeAtzmi.Classes.ResumeBuilderItems
{
    class ResumeUser
    {
        //Fields Definition
        private string _firstName, _lastName, _email, _phoneNumber, _address;
        private int _computer;
        private DateTime _birthday;
        private ObservableCollection<IResumeBuilderItem> _items;

        //Property Definition
        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                _firstName = value;
            }
        }
        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
            }
        }
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
            }
        }
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }

            set
            {
                _phoneNumber = value;
            }
        }
        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
            }
        }
        public DateTime Birthday
        {
            get
            {
                return _birthday;
            }

            set
            {
                _birthday = value;
            }
        }
        public int ComputerKnowledge
        {
            get
            {
                return _computer;
            }

            set
            {
                _computer = value;
            }
        }
        public ObservableCollection<IResumeBuilderItem> Items
        {
            get
            {
                return _items;
            }
        }

        //Constructor
        public ResumeUser(string fname, string lname, string emailAddress, string phoneNumber, string address, DateTime birthDate, int computer)
        {
            _firstName = fname;
            _lastName = lname;
            _email = emailAddress;
            _phoneNumber = phoneNumber;
            _address = address;
            _birthday = birthDate;
            _computer = computer;
            _items = new ObservableCollection<IResumeBuilderItem>();
        }

        public void AddItem(IResumeBuilderItem item)
        {
            _items.Add(item);
            _items = _items.OrderBy(m => m.Kind).ToObservableCollection<IResumeBuilderItem>();
        }

        public void WriteUserResumeToFile(string nameInIsolatedStorage)
        {
            if (!nameInIsolatedStorage.EndsWith(".docx")) nameInIsolatedStorage += ".docx";

            UnzipAndSaveFiles(getStream("Resume.zip"));

            TextReader input = new StreamReader(getStream("document.xml"), Encoding.UTF8);
            string doc = input.ReadToEnd();

            doc = doc.Replace("{CITY}", ReverseString(_address));
            doc = doc.Replace("{PHONE}", _phoneNumber);
            doc = doc.Replace("{BDATE}", _birthday.ToString("dd/MM/yy"));
            doc = doc.Replace("{EMAIL}", _email);
            doc = doc.Replace("{NAME}", ReverseString(_firstName + " " + _lastName));
            doc = doc.Replace("{COMPUTER}", getComputerString(_computer));

            input = new StreamReader(getStream("Lang.txt"), Encoding.UTF8);
            string file = input.ReadToEnd();
            string output = String.Empty;

            //Add languages
            foreach (ResumeLanguage item in _items.Where(x => x.Kind == ResumeBuilderItemKind.Languages))
            {
                string name = item.Value;
                string lvl = getLanguageLevelString(item.Level);
                output += file.Replace("{LEVEL}", lvl).Replace("{LANGNAME}", name);
            }

            doc = doc.Replace("{LANGS}", output);

            //Add Work Experience
            input = new StreamReader(getStream("Work.txt"), Encoding.UTF8);
            file = input.ReadToEnd();
            output = String.Empty;

            foreach (ResumeWorkExperience item in _items.Where(x => x.Kind == ResumeBuilderItemKind.WorkExperience))
            {
                string startYear = item.StartYear.ToString();
                string endYear = (item.EndYear == 0) ? "היום" : item.EndYear.ToString();
                string job = item.Role;
                string place = item.Value;
                string details = item.JobDetail;

                output += file.Replace("{STARTYEAR}", startYear).Replace("{ENDYEAR}", endYear).Replace("{JOBNAME}", job).Replace("{WORKPLACE}", place).Replace("{DETAILS}", details);
            }


            doc = doc.Replace("{WORKEX}", output);

            //Add militerry
            input = new StreamReader(getStream("Mil.txt"), Encoding.UTF8);
            file = input.ReadToEnd();
            output = String.Empty;

            foreach (ResumeMilitaryService item in _items.Where(x => x.Kind == ResumeBuilderItemKind.MilitaryService))
            {
                string startYear = item.StartYear.ToString();
                string endYear = (item.EndYear == 0) ? "היום" : item.EndYear.ToString();
                string job = item.Value;
                string place = item.Place;
                string details = item.JobDetail;

                output += file.Replace("{STARTYEAR}", startYear).Replace("{ENDYEAR}", endYear).Replace("{JOB}", job).Replace("{PLACE}", place).Replace("{DETAILS}", details);
            }

            doc = doc.Replace("{MIL}", output);

            //Add Education
            input = new StreamReader(getStream("Education.txt"), Encoding.UTF8);
            file = input.ReadToEnd();
            output = String.Empty;

            foreach (ResumeEducation item in _items.Where(x => x.Kind == ResumeBuilderItemKind.Education))
            {
                string startYear = item.StartYear.ToString();
                string endYear = (item.EndYear == 0) ? "היום" : item.EndYear.ToString();
                string cert = item.Certificate;
                string place = item.Value;
                string ext = item.Specialization;
                string details = item.Detail;

                output += file.Replace("{STARTYEAR}", startYear).Replace("{ENDYEAR}", endYear).Replace("{CERT}", cert).Replace("{PLACE}", place).Replace("{DETAILS}", details).Replace("{EXTEND}", ext);


                doc = doc.Replace("{EDUCATION}", output);
            }

            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();


            using (MemoryStream zipMemoryStream = new MemoryStream())
            {
                // Create zip archive
                using (ZipArchive zipArchive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create))
                {
                    List<string> allFiles = GetAllFiles("Resume\\", isoStore);
                    List<string> list = new List<string>();

                    foreach (var item in allFiles)
                    {
                        if (item.IndexOf("Resume/") == 0) list.Add(item);
                    }

                    foreach (string fileName in list)
                    {
                        ZipArchiveEntry entry = zipArchive.CreateEntry(fileName.Substring(7), CompressionLevel.Optimal);
                        using (Stream entryStream = entry.Open())
                        {
                            isoStore.OpenFile(fileName, FileMode.Open).CopyTo(entryStream);
                            entryStream.Flush();
                            entryStream.Close();
                        }
                    }

                    ZipArchiveEntry docEntry = zipArchive.CreateEntry("word/document.xml", CompressionLevel.Optimal);
                    using (StreamWriter writer = new StreamWriter(docEntry.Open()))
                    {
                        writer.Write(doc);
                        doc = null;
                    }

                }

                if (isoStore.FileExists(nameInIsolatedStorage)) isoStore.DeleteFile(nameInIsolatedStorage);

                // Created new file to store compressed files
                using (IsolatedStorageFileStream zipFile = isoStore.CreateFile(nameInIsolatedStorage))
                {
                    byte[] buffer = zipMemoryStream.ToArray();
                    zipFile.Write(buffer, 0, buffer.Length);
                    zipFile.Flush();
                }
            }

        }

        // Method to retrieve all directories, recursively, within a store. 
        public static List<String> GetAllDirectories(string pattern, IsolatedStorageFile storeFile)
        {
            // Get the root of the search string. 
            string root = Path.GetDirectoryName(pattern);

            if (root != "")
            {
                root += "/";
            }

            // Retrieve directories.
            List<String> directoryList = new List<String>(storeFile.GetDirectoryNames(pattern));

            // Retrieve subdirectories of matches. 
            for (int i = 0, max = directoryList.Count; i < max; i++)
            {
                string directory = directoryList[i] + "/";
                List<String> more = GetAllDirectories(root + directory + "*", storeFile);

                // For each subdirectory found, add in the base path. 
                for (int j = 0; j < more.Count; j++)
                {
                    more[j] = directory + more[j];
                }

                // Insert the subdirectories into the list and 
                // update the counter and upper bound.
                directoryList.InsertRange(i + 1, more);
                i += more.Count;
                max += more.Count;
            }

            return directoryList;
        }

        public static List<String> GetAllFiles(string pattern, IsolatedStorageFile storeFile)
        {
            // Get the root and file portions of the search string. 
            string fileString = Path.GetFileName(pattern);

            List<String> fileList = new List<String>(storeFile.GetFileNames(pattern));

            // Loop through the subdirectories, collect matches, 
            // and make separators consistent. 
            foreach (string directory in GetAllDirectories("*", storeFile))
            {
                foreach (string file in storeFile.GetFileNames(directory + "/" + fileString))
                {
                    fileList.Add((directory + "/" + file));
                }
            }

            return fileList;
        } // End of GetFiles.

        private string getLanguageLevelString(LanguageKnowledgeLevel languageKnowledgeLevel)
        {
            switch (languageKnowledgeLevel)
            {
                case LanguageKnowledgeLevel.BasicLevel:
                    return "רמה בסיסית";
                case LanguageKnowledgeLevel.BirthLanguage:
                    return "שפת אם";
                case LanguageKnowledgeLevel.BirthLanguageLevel:
                    return "ברמת שפת אם";
                case LanguageKnowledgeLevel.GoodLevel:
                    return "רמה טובה";
                case LanguageKnowledgeLevel.VeryGoodLevel:
                    return "רמה טובה מאוד";
            }

            return null;
        }

        private string getComputerString(int _computer)
        {
            switch(_computer)
            {
                case 0:
                    return "אין";
                case 1:
                    return "יש ידע בסיסי בלבד";
                case 2:
                    return "מכיר ועובד עם תוכנות Office";
                case 3: 
                    return "שולט היטב במגוון תוכנות מקצועיות";
            }

            return null;
        }

        private static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private Stream getStream(string p)
        {
            return Application.GetResourceStream(new Uri("Assets/ResumeBaseFiles/" + p, UriKind.Relative)).Stream;
        }

        private static void DeleteFolder(IsolatedStorageFile isoStore, string path)
        {
            if (!isoStore.DirectoryExists(path))
                return;
            // Get the subfolders that reside under path
            var folders = isoStore.GetDirectoryNames(Path.Combine(path, "*.*"));

            // Iterate through the subfolders and check for further subfolders            
            foreach (var folder in folders)
            {
                string folderName = path + "/" + folder;
                DeleteFolder(isoStore, folderName);
            }
            // Delete all files at the root level in that folder.
            foreach (var file in isoStore.GetFileNames(Path.Combine(path, "*.*")))
            {
                isoStore.DeleteFile(Path.Combine(path, file));
            }
            // Finally delete the path
            isoStore.DeleteDirectory(path);
        }

        private static void UnzipAndSaveFiles(Stream stream)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (isoStore.DirectoryExists("Resume"))
                {
                    DeleteFolder(isoStore, "Resume");
                }

                using (var zipStream = new UnZipper(stream))
                {
                    foreach (string file in zipStream.FileNamesInZip)
                    {
                        isoStore.CreateDirectory("Resume");
                        string fileName = Path.Combine(file.Split('/'));
                        if (!isoStore.DirectoryExists(Path.GetDirectoryName(fileName))) isoStore.CreateDirectory(Path.GetDirectoryName(fileName));

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            //save file entry to storage
                            using (var streamWriter =
                                new BinaryWriter(new IsolatedStorageFileStream(fileName,
                                                                               FileMode.Create,
                                                                               FileAccess.Write, FileShare.Write,
                                                                               isoStore)))
                            {
                                Stream fileStream = zipStream.GetFileStream(file);

                                var buffer = new byte[2048];
                                int size;
                                while ((size = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    streamWriter.Write(buffer, 0, size);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public static class CollectionExtensions
    {

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);
            return c;
        }
    }
}
