using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;


namespace BeastHunter
{
    public static class SaveLoadDialog
    {
        #region Methods
        public static void SaveCharacter(CharacterSave _character, string namefile)
        {
            Save_Load.SaveAsXmlFormat(_character, Environment.CurrentDirectory + "\\" + namefile);            
        }

        public static CharacterSave LoadCharacter(string namefile)
        {
            return Save_Load.LoadFromXmlFormat(Environment.CurrentDirectory + "\\" + namefile);
        }
        #endregion
    }

    class Save_Load
    {
        #region Methods

        public static void SaveAsXmlFormat(CharacterSave obj, string fileName)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(CharacterSave));
            Debug.Log(fileName);
            Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            xmlFormat.Serialize(fStream, obj);
            fStream.Close();
        }

        public static CharacterSave LoadFromXmlFormat(string fileName)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(CharacterSave));
            Stream fStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read);
            var obj = (xmlFormat.Deserialize(fStream) as CharacterSave);
            fStream.Close();
            return obj;
        }

        #endregion
    }
}
