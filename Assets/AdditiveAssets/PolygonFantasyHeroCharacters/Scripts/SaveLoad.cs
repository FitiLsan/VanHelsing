using PsychoticLab;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.AdditiveAssets.PolygonFantasyHeroCharacters.Scripts
{
    public static class SaveLoadDialog
    {
        public static void SaveCharacter(SaveCharacter _character, string namefile)
        {
            Save_Load.SaveAsXmlFormat(_character, Environment.CurrentDirectory + "\\" + namefile);            
        }

        public static SaveCharacter LoadCharacter(string namefile)
        {
            return Save_Load.LoadFromXmlFormat(Environment.CurrentDirectory + "\\" + namefile);
        }
    }

    class Save_Load
    {
        public static void SaveAsXmlFormat(SaveCharacter obj, string fileName)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(SaveCharacter));
            Debug.Log(fileName);
            Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            xmlFormat.Serialize(fStream, obj);
            fStream.Close();
        }

        public static SaveCharacter LoadFromXmlFormat(string fileName)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(SaveCharacter));
            Stream fStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read);
            var obj = (xmlFormat.Deserialize(fStream) as SaveCharacter);
            fStream.Close();
            return obj;
        }
    }
}
