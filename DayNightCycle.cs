using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace diaonic
{
    public class DayNightCycle : MonoBehaviour
    {
        public float hour;
        public int minutes;
        public int days;
        public int year;
        public bool playerIsAwake;
        public float intervalTime = .2f; //TODO REMOVE PUBLIC AFTER TESTING
        public Season currentSeason;
        public bool gameHasSeasonData;
        public GameObject nightOverlay;

        public enum Season
        {
            Spring,
            Summer,
            Fall,
            Winter
        }

        private void Start()
        {
            LoadSeasonData();
            StartDay();
        }

        /// <summary>
        /// Loads the season data or creates a fresh gameS
        /// </summary>
        private void LoadSeasonData()
        {
            if(gameHasSeasonData)
            {
                //load data here
            } else
            {
                //new game
                hour = 6.0f;
                minutes = 0;
                days = 0;
                year = 0;
                currentSeason = Season.Spring;
            }
        }
        private void StartDay()
        {
            playerIsAwake = true;
            hour = 6.00f;
            minutes = 0;
            Color tmp = nightOverlay.GetComponent<SpriteRenderer>().color;
            tmp.a = 0.37f;
            nightOverlay.GetComponent<SpriteRenderer>().color = tmp;
            StartCoroutine(TimeCycle());
        }

        private void EndDay()
        {
            //TODO black screen, send player to cell or infirmary.
        }

        IEnumerator TimeCycle()
        {
            while (playerIsAwake)
            {
                yield return new WaitForSeconds(intervalTime);
                IncrementGameTime();
                ReportTime();
            }
        }

        private void IncrementGameTime()
        {
            minutes += 10;
            if (minutes >= 60) {
                minutes = 0;
                hour += 1.0f;
                AdjustDarkness();
            }        
            
            if(hour >= 26)
            {
                hour = 6.0f;
                IncrementDays();
            }
        }

        private void IncrementDays()
        {
            days += 1;
            if (days >= 30)
            {
                IncrementGameSeason();
                days = 0;
            }
        }

        private void IncrementGameSeason()
        {
            if (currentSeason == Season.Winter)
            {
                currentSeason = 0;
                year++;
            }
            else
                currentSeason += 1;
        }

        private void ReportTime()
        {
            string meridiem = GenerateMeridiem();
            float standardHour = GenerateStandardFormatHour();

            if(minutes <= 0)
                Debug.Log("Time: " + standardHour + ":00" + " " + meridiem);
            else
                Debug.Log("Time: " + standardHour + ":" + minutes + " " + meridiem);
        }

        private float GenerateStandardFormatHour()
        {
            float stdHr = 0.0f;
            if (hour >= 13)
                stdHr = hour - 12.0f;
            else
                stdHr = hour;

            return stdHr;
        }

       private string GenerateMeridiem()
        {
            if(hour < 13)
            {
                return "AM";
            } else
            {
                return "PM";
            }
        }

        private void AdjustDarkness()
        {
            Color tmp = nightOverlay.GetComponent<SpriteRenderer>().color;
            if(hour < 13)
            {
                tmp.a -= .13f;
            } else
            {
                tmp.a += .13f;
            }
            nightOverlay.GetComponent<SpriteRenderer>().color = tmp;
        }
    }
}
