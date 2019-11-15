using GroceryStore.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GroceryStore.Utility
{
    /// <summary>
    /// utility class used for all JSON CRUD operations. Obviously, this wouldn't scale well if the 
    /// "database" is too large because everything is loaded into memory before the targeted item(s)
    /// is retrieved.
    /// </summary>
    public class EntityJsonFileManager
    {
        private string _filePath;

        JsonSerializer _serializer;


        private JObject GetData(string section)
        {
            var json = File.ReadAllText(_filePath);
            return JObject.Parse(json);
 
        }

        public EntityJsonFileManager(string filePath)
        {
            _filePath = filePath;
            _serializer =  new JsonSerializer()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public static void CreateFile(string path)
        {
            using (var stream = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes($"{{products: [],{Environment.NewLine}customers: [],{Environment.NewLine}orders: []}}");
                stream.Write(info, 0, info.Length);
            };
        }

        public void Insert(string section, IEnumerable<EntityBase> entities)
        {

            var jsonObj = GetData(section);
            var jEntities = jsonObj.GetValue(section) as JArray;
            foreach(var curItem in entities)
            {
                jEntities.Add(JObject.FromObject(curItem, _serializer));
            }
            jsonObj[section] = jEntities;
            string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(_filePath, newJsonResult);
        }

        public bool Update(string section, EntityBase entity, KeyValuePair<string, string> targetKey)
        {
            var jsonObj = GetData(section);
            var jEntities = (JArray)jsonObj[section];
            var obj = jEntities.FirstOrDefault(o => o[targetKey.Key].Value<string>() == targetKey.Value);
            if (obj == null)
            {
                return false;
            }

            var targetIndex = jEntities.IndexOf(obj);
            jEntities[targetIndex] = JObject.FromObject(entity, _serializer);
            string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(_filePath, newJsonResult);
            return true;
        }

        public IEnumerable<T> RetrieveMultiple<T>(string section, Expression<Func<T, bool>> expr = null) where T : EntityBase
        {
            var jsonObj = GetData(section);
            var jEntities = (JArray)jsonObj[section];
            var entities  = jEntities.ToObject<IEnumerable<T>>();
            if (expr != null)
            {
                return entities.Where(expr.Compile());
            }
            return entities;
        }

        public T Retrieve<T>(string section, Expression<Func<T, bool>> expr = null) where T : EntityBase
        {
            var jsonObj = GetData(section);
            var jEntities = (JArray)jsonObj[section];
            var entities = jEntities.ToObject<IEnumerable<T>>();
            return entities.FirstOrDefault(expr.Compile());
        }

        public void Insert(string section, EntityBase entity, KeyValuePair<string, string> dupeKey)
        {
            var jsonObj = GetData(section);
            var jEntities = jsonObj.GetValue(section) as JArray;
            var obj = jEntities.FirstOrDefault(o => o[dupeKey.Key].Value<string>() == dupeKey.Value);
            if (obj != null)
            {
                throw new DuplicateEntityKeyException("duplicate entity not allowed", dupeKey.Value);
            }
            jEntities.Add(JObject.FromObject(entity, _serializer));
            jsonObj[section] = jEntities;
            string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(_filePath, newJsonResult);

        }

        public int GetMaxAttrValue(string section, string attribute)
        {
            var jsonObj = GetData(section);
            var jEntities = jsonObj.GetValue(section) as JArray;
            return jEntities.Count() == 0 ? 0 : jEntities.Max(o => o[attribute].Value<int>());
        }
    }
}
