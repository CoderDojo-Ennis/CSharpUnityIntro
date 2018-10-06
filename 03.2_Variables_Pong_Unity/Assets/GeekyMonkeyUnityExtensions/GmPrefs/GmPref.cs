using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GeekyMonkey
{
    /// <summary>
    /// Needed for base class reflection checking
    /// </summary>
    public abstract class GmPref : ObservableTriggerBase
    {
        /// <summary>
        /// Name of the field for the current instance of this object
        /// </summary>
        protected string fieldName;

        /// <summary>
        /// The prefs container
        /// </summary>
        protected GmPrefsStore prefsStore;

        /// <summary>
        /// Thede default value
        /// </summary>
        protected object defaultValue;

        /// <summary>
        /// Have we alredy been thru the init/load once
        /// </summary>
        protected bool initialized;

        /// <summary>
        /// Prevent recursion
        /// </summary>
        protected bool notifying;

        /// <summary>
        /// Load the value from storage
        /// </summary>
        protected abstract void Load();

        /// <summary>
        /// Set Get value from string version
        /// </summary>
        /// <param name="stringVal"></param>
        /// <returns></returns>
        protected abstract object Deserialize(string stringVal);

        /// <summary>
        /// Set initial values
        /// Called by the store's constructor to connect the objects to their store, and set their field name
        /// </summary>
        /// <remarks>
        /// Avoiding passing these in the constructor so that derived classes don't need to override the constructor
        /// </remarks>
        /// <param name="fieldName">Field name for this instance</param>
        /// <param name="prefsStore">The containing store</param>
        protected void Init(string fieldName, GmPrefsStore prefsStore, string defaultValue)
        {
            this.prefsStore = prefsStore;
            this.fieldName = fieldName;
            if (defaultValue != null)
            {
                // Use the specialized deserializer
                this.defaultValue = Deserialize(defaultValue);
            }
            Load();
            initialized = true;
        }
    }

    /// <summary>
    /// A pref object
    /// </summary>
    /// <typeparam name="T">Type of value to store</typeparam>
    public partial class GmPref<T> : GmPref
    {
        /// <summary>
        /// The current value
        /// </summary>
        protected T internalValue;

        /// <summary>
        /// The current value
        /// </summary>
        public T Value
        {
            get
            {
                return internalValue;
            }
            set
            {
                // Can't use "==" on the generic type because it may be a struct
                if (!EqualityComparer<T>.Default.Equals(internalValue, value))
                {
                    internalValue = value;
                    PlayerPrefs.SetString(StorageKey, Serialize());

                    NotifySubscribers();

                    if (prefsStore.AutoSave)
                    {
                        prefsStore.Save();
                    }
                }
            }
        }

        /// <summary>
        /// Key used to identify this value in the string store
        /// </summary>
        private string StorageKey
        {
            get
            {
                return $"{prefsStore.GamerId}_{prefsStore.Prefix}_{fieldName}";
            }
        }

        /// <summary>
        /// Notify subscribers of the new value
        /// </summary>
        protected virtual void NotifySubscribers()
        {
            if (!notifying)
            {
                notifying = true;
                if (observablePrefSubject != null)
                {
                    observablePrefSubject.OnNext(this);
                }
                if (observableValueSubject != null)
                {
                    observableValueSubject.OnNext(this.Value);
                }
                notifying = false;
            }
        }

        /// <summary>
        /// Lazy created observable pref subject
        /// </summary>
        private Subject<GmPref<T>> observablePrefSubject;

        /// <summary>
        /// Lazy created observable pref subject
        /// </summary>
        private Subject<T> observableValueSubject;

        /// <summary>
        /// Observable subject for this pref
        /// </summary>
        public IObservable<GmPref<T>> AsObservablePref {
            get {
                if (observablePrefSubject == null)
                {
                    observablePrefSubject = new Subject<GmPref<T>>();
                }
                return observablePrefSubject;
            }
        }

        /// <summary>
        /// Observable subject for this value
        /// </summary>
        public IObservable<T> AsObservableValue
        {
            get
            {
                if (observableValueSubject == null)
                {
                    observableValueSubject = new Subject<T>();
                }
                return observableValueSubject;
            }
        }

        /// <summary>
        /// Prepare this value for string storage
        /// </summary>
        /// <returns></returns>
        protected virtual string Serialize()
        {
            string serialized = internalValue.ToString();
            return serialized;
        }

        /// <summary>
        /// Retrieve this value from string storage
        /// </summary>
        /// <param name="storedString">The value being retrieved from string storage</param>
        /// <returns>Parsed value</returns>
        protected override object Deserialize(string storedString) {
            T parsedValue;
            try
            {
                if (typeof(T).IsEnum)
                {
                    parsedValue = (T)Enum.Parse(typeof(T), storedString);
                }
                else
                {
                    parsedValue = (T)Convert.ChangeType(storedString, typeof(T));
                }
            }
            catch (Exception)
            {
                // Can't parse it. The variable type may have changed after a value was stored
                parsedValue = default(T);
            }
            return parsedValue;
        }

        /// <summary>
        /// Load the value frm storage
        /// </summary>
        protected override void Load()
        {
            string key = StorageKey;

            bool setValue = false;

            try
            {
                if (PlayerPrefs.HasKey(key))
                {
                    internalValue = (T)Deserialize(PlayerPrefs.GetString(key));
                    setValue = true;
                }
                else if (defaultValue != null)
                {
                    internalValue = (T)defaultValue;
                    setValue = true;
                }
            } catch (Exception ex)
            {
                Debug.Log("Error loading " + key + " : " + ex.Message);
            }

            //Debug.Log("Loaded " + key + " = " + internalValue);

            if (initialized && setValue)
            {
                NotifySubscribers();
            }
        }

        /// <summary>
        /// This object is going away - release any subscribers
        /// </summary>
        /// <remarks>
        /// Part of the ObservableTriggerBase implmentation
        /// </remarks>
        protected override void RaiseOnCompletedOnDestroy()
        {
            if (observablePrefSubject != null)
            {
                observablePrefSubject.OnCompleted();
                observablePrefSubject = null;
            }
            if (observableValueSubject != null)
            {
                observableValueSubject.OnCompleted();
                observableValueSubject = null;
            }
        }

        public int ToInt()
        {
            string valueString = internalValue.ToString();
            int outValue = 0;
            if (typeof(T).IsEnum)
            {
                outValue = Convert.ToInt32(internalValue);
            }
            else if (!Int32.TryParse(valueString, out outValue))
            {
                Debug.Log("Error converting " + fieldName + "=" + internalValue + " to int");
            }
            return outValue;
        }

        public void FromInt(int newInt)
        {
            try
            {
                Value = (T)Deserialize(newInt.ToString());
            }
            catch
            {
                Debug.Log("Error setting int " + newInt + " on " + fieldName);
                Value = default(T);
            }
        }

        /// <summary>
        /// Destructor - last chance to clean up
        /// </summary>
        ~GmPref()
        {
            RaiseOnCompletedOnDestroy();
        }

        /*
        public static implicit operator int(GmPref<T> pref)
        {
            string valueString = pref.Value.ToString();
            int outValue;
            if (typeof(T).IsEnum)
            {
                valueString = Convert.ToInt32(pref.internalValue).ToString();
            }
            if (!Int32.TryParse(valueString, out outValue))
            {
                Debug.Log("Error converting " + pref.fieldName + "=" + pref.internalValue + " to int");
            }
            return outValue;
        }
        */

        /// <summary>
        /// Implicitly retrieve the value without requiring using the Value property
        /// </summary>
        /// <param name="pref">This pref</param>
        public static implicit operator T(GmPref<T> pref)
        {
            return pref.Value;
        }

    }
}
