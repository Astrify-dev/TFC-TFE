
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class S_switchOption : MonoBehaviour
{
    [SerializeField] Animator _animator;
    private int index = 0;

    [SerializeField] GameObject _volumeFirstSelect;
    [SerializeField] GameObject _videoFirstSelect;
    [SerializeField] Button _back;

    Inputs _inputs;

    private void Awake()
    {
        _inputs = new Inputs();

    }
    private void OnEnable()
    {
        index = 0;
        _inputs.UI.Enable();
        _inputs.UI.Switch.performed += Switch_performed;
    }

    private void OnDisable()
    {
        _inputs.UI.Disable();
        _inputs.UI.Switch.performed -= Switch_performed;
    }

    private void Switch_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        index++;
        if (index > 1) index = 0;

        if(index == 0)
        {
            EventSystem.current.SetSelectedGameObject(_volumeFirstSelect);
            _back.navigation = SetNav(_volumeFirstSelect);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(_videoFirstSelect);
            _back.navigation = SetNav(_videoFirstSelect);
        }

        _animator.SetInteger("IndexParam", index);
    }

    private Navigation SetNav(GameObject DownSelect)
    {
        Navigation nav = new Navigation();
        nav.mode = Navigation.Mode.Explicit;

        nav.selectOnDown = DownSelect.GetComponent<Selectable>();

        return nav;
    }
}
