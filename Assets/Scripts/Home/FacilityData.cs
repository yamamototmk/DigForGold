using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public abstract class FacilityData
{
    public int level = 1;
}
[System.Serializable]
public class Data_Mine : FacilityData
{
    public Data_Mine()
    {
       // lastGetTime = DateTime.Now.ToBinary();
    }
    public long lastGetTime;
}
[System.Serializable]

public class Data_Exchanger : FacilityData
{
    public int oreRate;
    public int goldRate;
    public int rubyRate;
    public int sapphireRate;
}
[System.Serializable]

public class Data_BlackSmith : FacilityData
{
    public bool pickAutoRepair;
    public bool armorAutoRepair;
}
public enum FacilityID { AutoMine }
