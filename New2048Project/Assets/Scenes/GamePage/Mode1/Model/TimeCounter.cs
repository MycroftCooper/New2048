using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public class UsingTime
    {
        public int hour;
        public int minute;
        public int second;
        public int millisecond;
        public UsingTime(float timeSpend)
        {
            setTime(timeSpend);
        }
        public void setTime(float timeSpend)
        {
            hour = (int)timeSpend / 3600;
            minute = ((int)timeSpend - hour * 3600) / 60;
            second = (int)timeSpend - hour * 3600 - minute * 60;
            millisecond = (int)((timeSpend - (int)timeSpend) * 1000);
        }
        public UsingTime(string time)
        {
            string[] strs = time.Split(':');
            hour = Convert.ToInt32(strs[0]);
            minute = Convert.ToInt32(strs[1]);
            second = Convert.ToInt32(strs[2].Split('.')[0]);
            millisecond = Convert.ToInt32(strs[2].Split('.')[1]);
        }
        public float getTimeSpend()
        {
            return hour * 3600 + minute * 60 + second + millisecond * 0.001f;
        }
        public override string ToString()
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", hour, minute, second, millisecond);
        }
    }
    public UsingTime usingTime;
    public static float timeSpend = 0.0f;
    public Text Timetext;

    private GameController GC;
    public void setTime(string usingTime)
    {
        this.usingTime = new UsingTime(usingTime);
        timeSpend = this.usingTime.getTimeSpend();
    }
    private void Awake()
    {
        GC = GameObject.Find("GameController").GetComponent<GameController>();
        Timetext = gameObject.GetComponent<Text>();
        usingTime = new UsingTime(0f);
    } 
    private void Update()
    {
        if(!GC.IsPause())
        {
            timeSpend += Time.deltaTime;
            usingTime.setTime(timeSpend);
            Timetext.text = usingTime.ToString();
        }
    }
}
