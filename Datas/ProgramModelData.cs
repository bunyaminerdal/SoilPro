using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExDesign.Datas
    {
    public class ProgramModelData
    {
        public List<string> ModelPaths { get; set; }
    }

    public static class ProgramModel
    {
        public static ProgramModelData programModel ;
        public static string tempPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "ExDesign.gexdb";
        public static void LoadModel()
        {            
            if (!File.Exists(tempPath))
            {
                ProgramModelData tempProgramModel = new ProgramModelData()
                {
                    ModelPaths = new List<string>()
                };
                string json = JsonConvert.SerializeObject(tempProgramModel);

                //write string to file
                File.WriteAllText(tempPath, json);
            }
            
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(tempPath))
            {
                JsonSerializer serializer = new JsonSerializer();
               programModel = (ProgramModelData)serializer.Deserialize(file, typeof(ProgramModelData));
            }
            ControlPathIsExist();
        }
        public static void ModelSave(string path)
        {
            programModel.ModelPaths.Add(path);
            string json = JsonConvert.SerializeObject(programModel);

            //write string to file
            File.WriteAllText(tempPath, json);
        }

        public static bool CheckPath(string path)
        {
            if (path == null) return false;
            if (!File.Exists(path)) return false;
            if(programModel.ModelPaths==null) return false;
            if(programModel.ModelPaths.Count == 0) return true;
            if (programModel.ModelPaths.Contains(path))
            {
                programModel.ModelPaths.Remove(path);
                return true;
            }
            return true;
        }
        public static void ControlPathIsExist()
        {
            for (int i = 0; i < programModel.ModelPaths.Count; i++)
            {
                if (!File.Exists(programModel.ModelPaths[i]))
                {
                    programModel.ModelPaths.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
