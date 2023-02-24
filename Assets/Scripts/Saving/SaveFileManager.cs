using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Manages File I/O for saving.
/// Used internally by the SaveManager.
/// </summary>
public class SaveFileManager {
    public const string directory = "SpellbookData";
    public const string fileName = "save.dat";
    public const string backupFileName = "save.dat.bak";

    //Properties
    private string DirectoryPath => Path.Combine(Application.persistentDataPath, directory);
    private string FullPath => Path.Combine(DirectoryPath, fileName);
    private string BackupPath => Path.Combine(DirectoryPath, backupFileName);

    /// <summary>
    /// Writes the save object to disk. The old save file becomes the new backup, and the previous backup is deleted.
    /// </summary>
    public void Save(SaveObject save) {
        BinaryFormatter formatter = new BinaryFormatter();
        if (!DirectoryExists()) {
            Directory.CreateDirectory(DirectoryPath);
        }

        //copies current save file to backup
        if (SaveExists()) {
            File.Copy(FullPath, BackupPath, true);
        }

        //opens file
        FileStream file = File.Create(FullPath);

        //saves info to file
        formatter.Serialize(file, save);

        //closes file
        file.Close();
    }

    /// <summary>
    /// Reads from the save file on disk (or the backup file)
    /// </summary>
    public SaveObject Load() {
        BinaryFormatter formatter = new BinaryFormatter();
        //First attempt to load main save
        if (SaveExists()) {
            try {
                FileStream file = File.Open(FullPath, FileMode.Open);
                SaveObject save = (SaveObject) formatter.Deserialize(file);

                file.Close();
                return save;
            }
            catch (SerializationException) {
                Debug.LogWarning("Failed to load save file.");
            }
        }
        
        //If main save fails, try backup save file
        if (BackupExists()) {
            Debug.Log("Attempting to load backup save file.");
            try {
                FileStream file = File.Open(BackupPath, FileMode.Open);
                SaveObject save = (SaveObject) formatter.Deserialize(file);

                file.Close();
                return save;
            }
            catch (SerializationException) {
                Debug.LogWarning("Failed to load backup save file.");
            }
        }

        //Save file does not exist yet -- create a new one
        return new SaveObject();
    }

    private bool SaveExists() {
        return File.Exists(FullPath);
    }

    private bool BackupExists() {
        return File.Exists(BackupPath);
    }

    private bool DirectoryExists() {
        return Directory.Exists(DirectoryPath);
    }

}
