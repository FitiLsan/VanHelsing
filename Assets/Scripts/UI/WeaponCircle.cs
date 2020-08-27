using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class WeaponCircle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    #region PrivateFields

    private Image _parentImage;
    private Image _childImage;

    private Color _parentAlfa;
    private Color _childAlfa;

    private const float PARENT_IMAGE_NON_DEDICATED_ALFA = 0.3f;
    private const float CHILD_IMAGE_NON_DEDICATED_ALFA = 0.4f;
    private const float PARENT_IMAGE_DEDICATED_ALFA = 0.6f;
    private const float CHILD_IMAGE_DEDICATED_ALFA = 0.7f;


    private const float PARENT_IMAGE_SCALE_NON_DEDICATED = 0.7f;
    private const float CHILD_IMAGE_SCALE_NON_DEDICATED = 0.75f;
    private const float IMAGE_DEDICATED_SCALE = 0.85f;


    #endregion


    #region Fields

    public Text txt;
    public string weaponTxt;

    #endregion


    #region UnityMethods

    private void Awake()
    {
        _parentImage = GetComponent<Image>();
        _parentAlfa = _parentImage.color;
        _childImage = GetComponentInChildren<Image>();
        _childAlfa = _parentImage.color;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (_parentImage != null && _childImage != null)
        {
            _parentAlfa.a = PARENT_IMAGE_DEDICATED_ALFA;
            _childAlfa.a = CHILD_IMAGE_DEDICATED_ALFA;
            _parentImage.rectTransform.localScale = new Vector3(IMAGE_DEDICATED_SCALE, IMAGE_DEDICATED_SCALE, IMAGE_DEDICATED_SCALE);
            _parentImage.color = _parentAlfa;
            _childImage.rectTransform.localScale = new Vector3(IMAGE_DEDICATED_SCALE, IMAGE_DEDICATED_SCALE, IMAGE_DEDICATED_SCALE);
            _childImage.color = _childAlfa;
            txt.text = weaponTxt;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (_parentImage != null && _childImage != null)
        {
            _parentAlfa.a = PARENT_IMAGE_NON_DEDICATED_ALFA;
            _childAlfa.a = CHILD_IMAGE_NON_DEDICATED_ALFA;
            _parentImage.rectTransform.localScale = new Vector3(PARENT_IMAGE_SCALE_NON_DEDICATED, PARENT_IMAGE_SCALE_NON_DEDICATED, PARENT_IMAGE_SCALE_NON_DEDICATED);
            _parentImage.color = _parentAlfa;
            _childImage.rectTransform.localScale = new Vector3(CHILD_IMAGE_SCALE_NON_DEDICATED, CHILD_IMAGE_SCALE_NON_DEDICATED, CHILD_IMAGE_SCALE_NON_DEDICATED);
            _childImage.color = _childAlfa;
        }
    }

    #endregion

}
