using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Holds a central save object to read from / write to.
/// </summary>
public class SaveManager : MonoBehaviour {
    [SerializeField] private SaveObject saveObject;

    [Header("Options")]
    [SerializeField] private bool autoSave;
    [SerializeField] private int autoSaveInterval;

    //Other fields
    private SaveFileManager fileManager;
    private bool loaded = false;

    //Properties
    public SaveObject SaveObject => saveObject;

    //Automatically loads the savefile in from disk
    private void Awake() {
        this.fileManager = new SaveFileManager();
        this.saveObject = this.fileManager.Load();
        this.loaded = true;

        if (this.autoSave && this.autoSaveInterval > 0) {
            StartCoroutine(AutoSave());
        }
    }

    [ContextMenu("Save")]
    public void Save() {
        if (!loaded) return;
        this.fileManager.Save(this.saveObject);
    }

    private IEnumerator AutoSave() {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(this.autoSaveInterval);

        while (this.autoSave) {
            Save();
            yield return wait;
        }
    }

    //Save on Quit
    private void OnApplicationQuit() {
        Save();
    }
}