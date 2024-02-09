using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Samples.Helpers
{
    public static class Creator
    {
        /// <summary>
        /// Gets instances of MonoBehaviours that inherit from T. Expensive operation since it uses FindObjectsOfType.
        /// </summary>
        /// <typeparam name="T">Interface Type To Check</typeparam>
        /// <returns>Instances of MonoBehaviours that inherit from T</returns>
        public static IEnumerable<T> GetMonoInstances<T>() => Object.FindObjectsOfType<MonoBehaviour>().OfType<T>();
        
        /// <summary>
        /// Creates Instances Of Every Type Which Inherits From T Interface
        /// </summary>
        /// <param name="exceptTypes">Types To Not Crease Instances Of</param>
        /// <typeparam name="T">Interface Type To Create</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> CreateInstancesOfType<T>(params Type[] exceptTypes)
        {
            var interfaceType = typeof(T);
            var result = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract &&
                            exceptTypes.All(type => !x.IsSubclassOf(type) && x != type))
                .Select(Activator.CreateInstance);

            return result.Cast<T>();
        }

        /// <summary>
        /// Logs all fields and properties of given c# object. Uses debug.log for logging.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="logTag"></param>
        public static void LogAllPropertiesAndFields(this object obj, string logTag = "")
        {
            foreach (FieldInfo fieldInfo in obj.GetType().GetFields())
            {
                string name = fieldInfo.Name;
                object value = fieldInfo.GetValue(obj);
                Debug.Log($"{logTag} - field - {obj.GetType().Name}: {name}={value}");
            }

            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                string name = propertyInfo.Name;
                object value = propertyInfo.GetValue(obj);
                Debug.Log($"{logTag} - property - {obj.GetType().Name}: {name}={value}");
            }
        }
    }
}