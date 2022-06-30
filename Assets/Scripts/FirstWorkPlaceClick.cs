using MyGame;
using UnityEngine;

public class FirstWorkPlaceClick : MonoBehaviour
{
    public void Click()
    {
        GameModel.Get().Offices[0].Units[0].WorkPlaces[0].Click();
    }
}
