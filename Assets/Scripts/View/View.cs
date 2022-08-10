using UnityEngine;

public class View : MonoBehaviour
{
    //show the view
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    //hide the view
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}