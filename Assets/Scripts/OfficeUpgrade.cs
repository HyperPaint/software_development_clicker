using UnityEngine;

class OfficeUpgrade : MonoBehaviour
{
    public void OfficeInternetUpgradeClick()
    {
        MyGame.GameModel.Get().Offices[0].PartInternet.BuyUpgrade();
    }

    public void OfficeClimateUpgradeClick()
    {
        MyGame.GameModel.Get().Offices[0].PartClimate.BuyUpgrade();
    }

    public void OfficeMusicUpgradeClick()
    {
        MyGame.GameModel.Get().Offices[0].PartMusic.BuyUpgrade();
    }
}

