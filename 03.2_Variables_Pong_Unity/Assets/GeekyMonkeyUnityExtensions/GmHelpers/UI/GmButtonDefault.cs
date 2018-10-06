using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GeekyMonkey
{
    /// <summary>
    /// Attach this script to a button control and optionally specify a form container
    /// otherwise the parent element of the button is considered to be the form.
    /// An enter key press when any input on the form will trigger a button click.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class GmButtonDefault : MonoBehaviour
    {
        private Button button;

        public Transform FormContainer;

        // Start
        private void Start()
        {
            // Remember the button we're attached to
            button = GetComponent<Button>();

            // If no form container specified, assume the immediate parent of the button
            if (FormContainer== null)
            {
                FormContainer = transform.parent;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                var selectedInput = EventSystem.current.currentSelectedGameObject;
                if (selectedInput != null && selectedInput != gameObject)
                {
                    if (selectedInput.IsDescendentOf(FormContainer))
                    {
                        FakeClickTheButton();
                    }
                }
            }
        }

        /// <summary>
        /// Simulate button click
        /// </summary>
        private void FakeClickTheButton()
        {
            button.onClick.Invoke();
        }
    }
}
