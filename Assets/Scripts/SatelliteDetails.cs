using UnityEngine;
using System.Collections;

public class SatelliteDetails : MonoBehaviour
{
    [SerializeField] private string missionText;
    [SerializeField] private string detailsText;

    public string MissionText
    {
        get { return missionText; }
        set { missionText = value; }
    }

    public string DetailsText
    {
        get { return detailsText; }
        set { detailsText = value; }
    }
}
