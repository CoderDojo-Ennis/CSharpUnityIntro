using TMPro;
using UniRx;
using UnityEngine.UI;

namespace GeekyMonkey
{
    public partial class GmPref<T>
    {
        /// <summary>
        /// Bind this pref to a dropdown input
        /// </summary>
        /// <param name="dropdown">Dropdown to bind to</param>
        public void Bind(TMP_Dropdown dropdown)
        {
            // Initial dropdown value from pref value
            //Debug.Log("Dropdown " + dropdown.name + " initializing with: " + Value + "=" + ToInt());
            dropdown.value = ToInt();

            // Pref changed
            AsObservablePref.Subscribe((newPref) => {
                //Debug.Log("Dropdown " + dropdown.name + " getting new value: " + newPref.Value + "=" + newPref.ToInt());
                int intValue = newPref.ToInt();
                if (dropdown.value != intValue)
                {
                    dropdown.value = intValue;
                }
            });

            // Dropdown selection changed
            dropdown.onValueChanged.AddListener((int selectedValue) => {
                //Debug.Log("Dropdown " + dropdown.name + " selection changed: " + selectedValue);
                FromInt(selectedValue);
            });
        }

        /// <summary>
        /// Bind this pref to a dropdown input
        /// </summary>
        /// <param name="dropdown">Dropdown to bind to</param>
        public void Bind(Slider slider)
        {
            // Set initial alue
            slider.value = float.Parse(Value.ToString());

            AsObservableValue.Subscribe((T prefValue) => {
                float floatValue = float.Parse(prefValue.ToString());

                if (slider.value != floatValue)
                {
                    slider.value = floatValue;
                }
            });

            slider.onValueChanged.AddListener((float newSliderValue) => {
                Value = (T)Deserialize(newSliderValue.ToString());
            });
        }

        /// <summary>
        /// One Way Bind this pref to a text for display
        /// </summary>
        /// <param name="dropdown">Dropdown to bind to</param>
        public void Bind(TMP_Text text)
        {
            // Set initial alue
            text.text = Serialize();

            AsObservablePref.Subscribe((prefValue) => {
                text.text = prefValue.Serialize();
            });
        }
    }
}
