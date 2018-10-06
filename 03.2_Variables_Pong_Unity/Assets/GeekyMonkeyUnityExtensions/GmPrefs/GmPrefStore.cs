using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace GeekyMonkey
{
    /// <summary>
    /// Prefs storage base class
    /// </summary>
    public abstract class GmPrefsStore
    {
        /// <summary>
        /// Construct the store
        /// </summary>
        public GmPrefsStore()
        {
            Initialized = false;
            Prefix = GetType().Name;
            CreatePrefs();
            InitPrefs();
            Initialized = true;
        }

        /// <summary>
        /// Has the constructor finished
        /// </summary>
        private bool Initialized = false;

        /// <summary>
        /// Gamer Identifier used as part of the prefix
        /// </summary>
        private string gamerId = string.Empty;

        /// <summary>
        /// Gamer Identifier used as part of the prefix
        /// </summary>
        /// <remarks>Default is empty for non-player prefs stores</remarks>
        public string GamerId
        {
            get
            {
                return gamerId;
            }
            set
            {
                gamerId = value;
                if (Initialized)
                {
                    // Re-load the prefs with the new prefix
                    LoadPrefs();
                }
            }
        }

        /// <summary>
        /// Prefix for the keys in this store
        /// </summary>
        private string prefix;

        /// <summary>
        /// Prefix for the keys in this store
        /// </summary>
        /// <remarks>Default is the store class name</remarks>
        public string Prefix
        {
            get
            {
                return prefix;
            }
            set
            {
                prefix = value;
                if (Initialized)
                {
                    // Re-load the prefs with the new prefix
                    LoadPrefs();
                }
            }
        }

        /// <summary>
        /// Should the store auto-save when a value changes
        /// </summary>
        private bool autosave = true;

        /// <summary>
        /// Should the store auto-save when a value changes
        /// </summary>
        /// <remarks>Default = true</remarks>
        public bool AutoSave
        {
            get
            {
                return autosave;
            }
            set
            {
                bool oldautosave = value;
                autosave = value;
                if (oldautosave == false && autosave == true)
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Create all of the the pref field objects, passing needed info to their constructors
        /// </summary>
        private void CreatePrefs()
        {
            var fields = GetType().GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                if (fieldInfo.FieldType.IsSubclassOf(typeof(GmPref)))
                {
                    // Make sure the field is marked as readonly to prevent misuse
                    if (!fieldInfo.IsInitOnly)
                    {
                        throw new Exception($"Field not readonly: '{fieldInfo.Name}'. All fields in {GetType().Name} must be marked as readonly.");
                    }

                    // Find the constructor method for this pref type
                    var constructorMethod = fieldInfo.FieldType.GetConstructors()[0]; // (BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                    if (constructorMethod != null)
                    {
                        // Construct a new pref object
                        var newPref = constructorMethod.Invoke(new object[] { });

                        // Save this constructed pref object in the pref store variable
                        fieldInfo.SetValue(this, newPref);
                    }
                    else
                    {
                        // Don't allow non pref field types since this could lead to confusion
                        throw new Exception($"Constructor not found for: '{GetType().Name}.{fieldInfo.Name}'");
                    }
                }
                else if (fieldInfo.FieldType == GetType() || fieldInfo.FieldType.IsSubclassOf(GetType()))
                {
                    // Ignore singleton instance fields
                }
                else
                {
                    // Don't allow non pref field types since this could lead to confusion
                    throw new Exception($"Invalid field: '{fieldInfo.Name}'. {GetType().Name} can only have fields of types derived from {nameof(GmPref)}.");
                }
            }
        }

        /// <summary>
        /// Wire up the pref properties with their store container and field names
        /// </summary>
        private void InitPrefs()
        {
            var fields = GetType().GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                // Get the constructed readonly field value
                var fieldValue = fieldInfo.GetValue(this);

                // Get the default value from an optional attribute
                string defaultValue = null;
                var defaultValueInfo = fieldInfo.GetCustomAttribute<DefaultValueAttribute>(true);
                if (defaultValueInfo != null)
                {
                    defaultValue = defaultValueInfo.Value.ToString();
                }

                // Initialize the pref and tie it to the collection
                var initMethod = fieldInfo.FieldType.GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(string), typeof(GmPrefsStore), typeof(string) }, null);
                if (initMethod != null)
                {
                    initMethod.Invoke(fieldValue, new object[] { fieldInfo.Name, this, defaultValue });
                }
            }
        }

        /// <summary>
        /// Re-Load the values for the prefs
        /// </summary>
        private void LoadPrefs()
        {
            var fields = GetType().GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                // Get the constructed readonly field value
                var fieldValue = fieldInfo.GetValue(this);

                // Call the load function
                var loadMethod = fieldInfo.FieldType.GetMethod("Load", BindingFlags.NonPublic | BindingFlags.Instance);
                if (loadMethod != null)
                {
                    loadMethod.Invoke(fieldValue, new object[] { });
                }
            }
        }

        /// <summary>
        /// Trigger saving all changed values to permanent storage
        /// </summary>
        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}
