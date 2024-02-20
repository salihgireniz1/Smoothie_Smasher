using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDown
{

    private TextMeshProUGUI _countdownText; // Süreyi gösterecek TextMeshPro nesnesi

    public CountDown(TextMeshProUGUI countdownText)
    {
        _countdownText = countdownText;
    }

    public void UpdateTimer(float currentTime)
    {
        if (currentTime > 0f)
        {

            // Süreyi 00.0 saniye.salise formatında metin olarak göster
            int seconds = Mathf.FloorToInt(currentTime);
            int milliseconds = Mathf.FloorToInt((currentTime * 1000f) % 1000f);
            string timeText = string.Format("{0:00}.{1:0}", seconds, milliseconds / 100);
            _countdownText.text = timeText;
        }
        else
        {
            _countdownText.text = "00.0"; // Süre tamamlandıysa metni 00.0 olarak ayarla
        }
    }
}
