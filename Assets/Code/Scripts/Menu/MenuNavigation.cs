using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] private Button[] buttons; 
    private int currentIndex = 0;

    void Start()
    {
        if (buttons.Length > 0)
        {
            buttons[currentIndex].Select();
        }
        else
        {
            Debug.LogWarning("No buttons assigned in the array.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            NavigateUp();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            NavigateDown();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            buttons[currentIndex].onClick.Invoke();
        }
    }

    void NavigateUp()
    {
        if (buttons.Length > 0)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
            }
            else
            {
                currentIndex = buttons.Length - 1;
            }
            buttons[currentIndex].Select();
        }
    }

    void NavigateDown()
    {
        if (buttons.Length > 0)
        {
            if (currentIndex < buttons.Length - 1)
            {
                currentIndex++;
            }
            else
            {
                currentIndex = 0;
            }
            buttons[currentIndex].Select();
        }
    }
}

